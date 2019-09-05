using UnityEngine;
using System.Collections;

// 카메라와 동일한 회전을 적용한 후 일정 각도 만큼 추가로 회전해 주는
// 화면정렬 빌보드 스크립트 이다.
public class UTBillboardSAAndRotation : MonoBehaviour
{
   // 쿼터니언을 사용해서 회전할지 오일러 각을 사용해서 회전 할지 정한다.
   public bool m_UseQuaternion = false;

   // 오일러 각을 사용 할 경우 X, Y, Z 축에 대한 회전 값을 사용한다.
   public Vector3 m_EulerRotation = new Vector3(0f, 0f, 0f);

   // 쿼터니언을 직접 사용하여 회전 해줄 경우 사용 할 쿼터니언
   public Quaternion m_Rotation = new Quaternion (0f, 0f, 0f, 0f);

   void Update()
   {
      // 카메라의 현재 회전각을 쿼터니언으로 얻어온다.
      Quaternion camRotation = Camera.main.transform.rotation;

      // 원하는 축이 카메라를 향하도록 쿼터니언 또는 오일러 회전 적용
      if (m_UseQuaternion) // 쿼터니언 회전인 경우
      {
         // 카메라 회전과 쿼터니언 회전을 모두 적용해 준다.
         transform.rotation = camRotation * m_Rotation;
      }
      else // 오일러 회전인 경우
      {
         // 카메라 회전과 쿼터니언을 변환된 오일러 회전을 적용해 준다.
         transform.rotation = camRotation * Quaternion.Euler(m_EulerRotation);
      }
   }
}

