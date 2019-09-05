using UnityEngine;
using System.Collections;

public class UTBillboardAxialYLocalWO : MonoBehaviour
{
	[SerializeField] private GameObject m_Camera;
	void Start()
	{
		if(m_Camera == null){
			m_Camera = GameObject.FindGameObjectWithTag("MainCamera");// GameObject.Find("Camera (eye)");
		}
	}
   void Update()
   {
      // 현재 위치와 카메라 간의 벡터를 구한다.
      Vector3 objToCamVecWorld = m_Camera.transform.position - transform.position;

      // 위의 벡터를 로컬 좌표계로 변환 한 후 정규화 한다.
      Vector3 objToCamVec = transform.InverseTransformDirection(objToCamVecWorld);
      objToCamVec.Normalize();

#if UNITY_EDITOR
      Debug.DrawRay(transform.position, objToCamVecWorld, Color.red);
      Debug.DrawRay(transform.position,
         transform.TransformDirection(Vector3.right), Color.cyan);
      Debug.DrawRay(transform.position,
         transform.TransformDirection(Vector3.forward), Color.gray);
#endif // UNITY_EDITOR

      // 현재 위치와 카메라 사이를 연결하는 벡터에서 x, z 성분만 가져온다. (2차원에 투영)
      Vector2 objToCamProj = new Vector2(objToCamVec.x, objToCamVec.z);

      // TextMesh의 노멀방향(로컬 -Z 축방향이며 위의 2차원 평면에서는 -Vector2.up 방향)과
      // 투영된 벡터간의 각도를 구한다.
      float angleToRot = Vector2.Angle(-Vector2.up, objToCamProj);

#if UNITY_EDITOR
      Debug.DrawRay(transform.position, transform.TransformDirection(
            new Vector3(objToCamProj.x, 0f, objToCamProj.y)), Color.green);
      //print("Angle : " + angleToRot);
#endif // UNITY_EDITOR

      // 좌측과 우측 회전을 투영된 x의 길이를 사용하여 구별해 준다.
      angleToRot *= (objToCamProj.x > 0 ? -1 : 1);

      // Y축에 대해서 회전해 준다.
      transform.Rotate(0f, angleToRot, 0f);
   }
}
