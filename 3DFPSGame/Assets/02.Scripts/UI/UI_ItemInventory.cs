using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemInventory : MonoBehaviour
{

    public TextMeshProUGUI HealthItemCountTextUI;
    public TextMeshProUGUI StaminaItemCountTextUI;
    public TextMeshProUGUI BulletItemCountTextUI;



    // UI를 새로고침 하는 함수
    public void Refresh()
    {
        HealthItemCountTextUI.text = $"x{ItemManager.Instance. GetItemcount(ItemType.Health)}";
        StaminaItemCountTextUI.text = $"x{ItemManager.Instance.GetItemcount(ItemType.Stamina)}";
        BulletItemCountTextUI.text = $"x{ItemManager.Instance.GetItemcount(ItemType.Bullet)}";
    }
    // 업데이트 함수 안에 Refresh함수를 넣는 것은 전혀 좋지 않다.

}
