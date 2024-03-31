using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("�̵��� ����")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    bool jumpCheck = false;

    Rigidbody2D rigid;
    Vector3 moveDir;
    Animator anim;

    //�ٴ�üũ
    bool isGround;
    BoxCollider2D boxColl;

    float defaultGravity = 2f;

    [Header("ȸ�Ǳ��")]
    [SerializeField] float dashTime;
    [SerializeField] float dashPower;
    float doDash;
    float dashTimer;
    TrailRenderer tr;
    float dashVertical;
    bool dashCheck;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();

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

        checkGround();
        coolTimeTimer();
    }

    /// <summary>
    /// ����üũ
    /// </summary>
    private void checkGround()
    {
        isGround = false;
        float raySize = boxColl.size.y;

        RaycastHit2D ray = Physics2D.BoxCast(boxColl.bounds.center, boxColl.bounds.size, 0f, Vector2.down, raySize/2.5f, LayerMask.GetMask("Ground"));

        if (ray)
        {
            isGround = true;
        }

    }

    /// <summary>
    /// �÷��̾� �̵�
    /// </summary>
    private void playerMove()
    {
        
        moveDir.x = Input.GetAxisRaw("Horizontal") * (moveSpeed + doDash);//�÷��̾� �̵�
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;

        anim.SetBool("isRun", moveDir.x != 0.0f);

        if (moveDir.x != 0.0f)//�÷��̾� �¿� ����
        {
            Vector3 localScale = transform.localScale;
            localScale.x = Input.GetAxisRaw("Horizontal") * -1;
            transform.localScale = localScale;
        }
    }

    private void playerJump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && isGround == true && jumpCheck == true)//�ִ�����
        {
            rigid.velocity = new Vector2(moveDir.x, jumpForce);
            jumpCheck = false;
           
        }
        else if(Input.GetKeyUp(KeyCode.Space) && moveDir.y > 0f && jumpCheck == true && isGround == true)//������
        {
            rigid.velocity = new Vector2(moveDir.x, jumpForce*0.5f);
        }

        anim.SetBool("isJump", isGround != true);

        if (moveDir.y < 0)//���ϼӵ�
        {
            rigid.gravityScale = defaultGravity * 1.5f;
        }
        else if (moveDir.y <= 1f && moveDir.y >= 0f)//�ְ� ���� ���� ���ʽ�
        {
            rigid.gravityScale = defaultGravity * 0.19f;
        }
        else
        {
            rigid.gravityScale = defaultGravity;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpCheck = true;
        }

        Debug.Log(jumpCheck);

        //������ ���� �̵��ӵ� ����
    }

    private void playerDash()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer == 0 && dashCheck == true)//�뽬�۵�
        {
            jumpForce = 0f;
            doDash = dashPower;

            dashTimer = dashTime;
            tr.enabled = true;

            dashCheck = false;
        }

        if(isGround == true)
        {
            dashCheck = true;
        }

        jumpForce = dashVertical;
    }

    private void coolTimeTimer()
    {
        if (dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
            if(dashTimer < 0f)
            {
                dashTimer = 0f;
                doDash = 0f;
                tr.enabled = false;
                tr.Clear();
            }
        }
    }

}
