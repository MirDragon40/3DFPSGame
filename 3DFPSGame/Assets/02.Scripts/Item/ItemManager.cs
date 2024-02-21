using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 역할: 아이템들을 관리해주는 관리자
// 데이터 관리 -> 데이터(여기에서는 아이템)를 생성, 수정, 삭제, 조회(검색)  // 또는 정렬

public class ItemManager : MonoBehaviour
{
    public ItemManager Instance { get; private set; }

    public Text HealthItemCountTextUI;
    public Text StaminaItemCountTextUI;
    public Text BulletItemCountTextUI;




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Item> ItemList = new List<Item>();   // 아이템 리스트


    private void Start()
    {
        ItemList.Add(new Item(ItemType.Health, 1));   // 0: Health
        ItemList.Add(new Item(ItemType.Stamina, 1));   // 1: Stamina
        ItemList.Add(new Item(ItemType.Bullet, 1));   // 2: Bullet

        RefreshUI();


    }

    // 1. 아이템 추가(생성)
    public void AddItem(ItemType itemType)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemType == itemType)
            {
                ItemList[i].Count++;
                break;
            }
        }
    }

    // 2. 아이템 조회
    public int GetItemcount(ItemType itemType)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemType == itemType)
            {
                return ItemList[i].Count;
            }
        }

        return 0;
    }

    // 3. 아이템 사용
    public bool TryUseItem(ItemType itemType)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemType == itemType)
            {
                return ItemList[i].TryUse();
            }
        }
        return false;
    }

    // UI를 새로고침 하는 함수
    public void RefreshUI()
    {
        HealthItemCountTextUI.text = $"x{GetItemcount(ItemType.Health)}";
        StaminaItemCountTextUI.text = $"x{GetItemcount(ItemType.Stamina)}";
        BulletItemCountTextUI.text = $"x{GetItemcount(ItemType.Bullet)}";
    }
}
