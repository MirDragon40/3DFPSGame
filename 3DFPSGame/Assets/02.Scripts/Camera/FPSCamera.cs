using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1인칭 슈팅 

public class FPSCamera : MonoBehaviour
{

    // 목표: 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶다.
    // 필요 속성: 회전 속도, 누적할 x각도와 y각도 
    // - 회전 속도
    public float RotationSpeed = 200;  // 초당 200초까지 회전 가능한 속도
    // 누적할 x각도와 y 각도
    public float _mx = 0;
    public float _my = 0;

    /** 카메라 이동 **/
    // 목표: 카메라를 캐릭터의 눈으로 이동시키고싶다.
    // 필요속성:
    // - 캐릭터의 눈 위치
    public Transform Target;


    // 순서:
    // 1. 마우스를 입력(Drag) 받는다.
    // 2. 마우스 입력 값을 이용해 회전 방향을 구한다. 
    // 3. 회전 방향 회전한다.


    private void Start()
    {
        // 마우스 커서를 숨기는 코드
        Cursor.visible = false;
        // 마우스를 고정시키는 코드
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void LateUpdate()

    {
        // 1. 캐릭터의 눈 위치로 카메라를 이동시킨다. 
        transform.position = Target.transform.position;

        // 1. 마우스를 입력(Drag) 받는다.
        float mouseX = Input.GetAxis("Mouse X");  // 방향에 따라 -1 ~ 1 사이의 값 변환
        float mouseY = Input.GetAxis("Mouse Y");
        //Debug.Log(message: $"GetAxis: {mouseX} {mouseY}");
        //Vector2 mousePosition = Input.mousePosition;  // 진짜 마우스 좌표값
        //Debug.Log(message: $"mousePosition: {mousePosition.x}, {mousePosition.y}");

        // 2. 마우스 입력 값을 이용해 회전 방향을 구한다. 
        Vector3 rotationDir = new Vector3(mouseX, mouseY, z: 0);
        // rotationDir.Normalize();   // 정규화



        // 3. 회전 방향 회전한다.
        // 새로운 위치 = 이전 위치 * 방향 * 속도 * 시간
        // 새로운 회전 = 이전 회전 + 방향 * 속도 * 시간 
        //transform.eulerAngles += rotationDir * RotationSpeed * Time.deltaTime;

        // 3-1. 회전 방향에 따라 마우스 입력 값 만큼 미리 누적시킨다. 
        _mx += +rotationDir.x * RotationSpeed * Time.deltaTime;
        _my += +rotationDir.y * RotationSpeed * Time.deltaTime;

        // 4. 시선의 상하 제한을 -90 ~ 90도 사이로 제한하고 싶다. 
        //Vector3 rotation = transform.eulerAngles;

        //rotation.x = Mathf.Clamp(value: rotation.x, min: -90f, max: 90f);
        //rotation.y = Mathf.Clamp(value: rotation.y, min: -200f, max:200f);
        //transform.eulerAngles = rotation;

        _my = Mathf.Clamp(value: _my, min: -90f, max: 90f);


        if (CameraManager.instance.Mode == CameraMode.FPS)
        {
            transform.eulerAngles = new Vector3(x: -_my, y: _mx, z: 0);
        }

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

        // 오일러 각도의 단점
        // 1. 짐벌락 현상
        // 2. 0보다 작아지면 -1이 아닌 359(360-1)가 된다. (유니티 내부에서 이렇게 자동연산)
        // 위 문제 해결을 위해서 우리가 미리 연산을 해줘야 한다. 

    }
}
