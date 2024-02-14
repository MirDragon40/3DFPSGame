using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{


    // ��ǥ: Ű���� ����Ű(wasd)�� ������ ĳ���͸� �ٶ󺸴� ���� �������� �̵���Ű�� �ʹ�. 
    // �Ӽ�:
    // - �̵��ӵ�
    public float MoveSpeed = 5;     // �Ϲ� �ӵ�
    public float RunSpeed = 10;    // �ٴ� �ӵ�

    public float Stamina = 100;             // ���¹̳�
    public const float MaxStamina = 100;    // ���¹̳� �ִ뷮
    public float StaminaConsumeSpeed = 33f; // �ʴ� ���¹̳� �Ҹ�
    public float StaminaChargeSpeed = 50;  // �ʴ� ���¹̳� ������

    public int JumpTimes = 1;  // ���� Ƚ��

    [Header("���¹̳� �����̴� UI")]
    public Slider StaminaSliderUI;

    private CharacterController _characterController;

    // ��ǥ: �����̽� �ٸ� ������ ĳ���͸� �����ϰ� �ʹ�. 
    // �ʿ� �Ӽ�:
    // - ���� �Ŀ� ��
    public float JumpPower = 10f;

    // ���� ����:
    // 1. ���࿡ [Spacebar] ��ư�� ������ ���� && ���̸�...

    // 2. �÷��̾�� y�࿡ �־� ���� �Ŀ��� �����Ѵ�. 


    // ��ǥ: ĳ���Ϳ��� �߷��� �����ϰ� �ʹ�. 
    // �ʿ�Ӽ�: 
    // - �߷� ��
    private float _gravity = -20;  // �߷� ����
    // - ������ �߷� ����: y�� �ӵ�
    private float _yVelocity = 0f;
    // ���� ����:
    // 1. �߷� ���ӵ��� �����ȴ�. 
    // 2. �÷��̾�� y�࿡ �־� �߷��� �����Ѵ�.

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Stamina = MaxStamina;
    }

    // ���� ����
    // 1. Ű �Է� �ޱ�
    // 2. 'ĳ���Ͱ� �ٶ󺸴� ����'�� �������� ���ⱸ�ϱ�
    // 3. �̵��ϱ�
    void Update()
    {
        // 1. Ű �Է� �ޱ�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. 'ĳ���Ͱ� �ٶ󺸴� ����'�� �������� ���ⱸ�ϱ�
        Vector3 dir = new Vector3(h, 0, v);             // ���� ��ǥ�� (������ ��������) 
        dir.Normalize();
        // Transforms direction from local space to world space.
        dir = Camera.main.transform.TransformDirection(dir); // �۷ι� ��ǥ�� (������ ��������)

        // �ǽ� ���� 1. Shift ������ ������ ���� �ٱ�
        float speed = MoveSpeed; // 5
        if (Input.GetKey(KeyCode.LeftShift)) // �ǽ� ���� 2. ���¹̳� ����
        {
            // - Shfit ���� ���ȿ��� ���¹̳��� ������ �Ҹ�ȴ�. (3��)
            Stamina -= StaminaConsumeSpeed * Time.deltaTime; // �ʴ� 33�� �Ҹ�
            if (Stamina > 0)
            {
                speed = RunSpeed;
            }
        }
        else
        {
            // - �ƴϸ� ���¹̳��� �Ҹ� �Ǵ� �ӵ����� ���� �ӵ��� �����ȴ� (2��)
            Stamina += StaminaChargeSpeed * Time.deltaTime; // �ʴ� 50�� ����
        }

        Stamina = Mathf.Clamp(Stamina, 0, 100);
        StaminaSliderUI.value = Stamina / MaxStamina;  // 0 ~ 1;//

        // ���� ����
        /*
        // 1. ���࿡ [Spacebar] ��ư�� ������ ���� && ���̸�...
        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)  
        {
            // 2. �÷��̾�� y�࿡ �־� ���� �Ŀ��� �����Ѵ�. 
            _yVelocity = JumpPower;
        }
        */
        // ���� ���� ����: 2�� ���� ����
        // 
        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVelocity = JumpPower;
            JumpTimes = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded == false)
        {
            if (JumpTimes == 1)
            {
                _yVelocity = JumpPower;
                JumpTimes = 0;
            }
        }







        // 3-1. �߷� ����
        // 1. �߷� ���ӵ��� �����ȴ�. 
        _yVelocity = _yVelocity + _gravity * Time.deltaTime;
        // 2. �÷��̾�� y�࿡ �־� �߷��� �����Ѵ�.
        dir.y = _yVelocity;

        // 3-2. �̵��ϱ�
        //transform.position += speed * dir * Time.deltaTime;
        _characterController.Move(motion: dir * speed * Time.deltaTime);

        // 9�� Ű�� ������ FPS �������� ��ȯ
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // FPS ī�޶� ���� ��ȯ
            //CameraManager.instance.fpsCamera.enabled = true;
            //CameraManager.instance.tpsCamera.enabled = false;

            //CameraManager.instance.SetFPSCameraMode();

            CameraManager.instance.SetCameraMode(CameraMode.FPS);
        }
        // 0�� Ű�� ������ TPS �������� ��ȯ
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // TPS ī�޶� ���� ��ȯ ver1
            //CameraManager.instance.fpsCamera.enabled = false;
            //CameraManager.instance.tpsCamera.enabled = true;

            // TPS ī�޶� ���� ��ȯ ver2
            //CameraManager.instance.SetTPSCameramode();

            // TPS ī�޶� ���� ��ȯ ver3
            CameraManager.instance.SetCameraMode(CameraMode.TPS);
        }

    }
}