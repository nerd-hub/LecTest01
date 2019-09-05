using UnityEngine;
using System.Collections;

public class UTBillboardSAVertexUpdate : MonoBehaviour
{
   // 버텍스들의 위치 나타내는 게임오브젝트들
   public GameObject m_V1;
   public GameObject m_V2;
   public GameObject m_V3;
   public GameObject m_V4;

   // 각 버텍스들의 로컬 좌표계 위치
   public Vector3 m_posV1;
   public Vector3 m_posV2;
   public Vector3 m_posV3;
   public Vector3 m_posV4;

   // 빌보드 평면을 구성하는 정점들의 중심 위치
   public Vector3 m_BillboardCenterPos;

   // 빌보드 평면을 저장 할 메쉬
   public MeshFilter m_Mesh;

   void Awake()
   {
      // 게임 오브젝트들의 로컬 위치를 가져와서 최초 버텍스 위치로 지정해 준다.
      m_posV1 = m_V1.transform.localPosition;
      m_posV2 = m_V2.transform.localPosition;
      m_posV3 = m_V3.transform.localPosition;
      m_posV4 = m_V4.transform.localPosition;

      // 메쉬를 생성한 후 저장하기 위해서 메쉬필터를 얻어온다.
      m_Mesh = gameObject.AddComponent<MeshFilter>();
      
      // 빌보드 평면을 구성 할 버텍스들을 저장 할 배열을 생성한다.
      Vector3[] verts = new Vector3[4];

      // 각 버텍스들의 위치를 설정해 준다.
      verts[0] = m_posV1;
      verts[1] = m_posV2;
      verts[2] = m_posV3;
      verts[3] = m_posV4;

      // 버텍스들의 UV 좌표를 설정해 준다.
      Vector2[] uvs = new Vector2[4];
      uvs[0] = new Vector2(0f, 1f);
      uvs[1] = new Vector2(1f, 1f);
      uvs[2] = new Vector2(0f, 0f);
      uvs[3] = new Vector2(1f, 0f);

      // 버텍스 인덱스를 사용하여 메쉬를 구성하는 삼각형들을 구성한다.
      int[] tris = new int[6];

      tris[0] = 2;
      tris[1] = 0;
      tris[2] = 1;

      tris[3] = 2;
      tris[4] = 1;
      tris[5] = 3;

      // 새로운 메쉬를 생성한다.
      Mesh tmpMesh = new Mesh();

      // 생성한 메쉬에 버텍스와 UV, 삼각형 인덱스를 설정한다.
      tmpMesh.vertices = verts;
      tmpMesh.uv = uvs;
      tmpMesh.triangles = tris;

      // 노멀과 바운딩 볼륨을 계산한다.
      tmpMesh.RecalculateNormals();
      tmpMesh.RecalculateBounds();

      // 메쉬필터의 메쉬에 생성한 메쉬를 설정해 준다. (화면에 보여지게 됨)
      m_Mesh.mesh = tmpMesh;
   }


   void Update()
   {
      // 빌보드 평면의 중심점을 현재 게임오브젝트의 월드 좌표로 설정한다.
      m_BillboardCenterPos = transform.position;

      // 카메라의 트랜스폼을 얻어온다.
      Transform camTransform = Camera.main.transform;

      // 평면을 구성하는 4개의 버텍스 위치를 나타내는 각 게임 오브젝트의 
      // 월드상의 위치를 정한다.
      // 최초 생성 버텍스의 로컬 위치를 카메라 로컬=>월드 벡터로 변환 한 후 
      // 현 게임오브젝트의 중점 위치에 이 벡터를 더해 준다.
      // 결과적으로 카메라와 동일하게 월드에서 회전된 위치가 된다.
      m_V1.transform.position =
         m_BillboardCenterPos + camTransform.TransformDirection(m_posV1);

      m_V2.transform.position =
         m_BillboardCenterPos + camTransform.TransformDirection(m_posV2);

      m_V3.transform.position =
         m_BillboardCenterPos + camTransform.TransformDirection(m_posV3);

      m_V4.transform.position =
         m_BillboardCenterPos + camTransform.TransformDirection(m_posV4);

      // m_V1의 카메라 로컬 => 월드 변환 예를 보인다.
      // m_posV1 (vertex 1의 초기 로컬 벡터 방향)
      Debug.DrawRay(transform.position, m_posV1, Color.red);
      // m_posV1 벡터를 카메라 좌표계의 로컬 벡터를 월드 벡터로 변환 한 방향
      Debug.DrawRay(transform.position, 
         camTransform.TransformDirection(m_posV1), Color.green);

      // 새로운 위치의 정점들을 저장할 배열을 만든다.
      Vector3[] newVs = new Vector3[4];

      // 새로운 위치에 정점들을 생성한다.
      newVs[0] = m_V1.transform.localPosition;
      newVs[1] = m_V2.transform.localPosition;
      newVs[2] = m_V3.transform.localPosition;
      newVs[3] = m_V4.transform.localPosition;

      // 메쉬필터에 새로운 정점들을 설정하고 노멀과 바운딩 볼륨을 계산해 준다.
      m_Mesh.mesh.vertices = newVs;
      m_Mesh.mesh.RecalculateNormals();
      m_Mesh.mesh.RecalculateBounds();
   }

}