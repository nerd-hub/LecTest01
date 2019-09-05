using UnityEngine;
using System.Collections;

// 화면정렬 빌보드 스크립트 이다.
public class UTBillboardSA : MonoBehaviour
{
   void Update()
   {
      // 카메라와 동일하게 회전시켜 준다. (월드 좌표축에 대한 회전)
      // transform.rotation은 쿼터니언 값임에 유의
      transform.rotation = Camera.main.transform.rotation;

      // 아래 처럼 하면 루트 게임오브젝트가 아닐 경우 문제가 생길 수 있음
      //transform.localRotation = Camera.main.transform.rotation;
   }
}
