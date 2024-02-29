using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour, IHitable
{
    // 실습과제 19. 총으로  3번 맞출 시 사라지게 구현
    private int _hitCount = 0;

    public GameObject ExplosionPaticlePrefab;
    private Rigidbody _rigidbody;

    public Material[] materials;

    public float UpPower = 7f;

    public int Damage = 70;
    public float ExplosionRadius = 10f;

    // 드럼통이 폭발중인가?
    private bool _isExplosion = false;
    
    


    public void Hit(DamageInfo damageInfo)
    {
        _hitCount += 1;
        if (_hitCount >= 3)
        {
            Explosion();
        }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        // Random.Range를 이용하여 랜덤한 인덱스를 생성함.
        int index = Random.Range(0, materials.Length);
        meshRenderer.material = materials[index];
    }



    private void Explosion()
    {
        if (_isExplosion)
        {
            return;
        }
        _isExplosion = true;


        GameObject explosion = Instantiate(ExplosionPaticlePrefab);
        explosion.transform.position = this.transform.position;

        _rigidbody.AddForce(Vector3.up * UpPower, ForceMode.Impulse);
        _rigidbody.AddTorque(new Vector3(1, 0, 1) * UpPower / 2f);

        // 실습 과제 22. 드럼통 폭발할 때 주변 Hitable한 Monster와 Player에게 데미지 70
        // 1. 폭발 범위 내 콜라이더 찾기
        int findLayer = LayerMask.GetMask("Player") | LayerMask.GetMask("Monster") | LayerMask.GetMask("Environment");
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, findLayer);

        // 2. 콜라이더 내에서 Hitable 찾기
        foreach (Collider c in colliders)
        {
            IHitable hitable = null;
            if (c.TryGetComponent<IHitable>(out hitable))
            {
                // 3. 데미지 주기
                DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
                hitable.Hit(damageInfo);
            }
        }

        // 실습과제 23. 드럼통 폭발할 때 주변 드럼통도 같이 폭발되게 구현 (연쇄폭발)
        int environmentLayer = LayerMask.GetMask("Environment");
        Collider[] environmentColliders = Physics.OverlapSphere(transform.position, ExplosionRadius, environmentLayer);
        foreach (Collider c in environmentColliders)
        {
            Drum drum = null;
            
            if (c.TryGetComponent<Drum>(out drum))
            {
                // 3. 데미지 주기
                drum.Explosion();
            }
        }

        ItemObjectFactory.Instance.MakePercent(transform.position);

        StartCoroutine(Kill_Coroutine());

    }

    private IEnumerator Kill_Coroutine()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject, 3f);

    }

}
