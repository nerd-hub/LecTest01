using System.Collections;
using UnityEngine;


namespace YKUnityTools
{

public class CameraWalker : MonoBehaviour {

	//public Transform m_PathParent;
	public BezierSpline m_Path;
	private GameObject m_PathGO;

	public bool m_GoingForward = true;

	[Range(0.1f, 100f)]public float m_Duration = 5f;
	public enum MoveMode { LookTarget, LookForward, };
	public MoveMode m_CamMode = MoveMode.LookTarget;

	public Transform m_CamSysParent;
	public Transform m_StartTrans;
	public Transform m_EndTrans;
	private Quaternion m_Start;
	private Quaternion m_End;
	
	public SplineWalkerMode m_Mode;
	public AnimationCurve m_EaseCurve;
	
	private float m_Progress = 0f;
	[Range(0.001f, 0.1f)] public float m_Dt = 0.03f;
	// Editor System support
	[HideInInspector] public Vector3 m_BackupPos;
	[HideInInspector] public Quaternion m_BackupRot;
	[Range(0, 10000)] public int m_CounterMax = 1000;
	public int m_Counter = 0;
	private void Reset()
	{
		m_BackupPos = transform.position;
		m_BackupRot = transform.rotation;
	}

	private void OnEnable()
	{
		m_BackupPos = transform.position;
		m_BackupRot = transform.rotation;
	}

	private void Start()
	{
		ResetProgress();

		if(m_Path == null) CreatePath();
		else{
			SetPath();
		}
	}

	public float GetProgress()
	{
		return m_Progress;
	}

	public void ResetProgress()
	{
		m_Progress = 0f;
	}

	public void SetStartEnd(Transform a_Start, Transform a_End)
	{
		m_StartTrans = a_Start;
		m_EndTrans = a_End;		
	}

	public void CreatePath()
	{
		DeletePath();

		m_PathGO = new GameObject("Cam Path");
		m_PathGO.AddComponent<BezierSpline>();
		m_Path = m_PathGO.GetComponent<BezierSpline>();
		m_Path.Reset();

		//if(m_CamSysParent != null)
		//	m_Path.transform.parent = m_PathParent;
		//else m_Path.transform.parent = (new GameObject("CamPathRoot")).transform;
		
		if(m_EndTrans != null && m_StartTrans != null)
		{	
			SetPath();
		}
		else{ Debug.LogError(" Start and End points not set!! "); }
	}

	public void DeletePath()
	{
		if(m_Path != null)
		{
			if(m_PathGO != null) DestroyImmediate(m_PathGO);
		}
	}

	// SetPath from 
	void SetPath()
	{
		BezierSpline spline = m_Path;
		Transform nextCam = m_EndTrans;

		spline.SetControlPoint(0, 
			spline.transform.InverseTransformPoint(m_StartTrans.position));
		spline.SetControlPoint(1,
			spline.transform.InverseTransformPoint(m_StartTrans.position - m_StartTrans.forward));
		spline.SetControlPoint(3,
			spline.transform.InverseTransformPoint(nextCam.position));
		spline.SetControlPoint(2,
			spline.transform.InverseTransformPoint(nextCam.position - nextCam.forward));	
		
		m_Start = m_StartTrans.rotation;
		m_End = m_EndTrans.rotation;
	}

	public float Move()
	{
		//StopCoroutine("Movement");
		//StartCoroutine("Movement");

		if(m_Path == null || m_StartTrans == null || m_EndTrans == null) 
			return -1f;
		
		MoveProcess();
		return m_Progress;
	}

	IEnumerator Movement ()
    {
		if(m_StartTrans == null || m_EndTrans == null) yield return null;

		else{

			while(Vector3.Distance(m_StartTrans.position, m_EndTrans.position) > 0.01f)
			{
				MoveProcess();

				yield return null;
			}
		}
    }

	private void LateUpdate () {
		MoveProcess();
		m_Dt = Time.deltaTime;
	}

	private void MoveProcess()
	{
		if (m_GoingForward) {
			m_Progress += m_Dt / m_Duration;
			if (m_Progress > 1f) {
				if (m_Mode == SplineWalkerMode.Once) {
					m_Progress = 1f;
					enabled = false;
				}
				else if (m_Mode == SplineWalkerMode.Loop) {
					m_Progress -= 1f;
				}
				else {
					m_Progress = 2f - m_Progress;
					m_GoingForward = false;
				}
			}
		}
		else {
			m_Progress -= m_Dt / m_Duration;
			if (m_Progress < 0f) {
				m_Progress = -m_Progress;
				m_GoingForward = true;
			}
		}

		float easedProgress = m_EaseCurve.Evaluate(m_Progress);

		Vector3 position = m_Path.GetPoint(easedProgress);
		
		transform.localPosition = position;

		switch (m_CamMode)
		{
			case MoveMode.LookTarget:
				transform.rotation = Quaternion.Slerp(m_Start, m_End, easedProgress);
				break;
			case MoveMode.LookForward:
				transform.LookAt(position + m_Path.GetDirection(easedProgress));
				break;
			default:
				break;
		}
		
	}

	public void CreateCamSys()
	{
		GameObject camGo = new GameObject("CamSys", typeof(LookCamSystem));
			camGo.transform.SetPositionAndRotation (transform.position, transform.rotation);

		if(m_CamSysParent == null) 
			m_CamSysParent = (new GameObject("CamSysRoot")).transform;

		camGo.transform.parent = m_CamSysParent;
	}
}// class
}// namespace