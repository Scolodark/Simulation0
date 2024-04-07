using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("이동")]
    [SerializeField] float speed;
    Rigidbody2D rigid;

    Animator anim;

    [Header("플레이어 추적")]
    [SerializeField] GameObject player;
    bool rayCheck;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        playerCheck();
    }

    void Update()
    {
        
    }

    private void playerCheck()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position);

        if (ray.collider != null)
        {
            rayCheck = ray.collider.CompareTag("Player");
            if (rayCheck)
            {
                anim.SetBool("isMove", true);
                rigid.velocity = new Vector2(-speed, rigid.velocity.y);
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            }
            else
            {
                speed *= -1f;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;

                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
            }
        }
    }
}
