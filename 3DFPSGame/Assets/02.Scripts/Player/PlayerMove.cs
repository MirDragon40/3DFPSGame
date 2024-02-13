using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    // ��ǥ: Ű���� ����Ű(wasd)�� ������ ĳ���Ͱ� �ٶ󺸴� ���� �������� �̵���Ű�� �ʹ�. 
    // �Ӽ�:
    // - �̵��ӵ�
    public float MoveSpeed = 5;
    // ���� ����
    // 1. Ű �Է� �ޱ�
    // 2. 'ĳ���Ͱ� �ٶ󺸴� ����'�� �������� ���� ���ϱ�
    // 3. �̵��ϱ�


    void Update()
    {
        // 1. Ű �Է� �ޱ�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. ���ⱸ�ϱ�
        Vector3 dir = new Vector3(x: h, y: 0, z: -v);          // ���� ��ǥ�� (������ ��������)
        dir.Normalize();
        // Transforms direction form local space to world space.
        dir = Camera.main.transform.TransformDirection(dir);  // �۷ι� ��ǥ�� (������ ��������)


        // 3. �̵��ϱ�
        transform.position += MoveSpeed * dir * Time.deltaTime;

    }

}
