using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 목표: 수류탄 폭발 범위 데미지 기능 구현
    // 필요 속성:
    // - 범위
    public float ExplosionRadius;
    // 구현 순서:
    // 1. 터질 때
    // 2. 범위 안에 있는 모든 콜라이더를 찾는다.
    // 3. 찾은 콜라이더 중에서 타격 가능한(IHitable) 오브젝트를 찾는다.
    // 4. Hit() 한다.


    public int Damage = 60;

    // 실습 과제 8. 수류탄이 폭발할 때(사라질 때) 폭발 이펙트를 자기 위치에 생성하기
    public GameObject BombEffectPrefab;

    private Collider[] _colliders = new Collider[10];


    // 1. 터질 때
    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false); // 창고에 넣는다.

        GameObject effect = Instantiate(BombEffectPrefab);
        effect.transform.position = this.gameObject.transform.position;

        // 2. 범위 안에 있는 모든 콜라이더를 찾는다.
        // -> 피직스, 오버랩 함수는 특정 영역(radius) 안에있는 특정 레이어들의 게임 오브젝트의
        //    콜라이더 컴포넌트들을 모두 찾아 배열로 변환하는 함수
        //    영역의 형태: 스피어, 큐브, 캡슐

        int layer = LayerMask.NameToLayer("Monster") /*| LayerMask.NameToLayer("Player")*/;
        int count = Physics.OverlapSphereNonAlloc(transform.position, ExplosionRadius,_colliders, layer); // 비트 합 연산자
        //Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, 1 << 8 | 1 << 9);

        // 3. 찾은 콜라이더 중에서 타격 가능한(IHitable) 오브젝트를 찾아서 Hit() 한다.
        for (int i = 0; i < count; i++)
        {
            Collider c = _colliders[i];
            IHitable hitable = c.gameObject.GetComponent<IHitable>();
            if (hitable != null)
            {
                hitable.Hit(Damage);
            }
        }

    }
}
