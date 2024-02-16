using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{
    // ��ǥ: ���콺 ���� ��ư�� ������ �ü��� �ٶ󺸴� �������� ���� �߻��ϰ� �ʹ�.
    // �ʿ� �Ӽ�
    // - �Ѿ� Ƣ�� ����Ʈ ������
    public ParticleSystem HitEffect;
    // ���� ����:
    // 1. ���࿡ ���콺 ���� ��ư�� ������ 
    // 2. ����(����)�� �����ϰ�, ��ġ�� ������ �����Ѵ�.
    // 3. ���̸� �߻��Ѵ�. 
    // 4. ���̰� �΋H�� ����� ������ �޾ƿ´�.
    // 5. �ε��� ��ġ�� (�Ѿ��� Ƣ��) ����Ʈ�� �����Ѵ�. 


    private void Update()
    {
        // 1. ���࿡ ���콺 ���� ��ư�� ������ 
        if (Input.GetMouseButtonDown(0))  // ���콺 ���� ��ư 0
        {
            // 2. ����(����)�� �����ϰ�, ��ġ�� ������ �����Ѵ�.
            Ray ray = new Ray(Camera.main.transform.position, direction: Camera.main.transform.forward);
            // 3. ���̸� �߻��Ѵ�. 
            Physics.Raycast(ray);
            // 4. ���̰� �΋H�� ����� ������ �޾ƿ´�.
            RaycastHit hitInfo;
            bool IsHit = Physics.Raycast(ray, out hitInfo);
            if (IsHit)
            {
                // 5. �ε��� ��ġ�� (�Ѿ��� Ƣ��) ����Ʈ�� �����Ѵ�. 
                //Debug.Log(hitInfo.point);
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. ����Ʈ�� �Ĵٺ��� ������ �ε��� ��ġ�� ���� ���ͷ� �Ѵ�. 
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();  // ��ƼŬ�� ������Ͱ��� play�� ������־�� �Ѵ�.
            }
        }

    }
}
