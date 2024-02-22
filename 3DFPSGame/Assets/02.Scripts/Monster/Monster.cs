using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour, IHitable
{
    
    [Range(0,100)]    // 유니티 에디터에서 지원
    public int Health;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;


    void Update()
    {
        HealthSliderUI.value = (float)Health / (float) MaxHealth;   // 0~1

    }

    public void Init()
    {
        Health = MaxHealth;
    }

    public void Hit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        /*
        if (Random.Range(0,2) == 1)
        {
            // 아이템 주문
            ItemObjectFactory.Instance.Make(ItemType.Health, transform.position);   
        }
        */

        // 죽을 때 아이템 생성
        ItemObjectFactory.Instance.MakePercent(transform.position);

        Destroy(gameObject);

    }

}
