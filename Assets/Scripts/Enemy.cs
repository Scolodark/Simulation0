using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("�̵�")]
    [SerializeField] float speed;
    float jumpTimer;
    [SerializeField] float jumpTime;
    float jump = 3.5f;

    Rigidbody2D rigid;
    Collider2D groundCheckColl;
    Collider2D jumpCheckColl;
    Animator anim;

    [Header("�ɷ�ġ")]
    [SerializeField] float hp;
    [SerializeField] float atk;

    [Header("������")]
    [SerializeField] GameObject deathEffect;
    GameObject deathEffectSub;//����

    public void Damage(float damge)
    {
        hp = hp - damge;
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        groundCheckColl = transform.GetChild(0).GetComponent<Collider2D>();
        jumpCheckColl = transform.GetChild(1).GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //�׶��尡 ������ �ø�
        if (groundCheckColl.IsTouchingLayers(LayerMask.GetMask("Ground")) == false ||
            groundCheckColl.IsTouchingLayers(LayerMask.GetMask("Wall")) == true)
        {
            flip();
        }
    }

    void Update()
    {
        slimeJump();
        jumpTimeCheck();
        death();
    }

    /// <summary>
    /// �������� Ƣ�� ������ ����
    /// </summary>
    private void slimeJump()
    {
        if (groundCheckColl.IsTouchingLayers(LayerMask.GetMask("Ground")) == false) return;

        if (jumpTimer == 0) //�����ϴ� Ÿ�̹�
        {
            anim.SetTrigger("Jumping");
            rigid.velocity = new Vector2(rigid.velocity.x, jump);
            jumpTimer = jumpTime;
        }

        if (jumpCheckColl.IsTouchingLayers(LayerMask.GetMask("Ground")) == false) //������ �Ҷ� �̵�����
        {
            rigid.velocity = new Vector2(speed, rigid.velocity.y);
        }
        else //������ ���� ������ �̵����� ����
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }

    /// <summary>
    /// ������ȯ
    /// </summary>
    private void flip()
    {
        speed *= -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }


    /// <summary>
    /// �÷��̾� ����
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().Damage(atk);
        }
    }

    /// <summary>
    /// Ÿ�̸�
    /// </summary>
    private void jumpTimeCheck()
    {
        if (jumpTimer > 0f)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer < 0f)
            {
                jumpTimer = 0f;
            }
        }
    }


    /// <summary>
    /// ü���� ��� ��������
    /// </summary>
    private void death()
    {
        if(hp <= 0)
        {
            deathEffectPrefabs();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���� ����Ʈ ������ ����
    /// </summary>
    private void deathEffectPrefabs()
    {
        Destroy(Instantiate(deathEffect,transform.position,Quaternion.identity), 0.4f);
    }
}
