using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("능력치")]
    [SerializeField] float hp;
    [SerializeField] float atk;

    [Header("이동")]
    [SerializeField] float speed;
    Vector3 trueScale;
    Vector3 falseScale;
    Rigidbody2D rigid;

    Animator anim;

    [Header("플레이어 추적")]
    [SerializeField] GameObject player;

    [Header("근접공격 기능")]
    [SerializeField] Collider2D playerCloseUpCheckColl;
    [SerializeField] Transform CloseAttackPos;
    [SerializeField] Vector2 CloseAttackCover;
    bool closeAttack;

    public void Damage(float damge)
    {
            hp = hp - damge;
            anim.SetTrigger("isDamage");
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
        if (hit)
        {
            

            if (hit.transform.tag == "Wall" || hit.transform.tag == "Ground" || closeAttack == true)
            {
                anim.SetBool("isMove", false);
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                return;
            }

            if (hit.transform.CompareTag("Player") == true)
            {
                anim.SetBool("isMove", true);
                rigid.velocity = new Vector2(-speed, rigid.velocity.y);

                //플레이어 방향을 추적하고
                //내 스케일이 플레이어를 보고있는지 확인
                //변경

                bool isRight = (player.transform.position - transform.position).x > 0;//플레이어가 보스의 뒤로 갈때 true가 됨
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

    private void attackCheck()
    {
        if(playerCloseUpCheckColl.IsTouchingLayers(LayerMask.GetMask("Player")) == true)
        {
            closeAttack = true;
            anim.SetTrigger("isAttack");

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(CloseAttackPos.position, CloseAttackCover, 0);

            int count = collider2Ds.Length;
            for(int iNum = 0; iNum <count; iNum++)
            {
                if (collider2Ds[iNum].gameObject.tag == "Player")
                {
                    collider2Ds[iNum].GetComponent<Player>().Damage(atk);
                }
            }


        }
        else
        {
            closeAttack = false;
        }
    }

    private void OnDrawGizmos()//공격범위표시
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(CloseAttackPos.position, CloseAttackCover);
    }
}
