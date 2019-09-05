using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YKUnityTools
{

[CustomEditor(typeof(CameraWalker))]
public class CameraWalkerInspector : Editor 
{
	public GUIContent m_GizmoSet;
	
	private void Awake()
	{
		if(m_GizmoSet == null) m_GizmoSet = new GUIContent();

		//m_GizmoSet.image = (Texture2D)EditorGUIUtility.Load("YKUnityTools/CamSysIcon.png");
		m_GizmoSet.text = "YK Cam Walker";
		m_GizmoSet.tooltip = "Camera Walker";
	}
	
	float m_Prog = 0;
	bool m_MoveCamOn = false;
	bool m_MoveCamPause = false;
	float m_DtBackup = 0.1f;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		CameraWalker camWalk = target as CameraWalker;
		Transform camWalkTrans = camWalk.transform;
		
		Vector3 camPos = camWalk.transform.position;
		
		GUILayout.Space(10);
		// Move Camera
		if (GUILayout.Button("Camera Path Test"))
		{
			if(!m_MoveCamOn && !EditorApplication.isPlaying)
			{
				camWalk.m_BackupPos = camWalkTrans.position;
				camWalk.m_BackupRot = camWalkTrans.rotation;

				Undo.RecordObject(camWalk, "Camera Path Test");
				EditorUtility.SetDirty(camWalk);  // To ask save when quit
			
				camWalk.ResetProgress();
				camWalk.CreatePath();
				camWalk.m_Counter = 0;
				m_Prog = 0;
				m_MoveCamOn = true;
			}
		}

		// Cam animation
		if(m_MoveCamOn && camWalk.m_Counter < camWalk.m_CounterMax)
			// && m_Prog >= 0f && m_Prog <= 0.99f)
		{
			EditorUtility.SetDirty(camWalk);

			m_Prog = camWalk.Move();
			camWalk.m_Counter++;
			//SceneView.RepaintAll();
			//Debug.Log("Progress " + m_Prog);
		}
		else
		{
			if(m_MoveCamOn)
			{
				camWalkTrans.position = camWalk.m_BackupPos;
				camWalkTrans.rotation = camWalk.m_BackupRot;

				camWalk.DeletePath();
			}
			m_MoveCamOn = false;
		}

		//Debug.Log("Counter : " + camWalk.m_Counter + " Prog : " + m_Prog + " On : " + m_MoveCamOn);
		
		GUILayout.Space(10);
		// Stop Camera
		if (GUILayout.Button("Camera Path Stop"))
		{
			if(m_MoveCamOn)
			{
				camWalkTrans.position = camWalk.m_BackupPos;
				camWalkTrans.rotation = camWalk.m_BackupRot;

				camWalk.DeletePath();
			}
			
			camWalk.ResetProgress();
			camWalk.m_Counter = 0;
			m_Prog = 0;
			m_MoveCamOn = false;
		}

		GUILayout.Space(10);
		// Pause Camera
		if (GUILayout.Button("Camera Path Pause"))
		{
			if(m_MoveCamOn && !m_MoveCamPause)
			{
				m_DtBackup = camWalk.m_Dt;
				camWalk.m_Dt = 0;
				m_MoveCamPause = true;
			}
			else if(m_MoveCamOn && m_MoveCamPause)
			{
				camWalk.m_Dt = m_DtBackup;
				
				m_MoveCamPause = false;
			}
			
		}
		
		GUILayout.Space(10);
		// Create LookCam Here
		if (GUILayout.Button("Create Look Cam Here"))
		{
			camWalk.CreateCamSys();
		}
	}	
	
	

}// class

}// namespace