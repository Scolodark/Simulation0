using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
    [SerializeField] float parryingDamage;
    bool parryingCheck;
    float attackCoolTime;
    float attackAnimTime;

    [Header("피격 설정")]
    [SerializeField] float safeTime;
    [SerializeField] BarController barController;
    float safeTimer;
    SpriteRenderer spr;
    bool hitDamageCheck;
    float fullHp;

    [Header("공격범위 설정")]
    [SerializeField] Transform pos;
    [SerializeField] Vector2 boxSize;
    SpriteRenderer sprAttack;
    bool attackAlphaCheck;
    float attackAlpha;

    //적 록온
    [Header("적 감지와 락온")]
    [SerializeField] Vector2 overlapBoxSize;
    [SerializeField] GameObject boss;
    [SerializeField] BoxCollider2D enemyCheckColl;
    bool enemyCheck;
    int mouseButtonCheck;
    SpriteRenderer sprLockOn;

    [Header("프리펩")]
    [SerializeField] GameObject jumpEffect;
    GameObject jumpEffectSup;

    [Header("카메라효과")]
    [SerializeField] Camera cameraInsert;
    int hitCheckCount;
    bool hitCountbool;

    [Header("사망처리")]
    [SerializeField] BoxCollider2D reSpwObj;
    [SerializeField] ReSpawn reSpawn;

    [Header("능력치변경")]
    [SerializeField] GameManager gameManager;

    /// <summary>
    /// 플레이어 능력치 조정
    /// </summary>
    public void ButtonCheck()
    {
        gameManager.PlayerStatusChange();
        hp = gameManager.PlayerHp;
        atk = gameManager.PlayerAtk;
        fullHp = hp;
    }


    /// <summary>
    /// 적의 공격을 받을때
    /// </summary>
    /// <param name="damge"></param>
    public void Damage(float damge)
    {
        if (parryingCheck == true) return;

        if(safeTimer == 0)
        {
            hp = hp - damge;
            anim.SetTrigger("isDamage");
            safeTimer = safeTime;
            barController.hpGage(fullHp, damge);
        }
    }

    /// <summary>
    /// 데미지를 받았는지 체크
    /// </summary>
    private void damageCheck()
    {
        if(safeTimer != 0)
        {
            hitDamageCheck = true;
        }
        else
        {
            hitDamageCheck = false;
        }
    }


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();
        climbCheckColl = transform.GetChild(0).GetComponent<Collider2D>();
        spr = GetComponent<SpriteRenderer>();
        sprAttack = transform.GetChild(2).GetComponent<SpriteRenderer>();
        sprLockOn = transform.GetChild(3).GetComponent<SpriteRenderer>();

        tr.enabled = false;
    }

    void Start()
    {
        fullHp = hp;
        jumpCheck = true;
        dashVertical = jumpForce;
        doDash = 0f;
        dashCheck = true;

        setAttackEffectAlpha(0f);
    }

    void Update()
    {
        playerMove();
        playerJump();
        playerDash();
        playerClimb();
        speedRanding();

        playerAttack();
        parrying();

        DieEffect();

        hitSprite();

        checkGround();
        enemyLockOn();
        bossChecking();

        coolTimeTimer();

        damageCheck();

        lockOnEffect();
        hitCameraEffect();
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
    /// 적이 있을때 체크
    /// </summary>
    private void enemyLockOn()
    {

        Collider2D detectEnemy = Physics2D.OverlapBox(pos.position,overlapBoxSize, 0f, LayerMask.GetMask("Monster"));

        if (detectEnemy)
        {
            if (Input.GetMouseButtonDown(2))
            {
                
                
                enemyCheck = true;
                mouseButtonCheck += 1;
            }

            if (mouseButtonCheck == 2)
            {
                mouseButtonCheck = 0;
                enemyCheck = false;
            }
        }
        else
        {
            enemyCheck = false;
        }
        
    }

    /// <summary>
    /// 보스 위치 확인
    /// </summary>
    private void bossChecking()
    {
        if (enemyCheck == true)
        {
            enemyCheckColl.gameObject.SetActive(true);
            if (enemyCheckColl.IsTouchingLayers(LayerMask.GetMask("Monster")) != true)
            {
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }
        }
        else
        {
            enemyCheckColl.gameObject.SetActive(false);
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

        if (moveDir.x != 0.0f && enemyCheck == false)//플레이어 좌우 변경
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
            jumpEffectPrefabs();
            
        }
        else if(Input.GetKeyUp(KeyCode.Space) && moveDir.y > 0f && jumpCheck == true && isGround == true)//숏점프
        {
            rigid.velocity = new Vector2(moveDir.x, jumpForce*0.5f);
            jumpCheck = false;
        }

        anim.SetBool("isJump", isGround != true);

        if (moveDir.y < 0)//낙하속도
        {
            rigid.gravityScale = defaultGravity * 1.5f;
        }
        else if (moveDir.y <= 1f && moveDir.y >= 0f)//최고 높이 유지 보너스
        {
            rigid.gravityScale = defaultGravity * 0.2f;
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
        if (hitDamageCheck == true) return;//공격받는 도중 공격 불가

        if (Input.GetMouseButtonDown(0) && attackCoolTime == 0)
        {
            sprAttack.color = new Color(1,0,0);
            StartCoroutine(FadeInOut());//이팩트 표시
            anim.SetTrigger("isAttack");
            attackAnimTime = anim.GetCurrentAnimatorStateInfo(0).length;//애니메이션 쿨타임
            attackCoolTime = attackAnimTime *0.7f;

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);//공격 범위

            foreach(Collider2D collider in collider2Ds)//적에게 데미지
            {
                if (collider.tag == "Enemy")
                {
                    collider.GetComponent<Enemy>().Damage(atk);
                }
                else if(collider.tag == "Boss")
                {
                    collider.GetComponent<Boss>().Damage(atk);
                }
            }
        }
    }

    /// <summary>
    /// 패링
    /// </summary>
    private void parrying()
    {
        if (hitDamageCheck == true) return;

        if (Input.GetMouseButtonDown(1) && attackCoolTime == 0)
        {
            sprAttack.color = new Color(0, 0, 1);
            StartCoroutine(FadeInOut());//이팩트 표시
            anim.SetTrigger("isAttack");
            attackAnimTime = anim.GetCurrentAnimatorStateInfo(0).length;//애니메이션 쿨타임
            attackCoolTime = attackAnimTime;

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

            foreach (Collider2D collider in collider2Ds)//적에게 데미지
            {
                if (collider.tag == "Enemy")
                {
                    collider.GetComponent<Enemy>().Damage(parryingDamage);
                }
                else if (collider.tag == "Boss")
                {
                    collider.GetComponent<Boss>().Damage(atk);
                }
            }
        }
    }

    /// <summary>
    /// 피격후 무적타임 효과
    /// </summary>
    private void hitSprite()
    {
        if(safeTimer > 0)
        {
            setPlayerColorAlpha(0.5f);
        }
        else
        {
            setPlayerColorAlpha(1f);
        }
    }

    /// <summary>
    /// 플레이어 사망시
    /// </summary>
    private void DieEffect()
    {
        if(hp <= 0)
        {
            anim.SetTrigger("Die");
            if (reSpwObj.IsTouchingLayers(LayerMask.GetMask("Player")))
            {
                hp = fullHp;
                anim.SetTrigger("isRespawn");
                reSpawn.RecoverHp();
            }
            else if(isGround == true)
            {
                SceneManager.LoadScene("GameOver");
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

    private void OnDrawGizmos()//공격범위표시
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);

        Gizmos.color = Color.yellow;//적 인식 범위
        Gizmos.DrawWireCube(pos.position, overlapBoxSize);
    }
    

    private void setPlayerColorAlpha(float _a)//플레이어 알파값
    {
        Color color = spr.color;
        color.a = _a;
        spr.color = color;
    }

    private void setAttackEffectAlpha(float _a)//공격이펙트 알파값
    {
        Color color = sprAttack.color;
        color.a = _a;
        sprAttack.color = color;
    }

    /// <summary>
    /// 공격 이펙트 설정
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeInOut()
    {
        float ratio = 0.0f;

        while (attackAlpha < 1.0f)//알파값이 1->0으로 서서히 이동
        {
            if (Input.GetMouseButtonDown(1) && attackCoolTime == 0)
            {
                parryingCheck = true;
            }
            attackAlpha = Mathf.Lerp(0f, 1.0f, ratio);
            ratio += Time.deltaTime/0.5f;
            setAttackEffectAlpha(attackAlpha);
            yield return null;
        }
        while (attackAlpha > 0.0f)//알파값이 0->1로 서서히 이동
        {
            parryingCheck = false;
            attackAlpha = Mathf.Lerp(1.0f, 0.0f, ratio);
            ratio += Time.deltaTime / 0.5f;
            setAttackEffectAlpha(attackAlpha);
            yield return null;
        }
    }

    /// <summary>
    /// 락온 이펙트 설정
    /// </summary>
    private void lockOnEffect()
    {
        if(enemyCheck == true)
        {
            anim.SetBool("Battle", true);
            sprLockOn.gameObject.SetActive(true);
        }
        else
        {
            anim.SetBool("Battle", false);
            sprLockOn.gameObject.SetActive(false);
        }
            
    }


    /// <summary>
    /// 점프 이펙트 프리펩 생성과 파괴
    /// </summary>
    private void jumpEffectPrefabs()
    {
        Destroy(Instantiate(jumpEffect,transform.position,Quaternion.identity), 0.4f);
    }

    private void hitCameraEffect()
    {
        if (hitDamageCheck == true && hitCountbool == false)
        {
            hitCheckCount += 1;

        }
        else if(hitDamageCheck == false)
        {
            hitCountbool = false;
        }

        if (hitCheckCount == 1)
        {
            hitCheckCount = 0;
            hitCountbool = true;
            cameraInsert.GetComponent<CameraSetting>().ShakeCamera(0.1f);
        }
    }
}
