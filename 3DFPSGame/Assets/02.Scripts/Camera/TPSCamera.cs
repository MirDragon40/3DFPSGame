using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 3인칭 슈팅 (Third Person Shooter) 
// 게임상의 캐릭터가 보는 시점이 아닌, 캐릭터를 보는 시점 즉, 3인칭 관찰자 시점의 카메라


public class TPSCamera : MonoBehaviour
{

    // ** 카메라 회전 **
    // 목표: 마우스를 조작하면 카메라를 캐릭터 중심에 다라 그 방향으로 회전시키고 싶다. 

    // 필요 속성:
    // - 회전 속도
    public float RotationSpeed = 200;
    // - 타겟(캐릭터)
    public Transform Target;
    public Vector3 Offset = new Vector3(x: 0, y: 3f, z: -3f);  // 오프셋 더해주기

    /*
    // 누적할 x 각도와 y 각도
    private float _mx = 0;
    private float _my = 0;
    */


    // 구현 순서:
    // 1. 카메라를 타겟(플레이어에서 조금 더 떨어진 거리)으로 이동시킨다. (따라다니게 한다.)
    // 2. 플레이어를 쳐다보게 한다. 
    // 3. 마우스 입력을 받는다. 
    // 4. 마우스 입력에 따라 회전 방향을 구한다.
    // 5. 타겟 중심으로 회전 방향에 맞게 회전한다.

    public float a;
    private void LateUpdate()
    {
        transform.localPosition = Target.position + Offset;
        transform.LookAt(Target);

        /*
        // 구현 순서:
        // 1. 카메라를 타겟(플레이어에서 조금 더 떨어진 거리)으로 이동시킨다. (따라다니게 한다.)

        // 3. 마우스 입력을 받는다. 
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 4. 마우스 입력에 따라 회전 방향을 구한다.
        _mx += mouseX * RotationSpeed * Time.deltaTime;
        _my += mouseY * RotationSpeed * Time.deltaTime;
        */

        // 5. 타겟 중심으로 회전 방향에 맞게 회전한다. 
        Vector2 xy = CameraManager.Instance.XY;
        transform.RotateAround(point: Target.position, axis: Vector3.up, xy.x);
        transform.RotateAround(point: Target.position, axis: transform.right, -xy.y);

        transform.localPosition = Target.position - (transform.forward * Offset.magnitude) + transform.up * (Offset.y-a); //+Vector3



    }
}
