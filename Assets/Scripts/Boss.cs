using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("능력치")]
    [SerializeField] float hp;
    [SerializeField] float atk;

    [Header("이동")]
    [SerializeField] float speed;
    [SerializeField] Collider2D wallCheckColl;
    Vector3 trueScale;
    Vector3 falseScale;
    Rigidbody2D rigid;
    bool isRight;

    Animator anim;

    [Header("플레이어 추적")]
    [SerializeField] GameObject player;

    [Header("근접공격 기능")]
    [SerializeField] Collider2D playerCloseUpCheckColl;
    [SerializeField] Transform CloseAttackPos;
    [SerializeField] Vector2 CloseAttackCover;
    bool closeAttack;
    bool closeAttacking;
    bool backStepCheck;
    bool invincible;

    [Header("원거리공격 기능")]
    [SerializeField] Collider2D playerLongDistanceCheckColl;
    [SerializeField] Transform longDistanceAttacPos;
    [SerializeField] Vector2 longDistanceAttackCover;
    bool longDistanceCheck;
    bool longDistanceAttackEnd;

    [Header("탄막 생성")]
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletCount;
    [SerializeField] GameObject spawnPointObj;

    SpriteRenderer spr;

    [SerializeField] GameManager gameManager;

    [Header("플레이어를 향해 순간이동")]
    [SerializeField] Collider2D teleportCheckColl;

    [Header("벽과 충돌시 반응")]
    [SerializeField] BoxCollider2D backCheck;
    [SerializeField] GameObject teleportTpEscape;
    bool isWallCheck;

    [Header("피격효과")]
    [SerializeField] BarController barController;

    [Header("2페이즈")]
    bool phase;
    float fullHp;

    [Header("사망")]
    bool deathCheck;

    [Header("능력치 변경")]
    [SerializeField] Button checkButton;
    [SerializeField] GameObject reSpawnObj;
    [SerializeField] ReSpawn reSpawn;
    

    /// <summary>
    /// 보스 능력치 변경
    /// </summary>
    public void BossStatousSetting()
    {
        gameManager.BossStatusChange();
        hp = gameManager.BossHp;
        atk = gameManager.BossAtk;
        fullHp = hp;
    }

    /// <summary>
    /// 공격받을때
    /// </summary>
    /// <param name="damge"></param>
    public void Damage(float damge)
    {
        if(invincible == false)
        {
            hp = hp - damge;
            anim.SetTrigger("isDamage");
            barController.hpGage(fullHp, damge);
            //공격받을때 색이 바뀜
        }
    }

    void Start()
    {
        fullHp = hp;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();

        trueScale = transform.localScale;
        falseScale = transform.localScale;
        trueScale.x *= -1f;
    }

    private void FixedUpdate()
    {
        playerCheck();
    }

    void Update()
    {
        attackCheck();
        longDistanceAttack();
        teleportToPlayer();
        phaseChack();
        death();
        wallCheck();
        hpReset();
    }

    /// <summary>
    /// 플레이어가 리스폰 될시 보스 체력 초기화
    /// </summary>
    private void hpReset()
    {
        if (player.transform.position.x == reSpawnObj.transform.position.x)
        {
            hp = fullHp;
            reSpawn.BossRecoverHp();
        }
    }

    /// <summary>
    /// 페이즈 전환
    /// </summary>
    private void phaseChack()
    {
        if (fullHp/2 >= hp)
        {
            phase = true;
        }

        if(phase == true)
        {
            //보스색 변경
            spr.color = new Color(1, 0, 0);
            //애니메이션 속도 2배
            anim.speed = 2f;
            speed =10;
            gameManager.checkSpawn();
        }
    }

    private void death()//죽음 확인
    {
        if (hp <= 0 && deathCheck == false)
        {
            anim.SetTrigger("isDeath");
            deathCheck = true;
        }
    }
    private void deathEnd()//보스 사망시 소멸
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 보스로부터 레이 벽이닿는다면 동작을 멈춤
    /// 플레이어가 닿는다면 보스가 플레이어를 추적
    /// </summary>
    private void playerCheck()
    {
        //레이는 플레이어나 벽만

        //RaycastHit2D[] ray = Physics2D.RaycastAll (transform.position, player.transform.position - transform.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 100.0f, LayerMask.GetMask("Player", "Ground", "Wall"));

        if (backStepCheck == true || longDistanceCheck == true) return;

        if (hit)
        {
            if (hit.transform.tag == "Wall" || hit.transform.tag == "Ground" || closeAttack == true)
            {
                anim.SetBool("isMove", false);
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                return;
            }

            if (hit.transform.CompareTag("Player") == true && backStepCheck != true)
            {
                anim.SetBool("isMove", true);
                rigid.velocity = new Vector2(-speed, rigid.velocity.y);

                //플레이어 방향을 추적하고
                //내 스케일이 플레이어를 보고있는지 확인
                //변경

                isRight = (player.transform.position - transform.position).x > 0;//플레이어가 보스의 뒤로 갈때 true가 됨
                if (isRight != true)
                {
                    rigid.velocity = new Vector2(-speed, rigid.velocity.y);
                    transform.localScale = falseScale;
                }
                else if (isRight)
                {
                    rigid.velocity = new Vector2(speed, rigid.velocity.y);
                    transform.localScale = trueScale;
                }
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            }
        }
        else
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
            
    }

    /// <summary>
    /// 보스의 근접 공격
    /// </summary>
    private void attackCheck()
    {
        if(playerCloseUpCheckColl.IsTouchingLayers(LayerMask.GetMask("Player")) == true)
        {
            closeAttack = true;
        }
        else
        {
            closeAttack = false;
            closeAttacking = true;
        }

        if(closeAttacking == true && closeAttack == true)
        {
            anim.SetTrigger("isAttack");
            closeAttacking = false;
        }

        if (closeAttack == true && closeAttacking == false && backStepCheck == false)//공격 이후 행동
        {
            anim.SetTrigger("useSpell");
        }
    }

    /// <summary>
    /// 공격 딜레이
    /// </summary>
    /// <returns></returns>
    IEnumerator attackDelay()
    {
        if(phase == false)
        {
            yield return new WaitForSeconds(0.7f);//만약 2페이즈가 시작될때 시간 감소
        }
        else if( phase == true)
        {
            yield return new WaitForSeconds(0.35f);
        }
       

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(CloseAttackPos.position, CloseAttackCover, 0);

        int count = collider2Ds.Length;
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (collider2Ds[iNum].gameObject.tag == "Player")
            {
                if (closeAttack == true)
                {
                    collider2Ds[iNum].GetComponent<Player>().Damage(atk);
                }
            }
        }
    }

    /// <summary>
    /// 공격 이후 이동
    /// </summary>
    private void backStep()
    {
        invincible = true;
        Color color = spr.color;
        color.a = 0f;
        spr.color = color;

        backStepCheck = true;

        if (backStepCheck == true)
        {
            rigid.velocity = new Vector2(-rigid.transform.localScale.x * -10f, rigid.velocity.y);
            if (wallCheckColl.IsTouchingLayers(LayerMask.GetMask("TransparentWall")))
            {
                rigid.velocity = new Vector2(0,rigid.velocity.y);
            }
        }
    }

    /// <summary>
    /// 원거리 공격
    /// </summary>
    private void longDistanceAttack()
    {

        if (playerLongDistanceCheckColl.IsTouchingLayers(LayerMask.GetMask("Player")) == true && backStepCheck == true)
        {

            invincible = false;
            Color color = spr.color;
            color.a = 1f;
            spr.color = color;

            longDistanceCheck = true;

            
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
            longDistanceAttackEnd = true;

            spawnBullet();
        }
        else
        {
            spawnPointObj.SetActive(false);
        }
    }

    /// <summary>
    /// 탄막 소환
    /// </summary>
    private void spawnBullet()
    {

        if (longDistanceAttackEnd == true)
        {
            gameManager.bulletSpawn();

            longDistanceCheck = false;
            backStepCheck = false;
            longDistanceAttackEnd = false;
        }
    }

    /// <summary>
    /// 체크시 플레이어를 향해 텔레포트
    /// </summary>
    private void teleportToPlayer()
    {
        if (teleportCheckColl.IsTouchingLayers(LayerMask.GetMask("Player"))==true)
        {
            anim.SetTrigger("isCast");
            transform.position = new Vector2(player.transform.position.x, transform.position.y);
        }
    }

    /// <summary>
    /// 보스가 벽에 닿았을 때의 행동
    /// </summary>
    private void wallCheck()
    {
        if (wallCheckColl.IsTouchingLayers(LayerMask.GetMask("TransparentWall")) == true)
        {
            invincible = false;
            Color color = spr.color;
            color.a = 1f;
            spr.color = color;

            isWallCheck = true;
            backStepCheck = false;

            transform.position = new Vector2(teleportTpEscape.transform.position.x, transform.position.y);
        }
    }

    private void OnDrawGizmos()//근접공격범위표시
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(CloseAttackPos.position, CloseAttackCover);

        longDistanceGizmos();
    }

    /// <summary>
    /// 원거리 공격 범위 표시
    /// </summary>
    private void longDistanceGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(longDistanceAttacPos.position, longDistanceAttackCover);
    }
}
