using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour, IHitable
{
    // 실습과제 19. 총으로  3번 맞출 시 사라지게 구현
    private int _hitCount = 0;


    public void Hit(int damage)
    {
        _hitCount += 1;
        if (_hitCount >= 3)
        {
            Destroy(gameObject);
        }
    }
}
