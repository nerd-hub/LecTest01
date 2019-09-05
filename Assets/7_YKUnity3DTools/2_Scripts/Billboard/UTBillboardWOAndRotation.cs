using UnityEngine;
using System.Collections;

public class UTBillboardWOAndRotation : MonoBehaviour {

   // 쿼터니언을 사용해서 회전할지 오일러 각을 사용해서 회전 할지 정한다.
   public bool m_UseQuaternion = false;

   // 오일러 각을 사용 할 경우 X, Y, Z 축에 대한 회전 값을 사용한다.
   public Vector3 m_EulerRotation = new Vector3(0f, 0f, 0f);

   // 쿼터니언을 직접 사용하여 회전 해 준다.
   public Quaternion m_Rotation = new Quaternion(0f, 0f, 0f, 0f);

   
   void Update () {

      // 자신의 Z축이 메인 카메라를 향하도록 회전 시킨다.
      transform.LookAt(Camera.main.transform);

      // 원하는 축이 카메라를 향하도록 회전해 준다.
      if (m_UseQuaternion) // 쿼터니언 회전인 경우
      {
         // 카메라 회전과 쿼터니언 회전을 모두 적용해 준다.
         transform.rotation *= m_Rotation;
      }
      else // 오일러 회전인 경우
      {
         // 카메라 회전과 쿼터니언을 변환된 오일러 회전을 적용해 준다.
         transform.rotation *= Quaternion.Euler(m_EulerRotation);

         m_Rotation = transform.rotation;
         // 윗 줄 대신 아래 처럼 해도 된다.
         //transform.Rotate(m_EulerRotation);
      }
   }
}
