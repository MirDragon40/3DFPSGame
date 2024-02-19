using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour
{
    [Header("체력")]
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



}
