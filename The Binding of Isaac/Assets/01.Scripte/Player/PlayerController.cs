using System;
using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



//플레이어상태를 enum으로 관리
public enum PlayerState
{
    Idle,   
    MoveAttack,
    Hit,
    Die
}

   
public class PlayerController : MonoBehaviour
{
    

    RoomManager roomManager;
    public Room roomThis;
    public float necronomiconRadius = 1f;
    private PlayerState PlayerState;
    public GameObject Bomb;
    public GameObject Tear;
    public GameObject isaacBody = default;

    public Rigidbody2D isaacRigid;
    public BoxCollider2D isaacBoxCollider2D;
    public Animator IsaacImage = default;
    public SpriteRenderer isaacSprite;

    public bool isKeyDownOneCheck = false;
    public bool isGetKeyCheck = false;
    public bool isOneHit = false;
    public bool isPause = false;

    public float isaacHeartHp;
    public float isaacHeartMaxHp;
    public float isaacDamage;
    public float isaacTearSpeed;
    public float isaacRange;
    public float isaacTime;
    public float isaacMaxReload;
    public float isaacReload;
    public float isaacTearHigh;
    public float isaacMoveSpeed;


    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            GameManager.Instance = new GameManager();
        }
        Initailize();
    }
    void Start()
    {
       
        StartCoroutine(DelayStart());

        isaacBoxCollider2D = GetComponent<BoxCollider2D>();
        isaacSprite = GetComponent<SpriteRenderer>();
        

        GameObject roommmm = GameObject.Find("RoomManager");
        if (roommmm != null)
        {
            roomManager = roommmm.GetComponent<RoomManager>();
        }
        IsaacImage = gameObject.GetComponent<Animator>();

        isGetKeyCheck = false;
        isPause = false;
       
        
    }   //Start()

    public void Initailize() 
    {

        isaacBoxCollider2D = GetComponent<BoxCollider2D>();
        isaacSprite = GetComponent<SpriteRenderer>();
        isaacRigid = GetComponent<Rigidbody2D>();
        isaacHeartMaxHp = GameManager.Instance.IsaacHeartMaxHp;
        isaacHeartHp = GameManager.Instance.IsaacHealthHp;
        isaacDamage = GameManager.Instance.IsaacDamage;
        isaacTearSpeed = GameManager.Instance.IsaacTearSpeed;
        isaacRange = GameManager.Instance.IsaacRange + Time.deltaTime;
        isaacTime = GameManager.Instance.IsaacTime;

        isaacMaxReload = GameManager.Instance.IsaacMaxReload;
        isaacReload = GameManager.Instance.IsaacReload;

        isaacTearHigh = GameManager.Instance.IsaacTearHigh;
        isaacMoveSpeed = GameManager.Instance.IsaacMoveSpeed;
        PlayerState = PlayerState.Idle;
        StateIdle();
    }
    private IEnumerator DelayStart()
    {
        yield return null;


        isaacHeartHp = GameManager.Instance.IsaacHealthHp;
        isaacHeartMaxHp = GameManager.Instance.IsaacHeartMaxHp;
        isaacDamage = GameManager.Instance.IsaacDamage;
        isaacTearSpeed = GameManager.Instance.IsaacTearSpeed;
        isaacRange = GameManager.Instance.IsaacRange + Time.deltaTime;
        isaacTime = GameManager.Instance.IsaacTime;

        isaacMaxReload = GameManager.Instance.IsaacMaxReload;
        isaacReload = GameManager.Instance.IsaacReload;

        isaacTearHigh = GameManager.Instance.IsaacTearHigh;
        isaacMoveSpeed = GameManager.Instance.IsaacMoveSpeed;
        PlayerState = PlayerState.Idle;
        StateIdle();
    }

    void Shoot(Vector3 vec, float x, float y)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 playerMoveDirection = new Vector2(horizontal, vertical).normalized;
        Vector2 shootDirection = new Vector2(horizontal, vertical).normalized;

        Vector2 tearPos = new Vector2(isaacBody.transform.position.x, isaacBody.transform.position.y + 0.5f);

        GameObject tear = Instantiate(Tear, tearPos, transform.rotation);
         tear.transform.Rotate(vec);
        Rigidbody2D tearRb = tear.GetComponent<Rigidbody2D>();
        tearRb.velocity = shootDirection * isaacTearSpeed;

        // 플레이어의 이동 방향에 따라 눈물의 초기 속도를 조정
        tearRb.velocity += playerMoveDirection * isaacTearSpeed;
       


    }

    //!{Update()
    void Update()
    {

        if (isPause == false) 
        {
            StateParttern();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Initailize();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (isPause == false)
            {
                isPause = true;
                GameManager.Instance.Pause(isPause);                
            }
            else 
            {
                isPause = false;
                GameManager.Instance.Pause(isPause);                
            }            
        }

        //Use Bomb
        if (0 < GameManager.bomb)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isKeyDownOneCheck = true;
                GameManager.BombChange(-1);
                GameObject bomb = Instantiate(Bomb, transform.position, transform.rotation);

            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                isKeyDownOneCheck = false;
            }
        }        

        if (Input.GetKeyDown(KeyCode.Space)) 
        {

            GameManager.Instance.UseAcitve();
        }
    }       //Update()

    private void StateParttern()
    {
        if (IsaacImage.GetBool("UnderGround"))
        {
            // UnderGround 상태에서는 다른 상태 전환을 하지 않음
            return;
        }
        switch (PlayerState)
        {
            case PlayerState.Idle:
                StateIdle();
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||
                    Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                {
                    ChangeState(PlayerState.MoveAttack);
                }               
                break;

            case PlayerState.MoveAttack:
                StateMoveAttack();
                if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) &&
               !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                {
                    ChangeState(PlayerState.Idle);
                    StateIdle();
                }
                break;
            case PlayerState.Hit:
                StateHit();
                // 처리
                break;
            case PlayerState.Die:
                StateDie();
                // 처리
                break;
            default:
                break;
        }
    }

    private void ChangeState(PlayerState newState)
    {
        PlayerState = newState;
    }
   
    private void StateIdle() 
    {
        if (PlayerState == PlayerState.Idle)
        {
            isaacRigid.velocity = new Vector2(0f, 0f);
        }
    }


    private void StateMoveAttack() 
    {        
        if (PlayerState != PlayerState.Die)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            float shootHor = Input.GetAxis("ShootHorizontal");
            float shootVer = Input.GetAxis("ShootVertical");

            isaacRigid.velocity = new Vector3(horizontal * isaacMoveSpeed * 10, vertical * isaacMoveSpeed * 10, 0);

            Vector3 right = new Vector3(0, 0, -90f);
            Vector3 left = new Vector3(0, 0, 90f);
            Vector3 up = new Vector3(0, 0, 0f);
            Vector3 down = new Vector3(0, 0, -180f);

            isaacReload += Time.deltaTime;

            if (shootHor != 0 || shootVer != 0)
            {
                if (isaacReload > isaacMaxReload)
                {
                    Vector3 shootDirection = Vector3.zero;
                    if (shootHor > 0) shootDirection = right;
                    if (shootHor < 0) shootDirection = left;
                    if (shootVer > 0) shootDirection = up;
                    if (shootVer < 0) shootDirection = down;

                    Shoot(shootDirection, shootHor, shootVer);
                    isaacReload = 0;
                }
            }

            // 방향에 따른 애니메이션 설정
            if (!Input.anyKey)
            {
                IsaacImage.SetBool("Stop", true);
            }
            else
            {
                IsaacImage.SetBool("Stop", false);
            }

            // 방향키 입력에 따른 애니메이션 설정
            if (Input.GetKey(KeyCode.W))
            {
                IsaacImage.SetBool("Up", true);
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                IsaacImage.SetBool("Up", false);
            }

            if (Input.GetKey(KeyCode.S))
            {
                IsaacImage.SetBool("Down", true);
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                IsaacImage.SetBool("Down", false);
            }

            if (Input.GetKey(KeyCode.D))
            {
                IsaacImage.SetBool("Right", true);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                IsaacImage.SetBool("Right", false);
            }

            if (Input.GetKey(KeyCode.A))
            {
                IsaacImage.SetBool("Left", true);
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                IsaacImage.SetBool("Left", false);
            }
        }

    }
   private void StateHit()
    {
        IsaacImage.SetBool("Hit", true);
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        isaacRigid.velocity = new Vector3(horizontal * isaacMoveSpeed * 10, vertical * isaacMoveSpeed * 10, 0);
        
    }
    IEnumerator HitDelay()
    {


        yield return new WaitForSeconds(1f);
        IsaacImage.SetBool("Hit", false);
        isaacRigid.velocity = Vector3.zero;
        ChangeState(PlayerState.Idle);
        RestorePlayerDirection();
        isOneHit = false;
    }
    private void RestorePlayerDirection()
    {
        // 플레이어의 현재 입력 방향을 기준으로 애니메이션 상태 복구
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 이동 방향에 따른 애니메이션 설정 복구
        IsaacImage.SetBool("Up", vertical > 0);
        IsaacImage.SetBool("Down", vertical < 0);
        IsaacImage.SetBool("Right", horizontal > 0);
        IsaacImage.SetBool("Left", horizontal < 0);
    }
    private void StateDie() 
    {        
        isaacRigid.velocity = Vector3.zero;
        IsaacImage.SetBool("Die", true);
        isaacBoxCollider2D.enabled = false;        
    }
    //상하좌우 Door충돌시 방 이동 코루틴 호출
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "MobRoom")
        {
            Room room = other.gameObject.GetComponent<Room>();

            room.isbool = true;
            room.MobSpawn(room.isbool);
            roomManager.PlayerEnterRoom(room);
        }
        else if (other.gameObject.name == "BossRoom")
        {
            Room room = other.gameObject.GetComponent<Room>();

            room.isbool = true;
            room.BossSpawn(room.isbool);
            roomManager.PlayerEnterRoom(room);
        }
        else if (other.gameObject.name == "GoldRoom")
        {
            Debug.Log("여기가 왜");
        }

        if (other.gameObject.name == "MobTear(Clone)" || other.gameObject.name == "MobSlowTear(Clone)" || other.tag == "Boss" || other.gameObject.name == "BabyPlum(Clone)" || other.gameObject.name == "UseBomb(Clone)")
        {
            if (PlayerState == PlayerState.Hit)
            {
                return;
            }

            if (GameManager.Instance != null && !isOneHit && 0 < GameManager.Instance.IsaacHealthHp)
            {
                isOneHit = true;
                GameManager.Instance.IsaacBossHit();
                ChangeState(PlayerState.Hit);
                StartCoroutine(HitDelay());
            }
            if (GameManager.Instance != null && GameManager.Instance.IsaacHealthHp == 0)
            {
                ChangeState(PlayerState.Die);
                return;
            }
        }
        else if (other.gameObject.name == "AtFly(Clone)" || other.gameObject.name == "BabyPlum(Clone)")
        {
            if (PlayerState == PlayerState.Hit)
            {
                return;
            }

            if (GameManager.Instance != null && !isOneHit && 0 < GameManager.Instance.IsaacHealthHp)
            {
                isOneHit = true;
                GameManager.Instance.IsaacNormalHit();
                ChangeState(PlayerState.Hit);
                StartCoroutine(HitDelay());
            }
            if (GameManager.Instance != null && GameManager.Instance.IsaacHealthHp == 0)
            {
                ChangeState(PlayerState.Die);
                return;
            }
        }



        if (other.gameObject.name == "RightDoor" || other.gameObject.name == "BossRightDoor" || other.gameObject.name == "GoldRightDoor")
        {
            if (!roomManager.isRoomMoveCheck)
            {
                StartCoroutine(roomManager.RightDoorTouch());
            }
        }
        if (other.gameObject.name == "LeftDoor" || other.gameObject.name == "BossLeftDoor" || other.gameObject.name == "GoldLeftDoor")
        {
            if (!roomManager.isRoomMoveCheck)
            {
                StartCoroutine(roomManager.LeftDoorTouch());
            }
        }
        if (other.gameObject.name == "TopDoor" || other.gameObject.name == "BossTopDoor" || other.gameObject.name == "GoldTopDoor")
        {
            if (!roomManager.isRoomMoveCheck)
            {
                StartCoroutine(roomManager.UpDoorTouch());
            }
        }
        if (other.gameObject.name == "BottomDoor" || other.gameObject.name == "BossBottomDoor" || other.gameObject.name == "GoldBottomDoor")
        {
            if (!roomManager.isRoomMoveCheck)
            {
                StartCoroutine(roomManager.DownDoorTouch());
            }
        }

        if (other.gameObject.name == "UnderGroundDoor")
        {
            IsaacImage.SetBool("UnderGround", true);
            Vector3 doorPosition = other.gameObject.transform.position;

            StartCoroutine(UnderGround(doorPosition));

        }
    }

    IEnumerator UnderGround(Vector2 doorPosition) 
    {
        while (Vector2.Distance(transform.position, new Vector2(doorPosition.x, doorPosition.y + 1f)) > 0.1f) // 0.1f는 멈출 거리 오차
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(doorPosition.x,doorPosition.y+1f), 10f * Time.deltaTime);
            yield return null; // 다음 프레임까지 대기
        }
        transform.position = doorPosition;
        isaacRigid.velocity = Vector3.zero;

        isaacRigid.AddForce(new Vector2(0f, -2f), ForceMode2D.Impulse);
        IsaacImage.SetBool("UnderGround", false);
        GameManager.NextScene();
    }

}
