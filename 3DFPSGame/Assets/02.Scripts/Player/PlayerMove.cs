using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{

    // ��ǥ: Ű���� ����Ű(wasd)�� ������ ĳ���Ͱ� �ٶ󺸴� ���� �������� �̵���Ű�� �ʹ�. 
    // �Ӽ�:
    // - �̵��ӵ�
    public float MoveSpeed = 5f;   // �Ϲݼӵ�
    public float RunSpeed = 10f;   // �ٴ� �ӵ�
    public float Stamina = 10f;
    private bool isRunning = false;
    // ���� ����
    // 1. Ű �Է� �ޱ�
    // 2. 'ĳ���Ͱ� �ٶ󺸴� ����'�� �������� ���� ���ϱ�
    // 3. �̵��ϱ�

    [Header("���׹̳� �����̴� UI")]
    public Slider slider;


    void Update()
    {
        // 1. Ű �Է� �ޱ�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. ���ⱸ�ϱ�
        Vector3 dir = new Vector3(x: h, y: 0, z: v);          // ���� ��ǥ�� (������ ��������)
        dir.Normalize();
        // Transforms direction form local space to world space.
        dir = Camera.main.transform.TransformDirection(dir);  // �۷ι� ��ǥ�� (������ ��������)

        // 3. �̵��ϱ�
        transform.position += MoveSpeed * dir * Time.deltaTime;

        float ConsumeSpeed = 2f;
        float ChargeSpeed = 5f;

        float speed = MoveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;

            speed = RunSpeed;
            Stamina -= ConsumeSpeed * Time.deltaTime;
            slider.value = Stamina;

            slider.minValue = 0;
            if (Stamina < 0)
            {
                Stamina = 0;
            }
        }
        else
        {
            speed = MoveSpeed;
            Stamina += ChargeSpeed * Time.deltaTime;
            slider.value = Stamina;
            slider.maxValue = 10f;

            if (Stamina > 10)
            {
                Stamina = 10;
            }

        }
    }
}
