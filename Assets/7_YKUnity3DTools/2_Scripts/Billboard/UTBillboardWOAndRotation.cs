using UnityEngine;
using System.Collections;

public class UTBillboardWOAndRotation : MonoBehaviour {

   // ���ʹϾ��� ����ؼ� ȸ������ ���Ϸ� ���� ����ؼ� ȸ�� ���� ���Ѵ�.
   public bool m_UseQuaternion = false;

   // ���Ϸ� ���� ��� �� ��� X, Y, Z �࿡ ���� ȸ�� ���� ����Ѵ�.
   public Vector3 m_EulerRotation = new Vector3(0f, 0f, 0f);

   // ���ʹϾ��� ���� ����Ͽ� ȸ�� �� �ش�.
   public Quaternion m_Rotation = new Quaternion(0f, 0f, 0f, 0f);

   
   void Update () {

      // �ڽ��� Z���� ���� ī�޶� ���ϵ��� ȸ�� ��Ų��.
      transform.LookAt(Camera.main.transform);

      // ���ϴ� ���� ī�޶� ���ϵ��� ȸ���� �ش�.
      if (m_UseQuaternion) // ���ʹϾ� ȸ���� ���
      {
         // ī�޶� ȸ���� ���ʹϾ� ȸ���� ��� ������ �ش�.
         transform.rotation *= m_Rotation;
      }
      else // ���Ϸ� ȸ���� ���
      {
         // ī�޶� ȸ���� ���ʹϾ��� ��ȯ�� ���Ϸ� ȸ���� ������ �ش�.
         transform.rotation *= Quaternion.Euler(m_EulerRotation);

         m_Rotation = transform.rotation;
         // �� �� ��� �Ʒ� ó�� �ص� �ȴ�.
         //transform.Rotate(m_EulerRotation);
      }
   }
}
