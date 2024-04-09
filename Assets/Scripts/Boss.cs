using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("�̵�")]
    [SerializeField] float speed;
    Vector3 trueScale;
    Vector3 falseScale;
    Rigidbody2D rigid;

    Animator anim;

    [Header("�÷��̾� ����")]
    [SerializeField] GameObject player;

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
            

            if (hit.transform.tag == "Wall" || hit.transform.tag == "Ground")
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
            //else if (hit.transform.CompareTag("Player") == false)
            //{
            //    speed *= -1f;
            //    Vector3 localScale = transform.localScale;
            //    localScale.x *= -1;
            //    transform.localScale = localScale;

            //    Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
            //}
        }
        else
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }


        //int count = ray.Length;
        //for (int iNum = 0; iNum < count; ++iNum)
        //{
        //    RaycastHit2D rayCheck = ray[iNum]; 
        //    if (rayCheck.collider.CompareTag("Wall") || rayCheck.collider.CompareTag("Monster"))
        //    {
        //        Debug.Log("��");
        //        return;
        //    }

        //    if (rayCheck.collider.CompareTag("Player") == true)
        //    {
        //        anim.SetBool("isMove", true);
        //        rigid.velocity = new Vector2(-speed, rigid.velocity.y);
        //        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
        //    }
        //    else if (rayCheck.collider.CompareTag("Player") == false)
        //    {
        //        speed *= -1f;
        //        Vector3 localScale = transform.localScale;
        //        localScale.x *= -1;
        //        transform.localScale = localScale;

        //        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
        //    }
        //}
            
            
    }
}
