using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1��Ī ���� 

public class FPSCamera : MonoBehaviour
{

    // ��ǥ: ���콺�� �����ϸ� ī�޶� �� �������� ȸ����Ű�� �ʹ�.
    // �ʿ� �Ӽ�: ȸ�� �ӵ�, ������ x������ y���� 
    // - ȸ�� �ӵ�
    public float RotationSpeed = 200;  // �ʴ� 200�ʱ��� ȸ�� ������ �ӵ�
    // ������ x������ y ����
    public float _mx = 0 ;
    public float _my = 0;

    /** ī�޶� �̵� **/
    // ��ǥ: ī�޶� ĳ������ ������ �̵���Ű��ʹ�.
    // �ʿ�Ӽ�:
    // - ĳ������ �� ��ġ
    public Transform Target;


    // ����:
    // 1. ���콺�� �Է�(Drag) �޴´�.
    // 2. ���콺 �Է� ���� �̿��� ȸ�� ������ ���Ѵ�. 
    // 3. ȸ�� ���� ȸ���Ѵ�.


    private void Start()
    {
        // ���콺 Ŀ���� ����� �ڵ�
        Cursor.visible = false;
        // ���콺�� ������Ű�� �ڵ�
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
        
    {
        // 1. ĳ������ �� ��ġ�� ī�޶� �̵���Ų��. 
        transform.position = Camera.main.transform.position;

        // 1. ���콺�� �Է�(Drag) �޴´�.
        float mouseX = Input.GetAxis("Mouse X");  // ���⿡ ���� -1 ~ 1 ������ �� ��ȯ
        float mouseY = Input.GetAxis("Mouse Y");
        //Debug.Log(message: $"GetAxis: {mouseX} {mouseY}");
        //Vector2 mousePosition = Input.mousePosition;  // ��¥ ���콺 ��ǥ��
        //Debug.Log(message: $"mousePosition: {mousePosition.x}, {mousePosition.y}");

        // 2. ���콺 �Է� ���� �̿��� ȸ�� ������ ���Ѵ�. 
        Vector3 rotationDir = new Vector3(mouseX,mouseY, z: 0);
       // rotationDir.Normalize();   // ����ȭ



        // 3. ȸ�� ���� ȸ���Ѵ�.
        // ���ο� ��ġ = ���� ��ġ * ���� * �ӵ� * �ð�
        // ���ο� ȸ�� = ���� ȸ�� + ���� * �ӵ� * �ð� 
        //transform.eulerAngles += rotationDir * RotationSpeed * Time.deltaTime;

        // 3-1. ȸ�� ���⿡ ���� ���콺 �Է� �� ��ŭ �̸� ������Ų��. 
        _mx += + rotationDir.x *RotationSpeed* Time.deltaTime;
        _my += + rotationDir.y *RotationSpeed* Time.deltaTime;

        // 4. �ü��� ���� ������ -90 ~ 90�� ���̷� �����ϰ� �ʹ�. 
        //Vector3 rotation = transform.eulerAngles;

        //rotation.x = Mathf.Clamp(value: rotation.x, min: -90f, max: 90f);
        //rotation.y = Mathf.Clamp(value: rotation.y, min: -200f, max:200f);
        //transform.eulerAngles = rotation;

        _my = Mathf.Clamp(value: _my, min: -90f, max: 90f);

        transform.eulerAngles = new Vector3(x: -_my, y: _mx, z: 0);

        /*
        if (rotation.x < -90)
        {
            rotation.x = 90;
        }
        else if (rotation.x > 90)
        {
            rotation.x = 90;
        }
        */

        // ���Ϸ� ������ ����
        // 1. ������ ����
        // 2. 0���� �۾����� -1�� �ƴ� 359(360-1)�� �ȴ�. (����Ƽ ���ο��� �̷��� �ڵ�����)
        // �� ���� �ذ��� ���ؼ� �츮�� �̸� ������ ����� �Ѵ�. 

    }
}
