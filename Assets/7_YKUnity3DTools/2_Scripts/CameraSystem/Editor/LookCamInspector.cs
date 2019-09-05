using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YKUnityTools
{

[CustomEditor(typeof(LookCamSystem))]
public class LookCamInspector : Editor 
{

	public GUIContent m_GizmoSet;
	//SceneView.OnSceneFunc m_OnSceneVar;
	
	private void Awake()
	{
		if(m_GizmoSet == null) m_GizmoSet = new GUIContent();

		//m_GizmoSet.image = (Texture2D)EditorGUIUtility.Load("YKUnityTools/CamSysIcon.png");
		m_GizmoSet.text = "YK Cam Sys";
		m_GizmoSet.tooltip = "Camera movement system";
	}

	private void OnEnable()
	{
		//if (m_OnSceneVar != null) SceneView.onSceneGUIDelegate -= m_OnSceneVar;
  //          m_OnSceneVar = SceneView.onSceneGUIDelegate += OnSceneEnabled;

		//SceneView.onSceneGUIDelegate -= OnScene;
		//SceneView.onSceneGUIDelegate += OnScene;
		
		// if GroundMask is not set, set it to "Ground" layer as default
		LookCamSystem camSys = target as LookCamSystem;

		Debug.Log("Mask val : " + camSys.m_GroundMask.value);
		if(camSys.m_GroundMask.value == 0)
		{
			//camSys.m_GroundMask = 1 << LayerMask.NameToLayer("Ground");
		}
	}

	private void OnDisable()
	{
		//if (m_OnSceneVar != null) SceneView.onSceneGUIDelegate -= m_OnSceneVar;
  //          m_OnSceneVar = SceneView.onSceneGUIDelegate += OnSceneDisabled;


		//SceneView.onSceneGUIDelegate -= OnScene;
		
	}
	private void OnSceneEnabled(SceneView a_SceneView)
	{
		//OnSceneGUI();
	}

	private void OnSceneDisabled(SceneView a_SceneView)
	{
		//OnSceneGUI();
	}

	private void OnSceneGUI()
	{
		EditorGUI.BeginChangeCheck();
		
		LookCamSystem camSys = target as LookCamSystem;
		Transform camSysTrans = camSys.transform;
		Transform targetObj = camSys.m_LookTarget;
		bool isTargetObj = targetObj ? true : false;
		Vector3 targetPos = Vector3.zero;

		Vector3 camPos = camSys.transform.position;

		if(isTargetObj)	targetPos = targetObj.position;
		else	targetPos = camPos + camSysTrans.forward;
		
		// Look at line
		Handles.color = Color.yellow;
		Handles.DrawDottedLine(camPos, targetPos, 3);
		Handles.color = Color.cyan;
		if(Event.current.type == EventType.Repaint)
			Handles.ArrowHandleCap(0, camPos, camSysTrans.rotation, 0.5f, EventType.Repaint);

		// Look at transform
		if(camSys.m_AutoLookAt)
			camSysTrans.LookAt(targetPos, Vector3.up);
		
		camSys.SetMarkerPos();

		// GUI
		Handles.BeginGUI();
		Rect gizmoRect = HandleUtility.WorldPointToSizedRect(camSys.transform.position, m_GizmoSet, GUIStyle.none);
		//GUI.DrawTexture(gizmoRect, m_GizmoSet.image);
		Handles.Label(camPos, m_GizmoSet);
		Handles.EndGUI();
	}

	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		LookCamSystem camSys = target as LookCamSystem;
		Transform camSysTrans = camSys.transform;
		Transform targetObj = camSys.m_LookTarget;
		bool isTargetObj = targetObj ? true : false;
		Vector3 targetPos = Vector3.zero;

		Vector3 camPos = camSys.transform.position;

		if(isTargetObj)	targetPos = targetObj.position;
		else	targetPos = camPos + camSysTrans.forward;
		
		GUILayout.Space(10);
		// Add camera
		if (GUILayout.Button("Add Camera"))
		{
			Undo.RecordObject(camSys, "Add Camera");
			EditorUtility.SetDirty(camSys);  // To ask save when quit

			camSys.AddCamera();
		}
		
		GUILayout.Space(10);
		// Look at target
		if (GUILayout.Button("Look At Target")) {
			Undo.RecordObject(camSys, "Look At Target");
			EditorUtility.SetDirty(camSys);	// To ask save when quit

			if(camSys.m_Cam == null) camSys.AddCamera();

			camSysTrans.LookAt(targetPos, Vector3.up);
		}
		
		GUILayout.Space(10);
		// Set Marker
		if (GUILayout.Button("Set Marker")) {
			Undo.RecordObject(camSys, "Set Marker");
			EditorUtility.SetDirty(camSys);	// To ask save when quit
			
			if(camSys.m_MarkerGO == null && camSys.m_MarkerPrefab != null)
			{
					camSys.m_MarkerGO = GameObject.Instantiate(camSys.m_MarkerPrefab,
						Vector3.zero, Quaternion.identity, camSysTrans );
			}
			camSys.SetMarkerPos();
			
		}
	}
	

}
}
