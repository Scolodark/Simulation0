using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("이동과 점프")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float runSpeed;
    float run;
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
    Collider2D climbCheckColl;

    [Header("공격과방어")]
    [SerializeField] float atk;
    [SerializeField] float hp;
    float attackCoolTime;
    float attackAnimTime;

    [Header("피격 설정")]
    [SerializeField] float safeTime;
    float safeTimer;

    [Header("공격범위 설정")]
    [SerializeField] Transform pos;
    [SerializeField] Vector2 boxSize;


    /// <summary>
    /// 적의 공격을 받을때
    /// </summary>
    /// <param name="damge"></param>
    public void Damage(float damge)
    {
        if(safeTimer == 0)
        {
            hp = hp - damge;
            anim.SetTrigger("isDamage");
            safeTimer = safeTime;
        }
    }


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();
        climbCheckColl = transform.GetChild(1).GetComponent<Collider2D>();

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
        speedRanding();

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

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            moveSpeed += runSpeed; 
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            moveSpeed -= runSpeed;
        }

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

        if (climbCheckColl.IsTouchingLayers(LayerMask.GetMask("Ground")) == true ||
            climbCheckColl.IsTouchingLayers(LayerMask.GetMask("Wall")) == true)
        {
            if (moveDir.x != 0)
            {
                if (climbTimer > 0f)
                {
                    rigid.velocity = new Vector2(moveDir.x, jumpForce * 0.5f);
                }
            }
        }
    }

    /// <summary>
    /// 빠른 착지
    /// </summary>
    private void speedRanding()
    {
        if(Input.GetKey (KeyCode.S))
        {
            rigid.gravityScale = defaultGravity * 100;
        }
        else if(Input.GetKeyUp (KeyCode.S))
        {
            jumpForce = defaultGravity / 100;
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
            attackCoolTime = attackAnimTime *0.7f;

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

            foreach(Collider2D collider in collider2Ds)//잘모름
            {
                if (collider.tag == "Enemy")
                {
                    collider.GetComponent<Enemy>().Damage(atk);
                }
            }
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

        if(attackCoolTime > 0f)//공격쿨타임
        {
            attackCoolTime -= Time.deltaTime;
            if(attackCoolTime < 0f)
            {
                attackCoolTime = 0f;
            }
        }

        if (climbTimer > 0f)//벽타기제한시간
        {
            climbTimer -= Time.deltaTime;
            if (climbTimer < 0f)
            {
                climbTimer = 0f;
            }
        }

        if (safeTimer > 0f)//피격쿨타임
        {
            safeTimer -= Time.deltaTime;
            if (safeTimer < 0f)
            {
                safeTimer = 0f;
            }
        }
    }

    /// <summary>
    /// 히트박스 들어옴
    /// </summary>
    /// <param name="_hitType"></param>
    /// <param name="_coll"></param>
    //public void TriggerEnter(HitBox.enumHitType _hitType, Collider2D _coll)
    //{
    //    switch (_hitType)
    //    {
    //        case HitBox.enumHitType.WallCheck:
    //            checkWall = true;
    //            break;

    //    }
    //}

    ///// <summary>
    ///// 히트박스 나옴
    ///// </summary>
    ///// <param name="_hitType"></param>
    ///// <param name="_coll"></param>
    //public void TriggerExit(HitBox.enumHitType _hitType, Collider2D _coll)
    //{
    //        switch (_hitType)
    //    {
    //        case HitBox.enumHitType.WallCheck:
    //            checkWall = false;
    //            break;
                
    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

}
