using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 역할: 아이템들을 관리해주는 관리자
// 데이터 관리 -> 데이터(여기에서는 아이템)를 생성, 수정, 삭제, 조회(검색)  // 또는 정렬

public class ItemManager : MonoBehaviour
{
    public ItemManager Instance { get; private set; }


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
        ItemList.Add(new Item());
        ItemList.Add(new Item());
        ItemList.Add(new Item());

        ItemList[0].ItemType = ItemType.Health;
        ItemList[0].Count = 1;

        ItemList[1].ItemType = ItemType.Stamina;
        ItemList[1].Count = 1;

        ItemList[2].ItemType = ItemType.Bullet;
        ItemList[2].Count = 1;
    }

}
