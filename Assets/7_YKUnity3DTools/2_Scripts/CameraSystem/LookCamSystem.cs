using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YKUnityTools
{

public class LookCamSystem : MonoBehaviour {

	public bool m_AutoLookAt = true;
	public Transform m_LookTarget;
	public Camera m_CamForCopy;
	public Camera m_Cam;

	public GameObject m_MarkerPrefab;
	public LayerMask m_GroundMask;
	public GameObject m_MarkerGO;


	//public Transform m_NextCamTrans;

	//public BezierSpline m_TestLine;

	private void Reset()
	{
		m_GroundMask = 1 << LayerMask.NameToLayer("Ground");

		AddCamera();			
	}
		
	private void Awake()
	{
		if(m_Cam != null)
		{
			m_Cam.enabled = false;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetMarkerPos()
	{
		if(m_MarkerGO == null) return;

		RaycastHit hit;
		// Does the ray intersect any objects excluding the player layer
		if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f,		m_GroundMask))
		{
			Vector3 markerFwd = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
			m_MarkerGO.transform.position = hit.point;
			m_MarkerGO.transform.LookAt( hit.point + markerFwd);
		}
		else
		{
			Debug.Log("Ground Did not Hit");
		}
	}

	public void AddCamera()
	{
		LookCamSystem camSys = this;
		Transform camSysTrans = camSys.transform;

		if (camSys.gameObject.GetComponentInChildren<Camera>() == null)
		{
			if (camSys.m_CamForCopy == null)
			{
				camSys.gameObject.AddComponent<Camera>();
			}
			else{
				UTGlobalUtility.AddComponent<Camera>( camSys.gameObject, camSys.m_CamForCopy);
			}
			
			camSys.m_Cam = camSys.GetComponent<Camera>();
			camSys.m_Cam.depth = -100;
		}

		return;
	}
}


}