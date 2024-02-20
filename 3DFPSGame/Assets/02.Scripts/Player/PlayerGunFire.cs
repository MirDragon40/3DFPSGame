using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunFire : MonoBehaviour
{

    public Gun CurrentGun;   // 현재 들고있는 총
    private int _currentGunIndex;  // 현재 들고있는 총의 순서

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


    // - 발사 쿨타임
    private float _timer;

    private const int DefaultFOV = 60;
    private const int ZoomFOV = 20;
    private bool isZoomMode = false; // 줌 모드냐?

    // 총을 담는 인벤토리
    public List<Gun> GunInventory;

    // - 쏠 수 있는 총알 개수

    // - 총알 개수 텍스트 UI
    public Text BulletNumUI;
    // - 총알이 장전 상태인가?
    private bool _isReloading = false;      // 재장전 중이냐?
    public GameObject ReloadTextObject;     // - 총알 재장전 로딩 텍스트 UI

    // 무기 이미지 UI
    public Image GunImageUI;


    private void Start()
    {
        _currentGunIndex = 0;

        // 총알 개수 초기화
        RefreshUI();
        RefreshGun();

        // 처음 시작할 때 라이플 

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (isZoomMode)
            {
                isZoomMode = false;
                Camera.main.fieldOfView = DefaultFOV;
            }
            else
            {
                isZoomMode = true;
                Camera.main.fieldOfView = ZoomFOV;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            // 뒤로가기
            _currentGunIndex--;
            if (_currentGunIndex < 0)
            {
                _currentGunIndex = GunInventory.Count - 1;
            }
            CurrentGun = GunInventory[_currentGunIndex];
            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            // 앞으로 가기
            _currentGunIndex++;
            if (_currentGunIndex >= GunInventory.Count)
            {
                _currentGunIndex = 0;
            }
            CurrentGun = GunInventory[_currentGunIndex];
            RefreshGun();
            RefreshUI();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentGunIndex = 0;
            CurrentGun = GunInventory[0];
            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentGunIndex = 1;
            CurrentGun = GunInventory[1];
            RefreshGun();
            RefreshUI();


        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentGunIndex = 2;
            CurrentGun = GunInventory[2];
            RefreshGun();
            RefreshUI();

        }

        _timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R) && CurrentGun.BulletRemainCount < CurrentGun.MaxBulletCount)
        {
            if (!_isReloading)
            {
                StartCoroutine(Reload_Coroutine());
            }
        }

        ReloadTextObject.SetActive(_isReloading);


        // 1. 만약에 마우스 왼쪽 버튼을 누른 상태 && 쿨타임이 다 지난 상태
        if (Input.GetMouseButton(0) && _timer >= CurrentGun.FireCoolTime && CurrentGun.BulletRemainCount > 0)  // 마우스 왼쪽 버튼 0
        {
            // 재장전 취소
            if (_isReloading)
            {
                StopAllCoroutines();
                _isReloading = false;
            }

            CurrentGun.BulletRemainCount--;
            RefreshUI();
            _timer = 0;

            // 2. 레이(광선)을 생성하고, 위치와 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, direction: Camera.main.transform.forward);
            // 3. 레이를 발사한다. 
            Physics.Raycast(ray);
            // 4. 레이가 부딫힌 대상의 정보를 받아온다.
            RaycastHit hitInfo;
            bool IsHit = Physics.Raycast(ray, out hitInfo);
            if (IsHit)
            {
                // 실습과제 18. 레이저를 몬스터에게 맞출 시 몬스터 체력 닳는 기능 구현
                IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
                if (hitObject != null)   // 때릴 수 있는 친구인가요?
                {
                    hitObject.Hit(CurrentGun.Damage);
                }

                /*
                if (hitInfo.collider.CompareTag ("Monster"))
                {
                    Monster monster = hitInfo.collider.GetComponent<Monster>();
                    monster.Hit(Damage);
                }
                */

                // 5. 부딪힌 위치에 (총알이 튀는) 이펙트를 생성한다. 
                //Debug.Log(hitInfo.point);
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. 이펙트가 쳐다보는 방향을 부딪힌 위치의 법선 벡터로 한다. 
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();  // 파티클도 오디오와같이 play를 사용해주어야 한다.
            }
        }

        


    }


    private void RefreshUI()
    {
        GunImageUI.sprite = CurrentGun.ProfileImage;
        BulletNumUI.text = $"{CurrentGun.BulletRemainCount} / {CurrentGun.MaxBulletCount}";
    }


    private IEnumerator Reload_Coroutine()
    {
        _isReloading = true;

        // R키 누르면 1.5초 후 재장전, (중간에 총 쏘는 행위를 하면 재장전 취소)
        yield return new WaitForSeconds(CurrentGun.RELOAD_TIME);
        CurrentGun.BulletRemainCount = CurrentGun.MaxBulletCount;
        RefreshUI();

        _isReloading = false;
    }

    private void RefreshGun()
    {
        foreach(Gun gun in GunInventory)
        {
            
            gun.gameObject.SetActive(gun == CurrentGun);
        }
    }
}
