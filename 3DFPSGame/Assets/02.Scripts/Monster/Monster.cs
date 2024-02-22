using UnityEngine;
using UnityEngine.UI;

public enum MonsterState   // 몬스터의 상태
{
    Idle,         // 0: 대기
    Trace,        // 1: 추적
    Attack,       // 공격
    ComeBack,       // 복귀
    Damaged,      // 공격 당함
    Die           // 사망
}

public class Monster : MonoBehaviour, IHitable
{

    [Range(0, 100)]    // 유니티 에디터에서 지원
    public int Health;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;

    private CharacterController _characterController;

    private Transform _target;  // 플레이어
    public float FindDistace = 5f;    // 감지 범위
    public float AttackDistance = 2f;  // 공격 범위
    public float MoveSpeed = 3f;   // 몬스터의 이동속도
    public Vector3 StartPosition;  // 시작 위치
    public float MoveDistance = 40f;  // 움직일 수 있는 거리
    public const float TOLERANCE = 0.1f;  // 허용 오차 (관용)
    public int Damage = 10;

    private MonsterState _currentState = MonsterState.Idle;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        StartPosition = transform.position;

        Init();
    }

    void Update()
    {
        HealthSliderUI.value = (float)Health / (float)MaxHealth;   // 0~1

        // 상태 패턴: 상태에 따라 행동을 다르게 하는 패턴
        // 1. 몬스터가 가질 수 있는 행동에 따라 상태를 나눈다.
        // 2. 상태들이 조건에 따라 자연스럽게 전환(Transtiton)되게 설계한다.

        switch (_currentState)
        {
            case MonsterState.Idle:
                Idle();
                break;

            case MonsterState.Trace:
                Trace();
                break;

            case MonsterState.Attack:
                Trace();
                break;
   
        }

    }

    private void Idle()
    {

        // Idle 상태일때의 행동 코드를 작성
        // 플레이어와의 거리가 일정 범위 안이면 Trace 상태로 전환

        // Todo: 몬스터의 Idle 애니메이션 재생
        if (Vector3.Distance(_target.position, transform.position) <= FindDistace)
        {
            Debug.Log("상태 전환: idle -> trace");
            _currentState = MonsterState.Trace;
        }
    }
    private void Trace()
    {
        // Trace 상태일때의 행동 코드를 작성
        // 플레이어에게 다가간다.
        // 1. 방향을 구한다. (target - me)
        Vector3 _dir = _target.transform.position - this.transform.position;
        _dir.y = 0;
        _dir.Normalize();
        // 2. 이동한다.
        _characterController.Move( _dir * MoveSpeed * Time.deltaTime);
        // 3. 쳐다본다.
        transform.forward = _dir;

        if (Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("상태전환: Trace -> ComeBack ");
            _currentState = MonsterState.ComeBack;
            
        }

        // 거리가 공격 범위 안이면
        
        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        {
            Debug.Log("상태전환: Trace -> Attack");
           _currentState = MonsterState.Attack;
        }

        // 원점 과의 거리가 너무 멀어지면
        /*
        if ()
        {
            _currentState= MonsterState.Return;
        }
        */
    }

    public void ComeBack()
    {
        // Trace 상태일때의 행동 코드를 작성
        // 플레이어에게 다가간다.
        // 1. 방향을 구한다. (target - me)
        Vector3 _dir =StartPosition - this.transform.position;
        _dir.y = 0;
        _dir.Normalize();
        // 2. 이동한다.
        _characterController.Move(_dir * MoveSpeed * Time.deltaTime);
        // 3. 쳐다본다.
        transform.forward = _dir;

        // 0이나 1이 아닌 것은 
        if (Vector3.Distance( StartPosition, transform.position) < TOLERANCE)
        {
            Debug.Log("상태전환: Comeback -> Idle");
            _currentState = MonsterState.Idle;
        }

    }

    private void Attack()
    {
        if (Vector3.Distance(_target.position, transform.position) < AttackDistance)
        {
            Debug.Log("상태전환: Attack -> Trace");
            _currentState = MonsterState.Trace;
            return;
        }
        IHitable playerHitable =  _target.GetComponent<IHitable>();
        if (playerHitable != null)
        {
            playerHitable.Hit(Damage);
        }
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

