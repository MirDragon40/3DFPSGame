using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveAbility : MonoBehaviour, IHitable
{


    // 목표: 키보드 방향키(wasd)를 누르면 캐릭터를 바라보는 방향 기준으로 이동시키고 싶다. 
    // 속성:
    // - 이동속도
    public float MoveSpeed = 5;     // 일반 속도
    public float RunSpeed = 10;    // 뛰는 속도

    public float Stamina;             // 스태미나
    public const float MaxStamina = 100;    // 스태미나 최대량
    public float StaminaConsumeSpeed = 33f; // 초당 스태미나 소모량
    public float StaminaChargeSpeed = 50;  // 초당 스태미나 충전량

    [Header("스태미나 슬라이더 UI")]
    public Slider StaminaSliderUI;

    private CharacterController _characterController;


    [Header("플레이어 점프")]
    // 목표: 스페이스 바를 누르면 캐릭터를 점프하고 싶다. 
    // 필요 속성:
    // - 점프 파워 값
    public float JumpPower = 10f;
    public int JumpMaxCount = 2;
    public int JumpRemainCount;
    private bool _isJumping = false;
    // 구현 순서:
    // 1. 만약에 [Spacebar] 버튼을 누르는 순간 && 땅이면...
    // 2. 플레이어에게 y축에 있어 점프 파워를 적용한다. 

   

    // 목표: 캐릭터에게 중력을 적용하고 싶다. 
    // 필요속성: 
    // - 중력 값
    private float _gravity = -20f;  // 중력 변수
    // - 누적할 중력 변수: y축 속도
    private float _yVelocity = 0f;
    // 구현 순서:
    // 1. 중력 가속도가 누적된다. 
    // 2. 플레이어에게 y축에 있어 중력을 적용한다.


    [Header("벽타기 구현")]
    // 목표: 벽에 닿아 있는 상태에서 스페이스바를 누르면 벽타기를 하고 싶다.
    // 필요 속성:
    // - 벽타기 파워
    public float ClimbingPower;

    // 벽타기 스태미너 소모량 팩터
    public float ClimbingStaminaConsumeFactor = 1.5f;

    // - 벽타기 상태
    private bool _isClimbing = false;

    // 구현 순서
    // 1. 만약에 벽에 닿아 있는데
    // 2. [Spacebar] 버튼을 누르고 있으면 
    // 3. 벽을 타겠다. 

    public int Health;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;



    public Image HitEffectImageUI;





    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Stamina = MaxStamina;
        Health = MaxHealth;
    }

    // 구현 순서
    // 1. 키 입력 받기
    // 2. '캐릭터가 바라보는 방향'을 기준으로 방향구하기
    // 3. 이동하기
    void Update()
    {
        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }

        HealthSliderUI.value = (float)Health / (float)MaxHealth;  // 0 ~ 1

        // 플레이어 벽타기 
        // 1. 만약에 벽에 닿아 있는데 && 스태미너가 > 0
        if (Stamina > 0 && _characterController.collisionFlags == CollisionFlags.Sides)
        {
            // 2. [Spacebar] 버튼을 누르고 있으면 
            if (Input.GetKey(KeyCode.Space))
            {
                // 3. 벽을 타겠다. 
                _isClimbing = true;
                _yVelocity = ClimbingPower;
            }
        }



        // 1. 키 입력 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. '캐릭터가 바라보는 방향'을 기준으로 방향구하기
        Vector3 dir = new Vector3(h, 0, v);             // 로컬 좌표계 (나만의 동서남북) 
        dir.Normalize();
        // Transforms direction from local space to world space.
        dir = Camera.main.transform.TransformDirection(dir); // 글로벌 좌표계 (세상의 동서남북)


        // 실습 과제 1. Shift 누르고 있으면 빨리 뛰기 or 
        float speed = MoveSpeed; // 5
        if (_isClimbing || Input.GetKey(KeyCode.LeftShift)) // 실습 과제 2. 스태미너 구현
        {
            // - Shfit 누른 동안에는 스태미나가 서서히 소모된다. (3초)
            // 삼향 연산자 이용
            // -> 조건식을 사용해서 조건식의 참, 거짓 여부에 따라 다른 결과값을 대입
            // 조건식 ? 조건식이 참일때의 값 : 조건식이 거짓일때의 값

            float factor = _isClimbing ? ClimbingStaminaConsumeFactor : 1f;
            Stamina -= StaminaConsumeSpeed * Time.deltaTime * factor;

            // 클라이밍 상태가 아닐 때만 스피드 up!
            if (!_isClimbing && Stamina > 0)
            {
                speed = RunSpeed;
            }
        }

        else
        {
            // - 아니면 스태미나가 소모 되는 속도보다 빠른 속도로 충전된다 (2초)
            Stamina += StaminaChargeSpeed * Time.deltaTime; // 초당 50씩 충전
        }

        Stamina = Mathf.Clamp(Stamina, 0, 100);
        StaminaSliderUI.value = Stamina / MaxStamina;  // 0 ~ 1;//




        // 점프 구현
        /*
        // 1. 만약에 [Spacebar] 버튼을 누르는 순간 && 땅이면...
        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)  
        {
            // 2. 플레이어에게 y축에 있어 점프 파워를 적용한다. 
            _yVelocity = JumpPower;
        }
        */
        // 점프 구현 과제: 2단 점프 구현
        // 땅이면 점프 횟수 초기화
        // 땅에 닿아있을 때
        if (_characterController.isGrounded)
        {
            _isJumping = false;
            _isClimbing = false;
            _yVelocity = 0f;
            JumpRemainCount = JumpMaxCount;

            if (_yVelocity < -3)
            {
                Hit(10 * (int)(_yVelocity / -10f));
            }
        }

        // 1. 만약에 [Spacebar] 버튼을 누르는 순간 && (땅이거나 or 점프 횟수가 남아있다면)
        if (Input.GetKeyDown(KeyCode.Space) && (_characterController.isGrounded || (_isJumping && JumpRemainCount > 0)))
        {
            _isJumping = true;
            JumpRemainCount--;

            // 2. 플레이어에게 y축에 있어 점프 파워를 적용한다.
            _yVelocity = JumpPower;
        }




        // 3-1. 중력 적용
        // 1. 중력 가속도가 누적된다.
        _yVelocity += _gravity * Time.deltaTime; 
       
        
        // 2. 플레이어에게 y축에 있어 중력을 적용한다.
        dir.y = _yVelocity; 

        // 3-2. 이동하기
        //transform.position += speed * dir * Time.deltaTime;
        _characterController.Move( dir * speed * Time.deltaTime);

        // 9번 키를 누르면 FPS 시점으로 전환
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // FPS 카메라 모드로 전환
            //CameraManager.instance.fpsCamera.enabled = true;
            //CameraManager.instance.tpsCamera.enabled = false;

            //CameraManager.instance.SetFPSCameraMode();

            CameraManager.Instance.SetCameraMode(CameraMode.FPS);
        }
        // 0번 키를 누르면 TPS 시점으로 전환
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // TPS 카메라 모드로 전환 ver1
            //CameraManager.instance.fpsCamera.enabled = false;
            //CameraManager.instance.tpsCamera.enabled = true;

            // TPS 카메라 모드로 전환 ver2
            //CameraManager.instance.SetTPSCameramode();

            // TPS 카메라 모드로 전환 ver3
            CameraManager.Instance.SetCameraMode(CameraMode.TPS);
        }

    }

    public void Hit(int damage)
    {
        StartCoroutine(HitEffect_Coroutine(0.3f));
        CameraManager.Instance.CameraShake.Shake();

        Health -= damage;
        if (Health <= 0)
        {
            HealthSliderUI.value = 0f;
            Destroy(gameObject);
        }
    }

    private IEnumerator HitEffect_Coroutine(float delay)
    {
        // 과제 40. 히트이펙트 이미지 0.3초동안 보이게 구현
        HitEffectImageUI.gameObject.SetActive(true); // 이미지 보여주기
        yield return new WaitForSeconds(delay); // 0.3초 동안 대기
        HitEffectImageUI.gameObject.SetActive(false); // 이미지 숨기기
    }


}