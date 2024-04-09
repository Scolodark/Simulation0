using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("이동")]
    [SerializeField] float speed;
    float jumpTimer;
    [SerializeField] float jumpTime;
    float jump = 3.5f;

    Rigidbody2D rigid;
    Collider2D groundCheckColl;
    Collider2D jumpCheckColl;
    Animator anim;

    [Header("능력치")]
    [SerializeField] float hp;
    [SerializeField] float atk;

    [Header("프리펩")]
    [SerializeField] GameObject deathEffect;
    GameObject deathEffectSub;//참조

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
        //그라운드가 없을때 플립
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
    /// 슬라임의 튀는 움직임 구현
    /// </summary>
    private void slimeJump()
    {
        if (groundCheckColl.IsTouchingLayers(LayerMask.GetMask("Ground")) == false) return;

        if (jumpTimer == 0) //점프하는 타이밍
        {
            anim.SetTrigger("Jumping");
            rigid.velocity = new Vector2(rigid.velocity.x, jump);
            jumpTimer = jumpTime;
        }

        if (jumpCheckColl.IsTouchingLayers(LayerMask.GetMask("Ground")) == false) //점프를 할때 이동을함
        {
            rigid.velocity = new Vector2(speed, rigid.velocity.y);
        }
        else //점프를 하지 않을때 이동하지 않음
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }

    /// <summary>
    /// 방향전환
    /// </summary>
    private void flip()
    {
        speed *= -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }


    /// <summary>
    /// 플레이어 공격
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
    /// 타이머
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
    /// 체력이 모두 떨어질때
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
    /// 점프 이펙트 프리펩 생성
    /// </summary>
    private void deathEffectPrefabs()
    {
        Destroy(Instantiate(deathEffect,transform.position,Quaternion.identity), 0.4f);
    }
}
