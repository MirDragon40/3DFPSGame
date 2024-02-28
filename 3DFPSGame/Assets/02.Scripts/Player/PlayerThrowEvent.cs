using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowEvent : MonoBehaviour
{
    private PlayerBombFireAbility _bombFire;

    private void Start()
    {
        _bombFire = GetComponentInParent<PlayerBombFireAbility>();
    }

    public void ThrowEvent()
    {
        Debug.Log("어택 이벤트 발생!");
        _bombFire.ThrowBomb();

    }
}
