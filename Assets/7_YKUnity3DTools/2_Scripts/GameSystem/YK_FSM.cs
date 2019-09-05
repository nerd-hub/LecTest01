/// By Yunkyu Choi (C) 
/// Ver 3.
/// 2017. 11. 08
/// 

#if UNITY_EDITOR
#define _DEBUG_
#endif

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



public class YK_FSM : MonoBehaviour
{
	// Standard Events
	public enum YK_STD_FSM_EVENT
	{
		NONE,
		FIRST,
		GO_NEXT,
		RESTART,
		LAST,
	}

	// State
	protected string m_ID = "";    // ID for distinguish state machine

	protected delegate IEnumerator StateDelegator();
	protected StateDelegator m_StateDelegator = null;
	protected string m_PrevState = "";
	protected string m_State = "";
	protected YK_STD_FSM_EVENT m_CurEvent;

	/// Events
	protected bool m_DoEvent = false;

	#region Help Functions ----------------------------------------------------

	protected void SetNextState(StateDelegator a_State)
	{
		m_PrevState = m_State;
		m_StateDelegator = a_State;
	}


	#endregion Help Functions -------------------------------------------------

	#region Monobehaviour -----------------------------------------------------

	// Use this for initialization
	void OnEnable()
	{
		//m_CurEvent = YK_STD_FSM_EVENT.NONE;

		m_StateDelegator = State_First;

		InitEvents();

		StartCoroutine(MainLoop());
	}

	#endregion Monobehaviour --------------------------------------------------

	#region State Machine Loop ------------------------------------------------

	protected IEnumerator MainLoop()
	{
		m_ID = GetInstanceID() + " : " + Time.realtimeSinceStartup;

		while (true)
		{
			m_State = m_StateDelegator.Method.Name;

#if _DEBUG_
			Debug.Log("- Game Object : " + name +
				" \t -- Coroutine ID " + m_ID +
				" \t -- Next State Name " + m_State);
#endif


#if _DEBUG_
			Debug.Log("=== Start State : " + m_StateDelegator.Method.Name +
					" State in GameObject " + gameObject.ToString() + "] ID : " + m_ID);
#endif

			yield return StartCoroutine(m_StateDelegator());


#if _DEBUG_
			Debug.Log("=== Move to State " + m_StateDelegator.Method.Name +
				" From [" + m_PrevState +
					"] in [" + gameObject.ToString() + "] ID : " + m_ID);
#endif

			InitEvents();
			
		}
	}

	#endregion State Machine Loop ---------------------------------------------


	#region Events ------------------------------------------------------------


	virtual protected void InitEvents()
	{
#if _DEBUG_
		Debug.Log("Init Event");
#endif
		m_CurEvent = YK_STD_FSM_EVENT.NONE;
		m_DoEvent = false;
	}

	// No name event
	virtual public void OutsideEventEnum()
	{
#if _DEBUG_
		Debug.Log("OE");
#endif
		m_DoEvent = true;
	}

	virtual public void OutsideEventEnum(YK_STD_FSM_EVENT a_Event)
	{
#if _DEBUG_
		Debug.Log("OE Enum " + a_Event.ToString());
#endif
		m_CurEvent = a_Event;
		m_DoEvent = true;
	}

	virtual public void OutsideEventString(string a_Event)
	{
#if _DEBUG_
		Debug.Log("OE string " + a_Event);
#endif
		m_CurEvent = (YK_STD_FSM_EVENT)Enum.Parse(typeof(YK_STD_FSM_EVENT), a_Event);
		m_DoEvent = true;
	}

	#endregion Events ---------------------------------------------------------


	#region Objects for Actions -----------------------------------------------



	#endregion Objects for Actions --------------------------------------------


	#region States ------------------------------------------------------------

	virtual protected IEnumerator State_First()
	{
		/// Enter Action --------
		
		/// Repeat Action --------
		while (!m_DoEvent)
		{

			yield return new WaitForSeconds(0.3f);
		}


		/// End Action --------

		/// Move to Next State
		switch (m_CurEvent)
		{	
			case YK_STD_FSM_EVENT.FIRST:
				SetNextState(State_First);
				break;
			case YK_STD_FSM_EVENT.GO_NEXT:
				SetNextState(State_Loop);
				break;
			default:
				SetNextState(State_Error);
				break;
		}
		
	}

	virtual protected IEnumerator State_Loop()
	{
		/// Enter Action --------
		
		/// Repeat Action --------
		while (!m_DoEvent)
		{

			yield return new WaitForSeconds(0.3f);
		}


		/// End Action --------
		/// Move to Next State
		switch (m_CurEvent)
		{
			case YK_STD_FSM_EVENT.FIRST:
				SetNextState(State_First);
				break;
			case YK_STD_FSM_EVENT.GO_NEXT:
				SetNextState(State_Loop);
				break;
			default:
				SetNextState(State_Error);
				break;
		}

	}

	virtual protected IEnumerator State_Error()
	{
		/// Enter Action --------

		/// Repeat Action --------
		while (!m_DoEvent)
		{
			Debug.Log(" State Error!! in [" + gameObject.ToString() + "] ID : " + m_ID);
			yield return new WaitForSeconds(0.3f);
		}
		
		/// End Action --------
		
	}
	#endregion States ---------------------------------------------------------
}
