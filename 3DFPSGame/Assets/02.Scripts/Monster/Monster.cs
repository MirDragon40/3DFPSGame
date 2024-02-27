using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum MonsterState   // 몬스터의 상태
{
    Idle,         // 0: 대기
    Trace,        // 1: 추적
    Attack,       // 공격
    ComeBack,     // 복귀
    Damaged,      // 공격 당함
    Die,          // 사망
    Patrol,       // 순찰
}

public class Monster : MonoBehaviour, IHitable
{

    [Range(0, 100)]    // 유니티 에디터에서 지원
    public int Health;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;

    /***********************************************************/


    // private CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;  

    private Transform _target;  // 플레이어
    public float FindDistance = 5f;    // 감지 범위
    public float AttackDistance = 2f;  // 공격 범위
    public float MoveSpeed = 3f;   // 몬스터의 이동속도
    public Vector3 StartPosition;  // 시작 위치
    public float MoveDistance = 10f;  // 움직일 수 있는 거리
    public const float TOLERANCE = 0.1f;  // 허용 오차 (관용)
    public int Damage = 10;
    public const float AttackDelay = 1f;
    private float _attackTimer = 0f;


    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    private float KNOCKBACK_DURATION = 0.1f;
    private float _knockbackPrograss = 0f;
    public float KnockbackPower = 1.2f;

    private const float IDLE_DURATION = 3f;
    public Transform PatrolTarget;
    private float _idleTimer = 0f;


    private MonsterState _currentState = MonsterState.Idle;

    private void Start()
    {
        //_characterController = GetComponent<CharacterController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = MoveSpeed;

        _animator = GetComponentInChildren<Animator>();   // 자식오브젝트에게서 찾는다.

        _target = GameObject.FindGameObjectWithTag("Player").transform;

        StartPosition = transform.position;

        Init();


    }

    void Update()
    {
        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }

        HealthSliderUI.value = (float)Health / (float)MaxHealth;   // 0~1

        // 상태 패턴: 상태에 따라 행동을 다르게 하는 패턴
        // 1. 몬스터가 가질 수 있는 행동에 따라 상태를 나눈다.
        // 2. 상태들이 조건에 따라 자연스럽게 전환(Transtiton)되게 설계한다.

