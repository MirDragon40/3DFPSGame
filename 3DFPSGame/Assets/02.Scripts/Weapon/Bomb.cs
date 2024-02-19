using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // ��ǥ: ����ź ���� ���� ������ ��� ����
    // �ʿ� �Ӽ�:
    // - ����
    public float ExplosionRadius;
    // ���� ����:
    // 1. ���� ��
    // 2. ���� �ȿ� �ִ� ��� �ݶ��̴��� ã�´�.
    // 3. ã�� �ݶ��̴� �߿��� Ÿ�� ������(IHitable) ������Ʈ�� ã�´�.
    // 4. Hit() �Ѵ�.


    public int Damage = 60;

    // �ǽ� ���� 8. ����ź�� ������ ��(����� ��) ���� ����Ʈ�� �ڱ� ��ġ�� �����ϱ�
    public GameObject BombEffectPrefab;


    // 1. ���� ��
    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false); // â�� �ִ´�.

        GameObject effect = Instantiate(BombEffectPrefab);
        effect.transform.position = this.gameObject.transform.position;

        // 2. ���� �ȿ� �ִ� ��� �ݶ��̴��� ã�´�.
        // -> ������, ������ �Լ��� Ư�� ����(radius) �ȿ��ִ� Ư�� ���̾���� ���� ������Ʈ��
        //    �ݶ��̴� ������Ʈ���� ��� ã�� �迭�� ��ȯ�ϴ� �Լ�
        //    ������ ����: ���Ǿ�, ť��, ĸ��

        int layer = LayerMask.NameToLayer("Monster") /*| LayerMask.NameToLayer("Player")*/;
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, layer);   // ��Ʈ �� ������
        //Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, 1 << 8 | 1 << 9);

        // 3. ã�� �ݶ��̴� �߿��� Ÿ�� ������(IHitable) ������Ʈ�� ã�Ƽ� Hit() �Ѵ�.
        foreach (Collider collider in colliders)
        {
            IHitable hitable = collider.gameObject.GetComponent<IHitable>();
            if (hitable != null)
            {
                hitable.Hit(Damage);
            }
        }

    }
}
