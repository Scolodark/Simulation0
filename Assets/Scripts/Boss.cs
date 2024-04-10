using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("�ɷ�ġ")]
    [SerializeField] float hp;
    [SerializeField] float atk;

    [Header("�̵�")]
    [SerializeField] float speed;
    Vector3 trueScale;
    Vector3 falseScale;
    Rigidbody2D rigid;

    Animator anim;

    [Header("�÷��̾� ����")]
    [SerializeField] GameObject player;

    [Header("�������� ���")]
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
    /// �����κ��� ���� ���̴�´ٸ� ������ ����
    /// �÷��̾ ��´ٸ� ������ �÷��̾ ����
    /// </summary>
    private void playerCheck()
    {
        //���̴� �÷��̾ ����

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

                //�÷��̾� ������ �����ϰ�
                //�� �������� �÷��̾ �����ִ��� Ȯ��
                //����

                bool isRight = (player.transform.position - transform.position).x > 0;//�÷��̾ ������ �ڷ� ���� true�� ��
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

    private void OnDrawGizmos()//���ݹ���ǥ��
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(CloseAttackPos.position, CloseAttackCover);
    }
}
