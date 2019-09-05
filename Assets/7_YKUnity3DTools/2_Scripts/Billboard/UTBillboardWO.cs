using UnityEngine;
using System.Collections;

public class UTBillboardWO : MonoBehaviour {

   // Update is called once per frame
   void Update () {

        transform.LookAt(Camera.main.transform);
   }
}


