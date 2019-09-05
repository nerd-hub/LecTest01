using System;
using System.Reflection;

using UnityEngine;
using System.Collections;


public static class UTGlobalUtility
{
	//Might not work on iOS.
	public static T GetCopyOf<T>(this Component comp, T other) where T : Component
	{
		 Type type = comp.GetType();
		 if (type != other.GetType()) return null; // type mis-match
		 BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
		 PropertyInfo[] pinfos = type.GetProperties(flags);
		 foreach (var pinfo in pinfos)
		 {
			  if (pinfo.CanWrite)
			  {
					try
					{
						 pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
					}
					catch { } // In case of NotImplementedException being thrown.
			  }
		 }
		 FieldInfo[] finfos = type.GetFields(flags);
		 foreach (var finfo in finfos)
		 {
			  finfo.SetValue(comp, finfo.GetValue(other));
		 }
		 return comp as T;
	}

	public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
	{
		 return go.AddComponent<T>().GetCopyOf(toAdd) as T;
	}//Example usage  Health myHealth = gameObject.AddComponent<Health>(enemy.health);

	
   public static bool IsBehindOfMainCamera(Camera a_Camera, Vector3 a_WorldPos)
   {
      // Cull the object if it is back side of camera
      Plane cameraFrontPlane = new Plane( 
         a_Camera.transform.TransformDirection(Vector3.forward), a_Camera.transform.position);

      float distFromCamPlane = cameraFrontPlane.GetDistanceToPoint(a_WorldPos);
      
      if(distFromCamPlane < 0)		
         return true;
      else
         return false;
   }

   
   /// <summary>
   /// Return true if a_WorldPos is back side of Z plane of a_Transform
   /// </summary>
   /// <param name="a_Transform"> Pivot transform </param>
   /// <param name="a_WorldPos"> Target point </param>
   /// <returns></returns>
   public static bool IsBehindOfZPlane(Transform a_Transform, Vector3 a_WorldPos)
   {
      float distFromZPlane = a_Transform.TransformPoint(a_WorldPos).z;

      if (distFromZPlane < 0)
         return true;
      else
         return false;
   }

	public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
	{
		int place = Source.LastIndexOf(Find);

		if (place == -1)
			return Source;

		string result = Source.Remove(place, Find.Length).Insert(place, Replace);
		return result;
	}

}

