using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    // 목표: 키보드 방향키(wasd)를 누르면 캐릭터가 바라보는 방향 기준으로 이동시키고 싶다. 
    // 속성:
    // - 이동속도
    public float MoveSpeed = 5;
    // 구현 순서
    // 1. 키 입력 받기
    // 2. '캐릭터가 바라보는 방향'을 기준으로 방향 구하기
    // 3. 이동하기


    void Update()
    {
        // 1. 키 입력 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. 방향구하기
        Vector3 dir = new Vector3(x: h, y: 0, z: -v);          // 로컬 좌표계 (나만의 동서남북)
        dir.Normalize();
        // Transforms direction form local space to world space.
        dir = Camera.main.transform.TransformDirection(dir);  // 글로벌 좌표계 (세상의 동서남북)


        // 3. 이동하기
        transform.position += MoveSpeed * dir * Time.deltaTime;

    }

}
