using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Health,   // 체력이 꽉찬다.
    Stamina,  // 스테미나 꽉찬다.
    Bullet    // 현재 들고있는 총의 총알이 꽉찬다.
}

public class Item    // 게임 관련된 요소들이 들어가 있는 Monobehaviour
{
    public ItemType ItemType;
    public int Count;

    public Item(ItemType itemType, int count)
    {
        ItemType = itemType;
        Count = count;
    }

    public bool TryUse()
    {
        if (Count == 0)
        {
             return false;
        }
        Count -= 1;

        switch (ItemType)
        {
            case ItemType.Health:
            {
                // Todo: 플레이어 체력 꽉차기
                PlayerMoveAbility playerMoveAbility = GameObject.FindWithTag("Player").GetComponent<PlayerMoveAbility>();
                playerMoveAbility.Health = playerMoveAbility.MaxHealth;
                break;
            }
            case ItemType.Stamina:
            {
                // Todo: 플레이어 스테미너 꽉차기
                PlayerMoveAbility playerMoveAbility = GameObject.FindWithTag("Player").GetComponent<PlayerMoveAbility>();
                playerMoveAbility.Stamina = PlayerMoveAbility.MaxStamina;
                break;
            }
            case ItemType.Bullet:
            {
                // Todo: 플레이어가 현재 들고있는 총의 총알이 꽉찬다.
                PlayerGunFireAbility pgfa = GameObject.FindWithTag("Player").GetComponent<PlayerGunFireAbility>();
                pgfa.CurrentGun.BulletRemainCount = pgfa.CurrentGun.MaxBulletCount;
                break;
            }
        }

        return true;    // 아이템을 사용했다.
    }
}