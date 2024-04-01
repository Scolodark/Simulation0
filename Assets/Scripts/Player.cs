using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("이동과 점프")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    bool jumpCheck = false;

    Rigidbody2D rigid;
    Vector3 moveDir;
    Animator anim;

    //바닥체크
    bool isGround;
    BoxCollider2D boxColl;

    float defaultGravity = 2f;

    [Header("회피기능")]
    [SerializeField] float dashTime;
    [SerializeField] float dashPower;
    float doDash;
    float dashTimer;
    TrailRenderer tr;
    float dashVertical;
    bool dashCheck;

    [Header("벽타기")]
    [SerializeField] float climbTime;
    float climbTimer;
    bool checkWall;

    [Header("공격과방어")]
    float attackCoolTime;
    float attackAnimTime;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();

        tr.enabled = false;
    }

    void Start()
    {
        jumpCheck = true;
        dashVertical = jumpForce;
        doDash = 0f;
        dashCheck = true;
    }

    void Update()
    {
        playerMove();
        playerJump();
        playerDash();
        playerClimb();

        playerAttack();

        checkGround();
        coolTimeTimer();
    }

    /// <summary>
    /// 지면체크
    /// </summary>
    private void checkGround()
    {
        isGround = false;
        float raySizeY = boxColl.size.y;

        RaycastHit2D ray = Physics2D.BoxCast(boxColl.bounds.center, boxColl.bounds.size, 0f, Vector2.down, raySizeY/2.5f, LayerMask.GetMask("Ground"));

        if (ray)
        {
            isGround = true;
        }
    }

    /// <summary>
    /// 플레이어 이동
    /// </summary>
    private void playerMove()
    {
        
        moveDir.x = Input.GetAxisRaw("Horizontal") * (moveSpeed + doDash);//플레이어 이동
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;

        anim.SetBool("isRun", moveDir.x != 0.0f);

        if (moveDir.x != 0.0f)//플레이어 좌우 변경
        {
            Vector3 localScale = transform.localScale;
            localScale.x = Input.GetAxisRaw("Horizontal") * -1;
            transform.localScale = localScale;
        }
    }

    /// <summary>
    /// 플레이어 점프
    /// </summary>
    private void playerJump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && isGround == true && jumpCheck == true)//최대점프
        {
            rigid.velocity = new Vector2(moveDir.x, jumpForce);
            jumpCheck = false;
           
        }
        else if(Input.GetKeyUp(KeyCode.Space) && moveDir.y > 0f && jumpCheck == true && isGround == true)//숏점프
        {
            rigid.velocity = new Vector2(moveDir.x, jumpForce*0.5f);
        }

        anim.SetBool("isJump", isGround != true);

        if (moveDir.y < 0)//낙하속도
        {
            rigid.gravityScale = defaultGravity * 1.5f;
        }
        else if (moveDir.y <= 1f && moveDir.y >= 0f)//최고 높이 유지 보너스
        {
            rigid.gravityScale = defaultGravity * 0.19f;
        }
        else//상승할때 기본 중력으로 복귀
        {
            rigid.gravityScale = defaultGravity;
        }

        if (Input.GetKeyUp(KeyCode.Space))//연속 숏점프 방지
        {
            jumpCheck = true;
        }

        //점프중 수평 이동속도 저하
    }

    /// <summary>
    /// 회피(대쉬)기능
    /// </summary>
    private void playerDash()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer == 0 && dashCheck == true)//대쉬작동
        {
            jumpForce = 0f;
            doDash = dashPower;

            dashTimer = dashTime;
            tr.enabled = true;

            dashCheck = false;
        }

        if(isGround == true)//공중에서 연속사용 방지
        {
            dashCheck = true;
        }

        jumpForce = dashVertical;
    }

    /// <summary>
    /// 벽타기
    /// </summary>
    private void playerClimb()
    {
        if (climbTimer == 0f && isGround == true)
        {
            climbTimer = climbTime;
        }

        if (moveDir.x != 0)
        {
            if (checkWall == true && climbTimer > 0f)
            {
                rigid.velocity = new Vector2(moveDir.x, jumpForce * 0.5f);
            }
        }
    }

    /// <summary>
    /// 공격하기
    /// </summary>
    private void playerAttack()
    {
        if (Input.GetMouseButtonDown(0) && attackCoolTime == 0)
        {
            anim.SetTrigger("isAttack");
            attackAnimTime = anim.GetCurrentAnimatorStateInfo(0).length;//애니메이션 쿨타임
            attackCoolTime = attackAnimTime;
        }
    }

    /// <summary>
    /// 타이머 기능
    /// </summary>
    private void coolTimeTimer()
    {
        if (dashTimer > 0f)///대쉬 사용시간
        {
            dashTimer -= Time.deltaTime;
            if(dashTimer < 0f)//대쉬 시간이 끝났을때
            {
                dashTimer = 0f;
                doDash = 0f;
                tr.enabled = false;
                tr.Clear();
            }
        }

        if(attackAnimTime > 0f)
        {
            attackCoolTime -= Time.deltaTime;
            if(attackCoolTime < 0f)
            {
                attackCoolTime = 0f;
            }
        }

        if (climbTimer > 0f)
        {
            climbTimer -= Time.deltaTime;
            if (climbTimer < 0f)
            {
                climbTimer = 0f;
            }
        }
    }

    /// <summary>
    /// 히트박스 들어옴
    /// </summary>
    /// <param name="_hitType"></param>
    /// <param name="_coll"></param>
    public void TriggerEnter(HitBox.enumHitType _hitType, Collider2D _coll)
    {
        switch (_hitType)
        {
            case HitBox.enumHitType.WallCheck:
                checkWall = true;
                break;
        }
    }

    /// <summary>
    /// 히트박스 나옴
    /// </summary>
    /// <param name="_hitType"></param>
    /// <param name="_coll"></param>
    public void TriggerExit(HitBox.enumHitType _hitType, Collider2D _coll)
    {
        switch (_hitType)
        {
            case HitBox.enumHitType.WallCheck:
                checkWall = false;
                break;
        }
    }

}
