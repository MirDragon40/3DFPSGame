using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 3��Ī ���� (Third Person Shooter) 
// ���ӻ��� ĳ���Ͱ� ���� ������ �ƴ�, ĳ���͸� ���� ���� ��, 3��Ī ������ ������ ī�޶�


public class TPSCamera : MonoBehaviour
{

    // ** ī�޶� ȸ�� **
    // ��ǥ: ���콺�� �����ϸ� ī�޶� ĳ���� �߽ɿ� �ٶ� �� �������� ȸ����Ű�� �ʹ�. 

    // �ʿ� �Ӽ�:
    // - ȸ�� �ӵ�
    public float RotationSpeed = 200;
    // - Ÿ��(ĳ����)
    public Transform Target;
    public Vector3 Offset = new Vector3(x: 0, y: 3f, z: -3f);  // ������ �����ֱ�
    // ������ x ������ y ����
    private float _mx = 0;
    private float _my = 0;



    // ���� ����:
    // 1. ī�޶� Ÿ��(�÷��̾�� ���� �� ������ �Ÿ�)���� �̵���Ų��. (����ٴϰ� �Ѵ�.)
    // 2. �÷��̾ �Ĵٺ��� �Ѵ�. 
    // 3. ���콺 �Է��� �޴´�. 
    // 4. ���콺 �Է¿� ���� ȸ�� ������ ���Ѵ�.
    // 5. Ÿ�� �߽����� ȸ�� ���⿡ �°� ȸ���Ѵ�.


    void Start()
    {

    }

    private void LateUpdate()
    {
        // ���� ����:
        // 1. ī�޶� Ÿ��(�÷��̾�� ���� �� ������ �Ÿ�)���� �̵���Ų��. (����ٴϰ� �Ѵ�.)
        if (CameraManager.instance.Mode == CameraMode.TPS)
        {
            transform.position = Target.position + Offset;
        }

        // 2. �÷��̾ �Ĵٺ��� �Ѵ�. 
        // Rotates the transform so the forward vector points at target's current position.
        if (CameraManager.instance.Mode == CameraMode.TPS)
        {
            transform.LookAt(Target);

        }

        // 3. ���콺 �Է��� �޴´�. 
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 4. ���콺 �Է¿� ���� ȸ�� ������ ���Ѵ�.
        _mx += mouseX * RotationSpeed * Time.deltaTime;
        _my += mouseY * RotationSpeed * Time.deltaTime;

        // 5. Ÿ�� �߽����� ȸ�� ���⿡ �°� ȸ���Ѵ�. 
        transform.RotateAround(point: Target.position, axis: Vector3.up, _mx);
        transform.RotateAround(point: Target.position, axis: transform.right, -_my);

        //transform.position = Target.position - (transform.forward * Offset.magnitude); //+Vector3



    }
}
