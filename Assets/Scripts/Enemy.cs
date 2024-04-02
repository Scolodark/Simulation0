using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed;
    float jumpTimer;
    [SerializeField] float jumpTime;
    float jump = 3.5f;

    Rigidbody2D rigid;
    Collider2D groundCheckColl;
    Collider2D jumpCheckColl;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        groundCheckColl = transform.GetChild(0).GetComponent<Collider2D>();
        jumpCheckColl = transform.GetChild(1).GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (groundCheckColl.IsTouchingLayers(LayerMask.GetMask("Ground")) == false)
        {
            flip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        slimeJump();
        jumpTimeCheck();
    }

    private void slimeJump()
    {
        if (groundCheckColl.IsTouchingLayers(LayerMask.GetMask("Ground")) == false) return;

        if (jumpTimer == 0)
        {
            anim.SetTrigger("Jumping");
            rigid.velocity = new Vector2(rigid.velocity.x, jump);
            jumpTimer = jumpTime;
        }

        if (jumpCheckColl.IsTouchingLayers(LayerMask.GetMask("Ground")) == false)
        {
            rigid.velocity = new Vector2(speed, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }


    }

    private void flip()
    {
        speed *= -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

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
}