        switch (_currentState)
        {
            case MonsterState.Idle:
                Idle();
                break;

            case MonsterState.Patrol:
                Patrol();
                break;


            case MonsterState.Trace:
                Trace();
                break;

            case MonsterState.ComeBack:
                ComeBack();
                break;

            case MonsterState.Attack:
                Attack();
                break;

            case MonsterState.Damaged:
                Damaged();
                break;

            case MonsterState.Die:
                Die();
                break;

        }

    }

    private void Idle()
    {
        _idleTimer += Time.deltaTime;

        if (PatrolTarget != null && _idleTimer >= IDLE_DURATION)
        {
            _idleTimer = 0f;
            Debug.Log("상태 전환: Idle -> Patrol");
            _animator.SetTrigger("IdleToPatrol");
            _currentState = MonsterState.Patrol;
        }


            // Idle 상태일때의 행동 코드를 작성
            // 플레이어와의 거리가 일정 범위 안이면 Trace 상태로 전환

            // Todo: 몬스터의 Idle 애니메이션 재생
            if (Vector3.Distance(_target.position, transform.position) < FindDistance)
        {
            Debug.Log("상태 전환: idle -> trace");
            _animator.SetTrigger("IdleToTrace");
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
        // _characterController.Move(_dir * MoveSpeed * Time.deltaTime);



        // 3. 쳐다본다.
        // transform.forward = _dir;   // forward 말고

        if (Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("상태전환: Trace -> ComeBack ");
            Debug.Log(Vector3.Distance(transform.position, StartPosition));
            _animator.SetTrigger("TraceToComeback");
            _currentState = MonsterState.ComeBack;

        }

        // 거리가 공격 범위 안이면

        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        {
            Debug.Log("상태전환: Trace -> Attack");
            _animator.SetTrigger("TraceToAttack");
            _currentState = MonsterState.Attack;
        }

        // 네비게이션이 접근하는 최소 거리를 공격 가능 거리로 설정
        _navMeshAgent.stoppingDistance = AttackDistance;
        // 네비게이션의 목적지를 플레이어의 위치로 한다.
        _navMeshAgent.destination = _target.position;

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
        Vector3 _dir = StartPosition - this.transform.position;
        _dir.y = 0;
        _dir.Normalize();
        // 2. 이동한다.
        // _characterController.Move(_dir * MoveSpeed * Time.deltaTime);
        // 3. 쳐다본다.
        //transform.forward = _dir;

        // 네비게이션이 접근하는 최소 거리를 공격 가능 거리로 설정
        _navMeshAgent.stoppingDistance = TOLERANCE;
        // 네비게이션의 목적지를 플레이어의 위치로 한다.
        _navMeshAgent.destination = StartPosition;

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            Debug.Log("상태전환: Comeback -> Idle");
            _animator.SetTrigger("ComebackToIdle");
            _currentState = MonsterState.Idle;
        }

        // 0이나 1이 아닌 것은 
        if (Vector3.Distance(StartPosition, transform.position) <= 3)
        {
            Debug.Log("상태전환: Comeback -> Idle");
            _animator.SetTrigger("ComebackToIdle");
            _currentState = MonsterState.Idle;
        }
    }

    private void Attack()
    {
        if (Vector3.Distance(_target.position, transform.position) > AttackDistance)
        {
            _attackTimer = 0f;
            Debug.Log("상태 전환: Attack -> Trace");
            _animator.SetTrigger("AttackToTrace");
            _currentState = MonsterState.Trace;
            return;
        }

        // 실습 과제 35. Attack 상태일 때 N초에 한 번 때리게 딜레이 주기
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackDelay)
        {
            _animator.SetTrigger("Attack");
            // _animator.Play("Attack");
        }
    }

    private void Patrol()
    {
        _navMeshAgent.stoppingDistance = 0f;
        _navMeshAgent.SetDestination(PatrolTarget.position);
        

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            Debug.Log("상태 전환: Patrol -> ComeBack");
            _animator.SetTrigger("PatrolToComeback");
            _currentState = MonsterState.ComeBack;
        }

        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환: Patrol -> Trace");
            _animator.SetTrigger("PatrolToTrace");
            _currentState = MonsterState.Trace;

        }

    }

    public void Init()
    {
        Health = MaxHealth;
    }


    private void Damaged()
    {
        // 1. Damage 애니메이션 실행 (0.5초)
        // todo: 애니메이션 실행
        // 2. 넉백 구현 (lerp 사용 -> 0.5초)
        // 2-1. 넉백 시작 / 최종 위치를 구한다.
        if (_knockbackPrograss == 0)
        {
            _knockbackStartPosition = transform.position;

            Vector3 dir = transform.position - _target.position;
            dir.y = 0;
            dir.Normalize();

            _knockbackEndPosition = transform.position + dir * KnockbackPower;
        }

        _knockbackPrograss += Time.deltaTime / KNOCKBACK_DURATION;

        // 2-2. Lerp를 이용해 넉백하기
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackPrograss);

        if (_knockbackPrograss > 1)
        {
            _knockbackPrograss = 0f;

            Debug.Log("상태 전환: Damaged -> Trace");
            _animator.SetTrigger("DamagedToTrace");
            _currentState = MonsterState.Trace;
        }
    }


    public void Hit(int damage)
    {
        if (_currentState == MonsterState.Die)
        {
            return;
        }

        Health -= damage;
        if (Health <= 0)
        {
            Debug.Log("상태 전환: Any -> Die1");
            _animator.SetTrigger($"Die{Random.Range(1, 3)}");
            _currentState = MonsterState.Die;
        }
        else
        {
            Debug.Log("상태 전환: Any -> Damaged");
            _animator.SetTrigger("Damaged");
            _currentState = MonsterState.Damaged;
        }
    }

    private Coroutine _dieCoroutine;


    private void Die()
    {
        if (_dieCoroutine == null)
        {
           _dieCoroutine =  StartCoroutine(Die_Coroutine());

        }
    }

    private IEnumerator Die_Coroutine()
    {
        // 죽으면 제자리에 멈춰 플레이어를 따라오지 못하게 함
        _navMeshAgent.isStopped = true;
        // 초기화
        _navMeshAgent.ResetPath();

        HealthSliderUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.6f);
        // 죽을 때 아이템 생성

        ItemObjectFactory.Instance.MakePercent(transform.position);

        Destroy(gameObject);
    }

    public void PlayerAttack()
    {
        IHitable playerHitable = _target.GetComponent<IHitable>();
        if (playerHitable != null)
        {
            Debug.Log("때렸다");
            playerHitable.Hit(Damage);
            _attackTimer = 0f;
        }

    }
}

