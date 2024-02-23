using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;


public class ItemObject : MonoBehaviour
{
    public enum ItemState
    {
        Idle,    // 대기 상태  (플레이어와의 거리를 체크한다.)
        //  (충분히 가까워지면..)
        Trace,   // 날라오는 상태 (N초에 걸쳐서 Slerp로 플레이어에게 날라온다.)
    }

    public ItemType ItemType;
    private ItemState _itemState = ItemState.Idle;

    private Transform _player;
    public float EatDistance = 5f;  // 플레이어가 아이템에 가까워졌을때 인식하는 거리

    private Vector3 _startPosition;
    private const float TRACE_DURATION = 0.3f;
    private float _prograss = 0;


    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _startPosition = transform.position;
    }



    //실습 과제 36. 상태패턴 방식으로 일정 거리가 되면 아이템이 Slerp로 날라오게 하기(대기 상태, 날라오는 상태)

    // Todo 1. 아이템 프리팹을 3개(Health, Stamina, Bullet) 만든다. (도형이나 색깔 다르게 해서 구별되게)
    // Todo 2. 플레이어와 일정 거리가 되면 아이템이 먹어지고 사라진다. 

    private void Update()
    {
        switch (_itemState)
        {
            case ItemState.Idle:
                Idle();
                break;

            case ItemState.Trace:
                Trace();
                break;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            // 1. 아이템 매니저(인벤토리)에 추가하고
            ItemManager.Instance.AddItem(ItemType);



            //* 플레이어와 나의 거리를 알고 싶다. 
            //float distance = Vector3.Distance(collider.transform.position, transform.position);
            //Debug.Log(distance); 
            

            // 1. 아이템 매니저(인벤토리)에 추가하고,

            // 2. 사라진다.
            Destroy(gameObject);
        }
    }
    // 실습과제 31. 몬스터가 죽으면 아이템이 드랍된다. (Health: 20%, Stamina: 20% Bullet: 10%)

    // 실습과제 32. 일정 거리가 되면 아이템이 Slerp 이용해서 날라오게 하기 (심심하면 베지어곡선 사용)

    private void Idle()
    {
        // 플레이어와 나와의 거리 구하기
        float distance = Vector3.Distance(_player.position, transform.position);
        // 플레이어와 아이템 사이의 거리가 충분히 가까워지면 
        if (distance <= EatDistance)
        {
            // 아이템의 상태를 Trace로 변환 (Transition)
            _itemState = ItemState.Trace;
        }
    }

    private Coroutine _traceCoroutine;

    private void Trace()
    {
        _prograss += Time.deltaTime / TRACE_DURATION;
        transform.position = Vector3.Slerp(_startPosition, _player.position, _prograss);
        // Slerp 사용.(시작점, 종료점, 진행도  -> 변수 필요)
        // 진행도를 누적할 시간

        if (_prograss >= 1)
        {
            // 1. 아이템 매니저
            ItemManager.Instance.AddItem(ItemType);

            // 2. 사라진다.
            gameObject.SetActive(false);
        }

        if (_traceCoroutine != null)
        {
            _traceCoroutine = StartCoroutine(Trace_Coroutine());
        }
    }

    private IEnumerator Trace_Coroutine()
    {
        while (_prograss < 1)
        {
            _prograss += Time.deltaTime / TRACE_DURATION;
            transform.position = Vector3.Slerp(_startPosition, _player.position, _prograss);

            yield return null;   // 다음 프레임까지 대기
        }


    }


}
