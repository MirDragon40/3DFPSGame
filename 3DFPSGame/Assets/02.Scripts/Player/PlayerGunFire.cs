using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunFire : MonoBehaviour
{

    public int Damage = 1;
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


    // - �߻� ��Ÿ��
    [Header("Gun Ÿ�̸�")]
    public float FireCoolTime = 0.2f;
    private float _timer;

    // - �� �� �ִ� �Ѿ� ����
    [Header("�Ѿ� ���� ����")]
    public int BulletRemainCount = 30;
    public int MaxBulletCount = 30;

    // - �Ѿ� ���� �ؽ�Ʈ UI
    public Text BulletNumUI;
    // - �Ѿ��� ���� �����ΰ�?
    private const float RELOAD_TIME = 1.5f; // ������ �ð�
    private bool _isReloading = false;      // ������ ���̳�?
    public GameObject ReloadTextObject;     // - �Ѿ� ������ �ε� �ؽ�Ʈ UI


    private void Start()
    {
        // �Ѿ� ���� �ʱ�ȭ
        BulletRemainCount = MaxBulletCount;
        RefreshUI();
    }
    private void Update()
    {
        _timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R) && BulletRemainCount < MaxBulletCount)
        {
            if (!_isReloading)
            {
                StartCoroutine(Reload_Coroutine());
            }
        }
        ReloadTextObject.SetActive(_isReloading);






        // 1. ���࿡ ���콺 ���� ��ư�� ���� ���� && ��Ÿ���� �� ���� ����
        if (Input.GetMouseButton(0) && _timer >= FireCoolTime && BulletRemainCount > 0)  // ���콺 ���� ��ư 0
        {
            // ������ ���
            if (_isReloading)
            {
                StopAllCoroutines();
                _isReloading = false;
            }

            BulletRemainCount--;
            RefreshUI();
            _timer = 0;

            // 2. ����(����)�� �����ϰ�, ��ġ�� ������ �����Ѵ�.
            Ray ray = new Ray(Camera.main.transform.position, direction: Camera.main.transform.forward);
            // 3. ���̸� �߻��Ѵ�. 
            Physics.Raycast(ray);
            // 4. ���̰� �΋H�� ����� ������ �޾ƿ´�.
            RaycastHit hitInfo;
            bool IsHit = Physics.Raycast(ray, out hitInfo);
            if (IsHit)
            {
                // �ǽ����� 18. �������� ���Ϳ��� ���� �� ���� ü�� ��� ��� ����
                IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
                if (hitObject != null)   // ���� �� �ִ� ģ���ΰ���?
                {
                    hitObject.Hit(Damage);
                }

                /*
                if (hitInfo.collider.CompareTag ("Monster"))
                {
                    Monster monster = hitInfo.collider.GetComponent<Monster>();
                    monster.Hit(Damage);
                }
                */

                // 5. �ε��� ��ġ�� (�Ѿ��� Ƣ��) ����Ʈ�� �����Ѵ�. 
                //Debug.Log(hitInfo.point);
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. ����Ʈ�� �Ĵٺ��� ������ �ε��� ��ġ�� ���� ���ͷ� �Ѵ�. 
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();  // ��ƼŬ�� ������Ͱ��� play�� ������־�� �Ѵ�.
            }
        }

    }



    private void RefreshUI()
    {
        BulletNumUI.text = $"{BulletRemainCount} / {MaxBulletCount}";
    }


    private IEnumerator Reload_Coroutine()
    {
        _isReloading = true;

        // RŰ ������ 1.5�� �� ������, (�߰��� �� ��� ������ �ϸ� ������ ���)
        yield return new WaitForSeconds(RELOAD_TIME);
        BulletRemainCount = MaxBulletCount;
        RefreshUI();

        _isReloading = false;
    }


}
