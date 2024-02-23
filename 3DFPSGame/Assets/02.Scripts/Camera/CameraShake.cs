using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 어떤 형태의 카메라 쉐이크인지 스크립트 이름에 적어주는 것이 좋다.
public class CameraShake : MonoBehaviour
{

    // 목표: 카메라를 일정 시간동안 랜덤하게 흔들고 싶다.
    // 필요 속성:
    // - 쉐이킹 시간
    public float ShakingDuration = 0.2f;
    // - 쉐이킹 누적 시간
    private float _shakingTimer = 0f;
    // - 쉐이킹 파워
    public float ShakingPower = 0.025f;
    // - 쉐이킹 중이냐?
    private bool _isShaking = false;

    // 구현 순서:
    // 1. 시간이 흐른다.
    // 2. 랜덤하게 흔든다.
    // 3. 일정 시간이 지나면 초기화
    public void Shake()
    {
        _shakingTimer = 0f;
        _isShaking=true;
    }
    private void Update()
    {
        if (!_isShaking)
        {
            return;
        }


        // 구현 순서:
        // 1. 시간이 흐른다.
        _shakingTimer += Time.deltaTime;

        // 2. 랜덤하게 흔든다.
        transform.position = Vector3.zero + Random.insideUnitSphere * ShakingPower;

        // 3. 일정 시간이 지나면 초기화
        if (_shakingTimer >= ShakingDuration)
        {
            _isShaking = false;
            transform.position = Vector3.zero;
        }
    }
}
