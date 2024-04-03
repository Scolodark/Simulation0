using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("�̵��� ����")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float runSpeed;
    float run;
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

    [Header("��Ÿ��")]
    [SerializeField] float climbTime;
    float climbTimer;
    Collider2D climbCheckColl;

    [Header("���ݰ����")]
    [SerializeField] float atk;
    [SerializeField] float hp;
    float attackCoolTime;
    float attackAnimTime;

    [Header("�ǰ� ����")]
    [SerializeField] float safeTime;
    float safeTimer;

    [Header("���ݹ��� ����")]
    [SerializeField] Transform pos;
    [SerializeField] Vector2 boxSize;


    /// <summary>
    /// ���� ������ ������
    /// </summary>
    /// <param name="damge"></param>
    public void Damage(float damge)
    {
        if(safeTimer == 0)
        {
            hp = hp - damge;
            anim.SetTrigger("isDamage");
            safeTimer = safeTime;
        }
    }


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();
        climbCheckColl = transform.GetChild(1).GetComponent<Collider2D>();

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
        playerClimb();
        speedRanding();

        playerAttack();

        checkGround();
        coolTimeTimer();
    }

    /// <summary>
    /// ����üũ
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
    /// �÷��̾� �̵�
    /// </summary>
    private void playerMove()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal") * (moveSpeed + doDash);//�÷��̾� �̵�

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

        if (moveDir.x != 0.0f)//�÷��̾� �¿� ����
        {
            Vector3 localScale = transform.localScale;
            localScale.x = Input.GetAxisRaw("Horizontal") * -1;
            transform.localScale = localScale;
        }
    }

    /// <summary>
    /// �÷��̾� ����
    /// </summary>
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
        else//����Ҷ� �⺻ �߷����� ����
        {
            rigid.gravityScale = defaultGravity;
        }

        if (Input.GetKeyUp(KeyCode.Space))//���� ������ ����
        {
            jumpCheck = true;
        }

        //������ ���� �̵��ӵ� ����
    }

    /// <summary>
    /// ȸ��(�뽬)���
    /// </summary>
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

        if(isGround == true)//���߿��� ���ӻ�� ����
        {
            dashCheck = true;
        }

        jumpForce = dashVertical;
    }

    /// <summary>
    /// ��Ÿ��
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
    /// ���� ����
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
    /// �����ϱ�
    /// </summary>
    private void playerAttack()
    {
        if (Input.GetMouseButtonDown(0) && attackCoolTime == 0)
        {

            anim.SetTrigger("isAttack");
            attackAnimTime = anim.GetCurrentAnimatorStateInfo(0).length;//�ִϸ��̼� ��Ÿ��
            attackCoolTime = attackAnimTime *0.7f;

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

            foreach(Collider2D collider in collider2Ds)//�߸�
            {
                if (collider.tag == "Enemy")
                {
                    collider.GetComponent<Enemy>().Damage(atk);
                }
            }
        }
    }

    /// <summary>
    /// Ÿ�̸� ���
    /// </summary>
    private void coolTimeTimer()
    {
        if (dashTimer > 0f)///�뽬 ���ð�
        {
            dashTimer -= Time.deltaTime;
            if(dashTimer < 0f)//�뽬 �ð��� ��������
            {
                dashTimer = 0f;
                doDash = 0f;
                tr.enabled = false;
                tr.Clear();
            }
        }

        if(attackCoolTime > 0f)//������Ÿ��
        {
            attackCoolTime -= Time.deltaTime;
            if(attackCoolTime < 0f)
            {
                attackCoolTime = 0f;
            }
        }

        if (climbTimer > 0f)//��Ÿ�����ѽð�
        {
            climbTimer -= Time.deltaTime;
            if (climbTimer < 0f)
            {
                climbTimer = 0f;
            }
        }

        if (safeTimer > 0f)//�ǰ���Ÿ��
        {
            safeTimer -= Time.deltaTime;
            if (safeTimer < 0f)
            {
                safeTimer = 0f;
            }
        }
    }

    /// <summary>
    /// ��Ʈ�ڽ� ����
    /// </summary>
    /// <param name="_hitType"></param>
    /// <param name="_coll"></param>
    //public void TriggerEnter(HitBox.enumHitType _hitType, Collider2D _coll)
    //{
    //    switch (_hitType)
    //    {
    //        case HitBox.enumHitType.WallCheck:
    //            checkWall = true;
    //            break;

    //    }
    //}

    ///// <summary>
    ///// ��Ʈ�ڽ� ����
    ///// </summary>
    ///// <param name="_hitType"></param>
    ///// <param name="_coll"></param>
    //public void TriggerExit(HitBox.enumHitType _hitType, Collider2D _coll)
    //{
    //        switch (_hitType)
    //    {
    //        case HitBox.enumHitType.WallCheck:
    //            checkWall = false;
    //            break;
                
    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

}
