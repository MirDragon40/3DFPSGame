using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    Rifle, // 따발총
    Sniper, // 저격총
    Pistol  // 권총
}


public class Gun : MonoBehaviour
{
    public GunType GType;

    [Header("총")]
    public GameObject Rifle1;
    public GameObject Sniper2;
    public GameObject Pistol3;

    // 공격력
    public int Damage;

    // - 발사 쿨타임
    [Header("Gun 타이머")]
    public float FireCoolTime;
    

    // - 총알 개수
    [Header("총알 개수 제한")]
    public int BulletRemainCount;
    public int MaxBulletCount;

    // - 재장전 시간
    public float RELOAD_TIME; // 재장전 시간

    private void Start()
    {
        // 총알 개수 초기화
        BulletRemainCount = MaxBulletCount;

    }



    // Update is called once per frame
    void Update()
    {

    }
}
