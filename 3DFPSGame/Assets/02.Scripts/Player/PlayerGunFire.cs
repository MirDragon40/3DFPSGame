using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{
    // 목표: 마우스 왼쪽 버튼을 누르면 시선이 바라보는 방향으로 총을 발사하고 싶다.
    // 필요 속성
    // - 총알 튀는 이펙트 프리펩
    public ParticleSystem HitEffect;
    // 구현 순서:
    // 1. 만약에 마우스 왼쪽 버튼을 누르면 
    // 2. 레이(광선)을 생성하고, 위치와 방향을 설정한다.
    // 3. 레이를 발사한다. 
    // 4. 레이가 부딫힌 대상의 정보를 받아온다.
    // 5. 부딪힌 위치에 (총알이 튀는) 이펙트를 생성한다. 


    private void Update()
    {
        // 1. 만약에 마우스 왼쪽 버튼을 누르면 
        if (Input.GetMouseButtonDown(0))  // 마우스 왼쪽 버튼 0
        {
            // 2. 레이(광선)을 생성하고, 위치와 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, direction: Camera.main.transform.forward);
            // 3. 레이를 발사한다. 
            Physics.Raycast(ray);
            // 4. 레이가 부딫힌 대상의 정보를 받아온다.
            RaycastHit hitInfo;
            bool IsHit = Physics.Raycast(ray, out hitInfo);
            if (IsHit)
            {
                // 5. 부딪힌 위치에 (총알이 튀는) 이펙트를 생성한다. 
                //Debug.Log(hitInfo.point);
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. 이펙트가 쳐다보는 방향을 부딪힌 위치의 법선 벡터로 한다. 
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();  // 파티클도 오디오와같이 play를 사용해주어야 한다.
            }
        }

    }
}
