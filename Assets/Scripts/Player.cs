using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
    [SerializeField] float parryingDamage;
    bool parryingCheck;
    float attackCoolTime;
    float attackAnimTime;

    [Header("�ǰ� ����")]
    [SerializeField] float safeTime;
    [SerializeField] BarController barController;
    float safeTimer;
    SpriteRenderer spr;
    bool hitDamageCheck;
    float fullHp;

    [Header("���ݹ��� ����")]
    [SerializeField] Transform pos;
    [SerializeField] Vector2 boxSize;
    SpriteRenderer sprAttack;
    bool attackAlphaCheck;
    float attackAlpha;

    //�� �Ͽ�
    [Header("�� ������ ����")]
    [SerializeField] Vector2 overlapBoxSize;
    [SerializeField] GameObject boss;
    [SerializeField] BoxCollider2D enemyCheckColl;
    bool enemyCheck;
    int mouseButtonCheck;
    SpriteRenderer sprLockOn;

    [Header("������")]
    [SerializeField] GameObject jumpEffect;
    GameObject jumpEffectSup;

    [Header("ī�޶�ȿ��")]
    [SerializeField] Camera cameraInsert;
    int hitCheckCount;
    bool hitCountbool;

    [Header("���ó��")]
    [SerializeField] BoxCollider2D reSpwObj;
    [SerializeField] ReSpawn reSpawn;

    [Header("�ɷ�ġ����")]
    [SerializeField] GameManager gameManager;

    /// <summary>
    /// �÷��̾� �ɷ�ġ ����
    /// </summary>
    public void ButtonCheck()
    {
        gameManager.PlayerStatusChange();
        hp = gameManager.PlayerHp;
        atk = gameManager.PlayerAtk;
        fullHp = hp;
    }


    /// <summary>
    /// ���� ������ ������
    /// </summary>
    /// <param name="damge"></param>
    public void Damage(float damge)
    {
        if (parryingCheck == true) return;

        if(safeTimer == 0)
        {
            hp = hp - damge;
            anim.SetTrigger("isDamage");
            safeTimer = safeTime;
            barController.hpGage(fullHp, damge);
        }
    }

    /// <summary>
    /// �������� �޾Ҵ��� üũ
    /// </summary>
    private void damageCheck()
    {
        if(safeTimer != 0)
        {
            hitDamageCheck = true;
        }
        else
        {
            hitDamageCheck = false;
        }
    }


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();
        climbCheckColl = transform.GetChild(0).GetComponent<Collider2D>();
        spr = GetComponent<SpriteRenderer>();
        sprAttack = transform.GetChild(2).GetComponent<SpriteRenderer>();
        sprLockOn = transform.GetChild(3).GetComponent<SpriteRenderer>();

        tr.enabled = false;
    }

    void Start()
    {
        fullHp = hp;
        jumpCheck = true;
        dashVertical = jumpForce;
        doDash = 0f;
        dashCheck = true;

        setAttackEffectAlpha(0f);
    }

    void Update()
    {
        playerMove();
        playerJump();
        playerDash();
        playerClimb();
        speedRanding();

        playerAttack();
        parrying();

        DieEffect();

        hitSprite();

        checkGround();
        enemyLockOn();
        bossChecking();

        coolTimeTimer();

        damageCheck();

        lockOnEffect();
        hitCameraEffect();
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
    /// ���� ������ üũ
    /// </summary>
    private void enemyLockOn()
    {

        Collider2D detectEnemy = Physics2D.OverlapBox(pos.position,overlapBoxSize, 0f, LayerMask.GetMask("Monster"));

        if (detectEnemy)
        {
            if (Input.GetMouseButtonDown(2))
            {
                
                
                enemyCheck = true;
                mouseButtonCheck += 1;
            }

            if (mouseButtonCheck == 2)
            {
                mouseButtonCheck = 0;
                enemyCheck = false;
            }
        }
        else
        {
            enemyCheck = false;
        }
        
    }

    /// <summary>
    /// ���� ��ġ Ȯ��
    /// </summary>
    private void bossChecking()
    {
        if (enemyCheck == true)
        {
            enemyCheckColl.gameObject.SetActive(true);
            if (enemyCheckColl.IsTouchingLayers(LayerMask.GetMask("Monster")) != true)
            {
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }
        }
        else
        {
            enemyCheckColl.gameObject.SetActive(false);
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

        if (moveDir.x != 0.0f && enemyCheck == false)//�÷��̾� �¿� ����
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
            jumpEffectPrefabs();
            
        }
        else if(Input.GetKeyUp(KeyCode.Space) && moveDir.y > 0f && jumpCheck == true && isGround == true)//������
        {
            rigid.velocity = new Vector2(moveDir.x, jumpForce*0.5f);
            jumpCheck = false;
        }

        anim.SetBool("isJump", isGround != true);

        if (moveDir.y < 0)//���ϼӵ�
        {
            rigid.gravityScale = defaultGravity * 1.5f;
        }
        else if (moveDir.y <= 1f && moveDir.y >= 0f)//�ְ� ���� ���� ���ʽ�
        {
            rigid.gravityScale = defaultGravity * 0.2f;
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
        if (hitDamageCheck == true) return;//���ݹ޴� ���� ���� �Ұ�

        if (Input.GetMouseButtonDown(0) && attackCoolTime == 0)
        {
            sprAttack.color = new Color(1,0,0);
            StartCoroutine(FadeInOut());//����Ʈ ǥ��
            anim.SetTrigger("isAttack");
            attackAnimTime = anim.GetCurrentAnimatorStateInfo(0).length;//�ִϸ��̼� ��Ÿ��
            attackCoolTime = attackAnimTime *0.7f;

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);//���� ����

            foreach(Collider2D collider in collider2Ds)//������ ������
            {
                if (collider.tag == "Enemy")
                {
                    collider.GetComponent<Enemy>().Damage(atk);
                }
                else if(collider.tag == "Boss")
                {
                    collider.GetComponent<Boss>().Damage(atk);
                }
            }
        }
    }

    /// <summary>
    /// �и�
    /// </summary>
    private void parrying()
    {
        if (hitDamageCheck == true) return;

        if (Input.GetMouseButtonDown(1) && attackCoolTime == 0)
        {
            sprAttack.color = new Color(0, 0, 1);
            StartCoroutine(FadeInOut());//����Ʈ ǥ��
            anim.SetTrigger("isAttack");
            attackAnimTime = anim.GetCurrentAnimatorStateInfo(0).length;//�ִϸ��̼� ��Ÿ��
            attackCoolTime = attackAnimTime;

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

            foreach (Collider2D collider in collider2Ds)//������ ������
            {
                if (collider.tag == "Enemy")
                {
                    collider.GetComponent<Enemy>().Damage(parryingDamage);
                }
                else if (collider.tag == "Boss")
                {
                    collider.GetComponent<Boss>().Damage(atk);
                }
            }
        }
    }

    /// <summary>
    /// �ǰ��� ����Ÿ�� ȿ��
    /// </summary>
    private void hitSprite()
    {
        if(safeTimer > 0)
        {
            setPlayerColorAlpha(0.5f);
        }
        else
        {
            setPlayerColorAlpha(1f);
        }
    }

    /// <summary>
    /// �÷��̾� �����
    /// </summary>
    private void DieEffect()
    {
        if(hp <= 0)
        {
            anim.SetTrigger("Die");
            if (reSpwObj.IsTouchingLayers(LayerMask.GetMask("Player")))
            {
                hp = fullHp;
                anim.SetTrigger("isRespawn");
                reSpawn.RecoverHp();
            }
            else if(isGround == true)
            {
                SceneManager.LoadScene("GameOver");
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

    private void OnDrawGizmos()//���ݹ���ǥ��
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);

        Gizmos.color = Color.yellow;//�� �ν� ����
        Gizmos.DrawWireCube(pos.position, overlapBoxSize);
    }
    

    private void setPlayerColorAlpha(float _a)//�÷��̾� ���İ�
    {
        Color color = spr.color;
        color.a = _a;
        spr.color = color;
    }

    private void setAttackEffectAlpha(float _a)//��������Ʈ ���İ�
    {
        Color color = sprAttack.color;
        color.a = _a;
        sprAttack.color = color;
    }

    /// <summary>
    /// ���� ����Ʈ ����
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeInOut()
    {
        float ratio = 0.0f;

        while (attackAlpha < 1.0f)//���İ��� 1->0���� ������ �̵�
        {
            if (Input.GetMouseButtonDown(1) && attackCoolTime == 0)
            {
                parryingCheck = true;
            }
            attackAlpha = Mathf.Lerp(0f, 1.0f, ratio);
            ratio += Time.deltaTime/0.5f;
            setAttackEffectAlpha(attackAlpha);
            yield return null;
        }
        while (attackAlpha > 0.0f)//���İ��� 0->1�� ������ �̵�
        {
            parryingCheck = false;
            attackAlpha = Mathf.Lerp(1.0f, 0.0f, ratio);
            ratio += Time.deltaTime / 0.5f;
            setAttackEffectAlpha(attackAlpha);
            yield return null;
        }
    }

    /// <summary>
    /// ���� ����Ʈ ����
    /// </summary>
    private void lockOnEffect()
    {
        if(enemyCheck == true)
        {
            anim.SetBool("Battle", true);
            sprLockOn.gameObject.SetActive(true);
        }
        else
        {
            anim.SetBool("Battle", false);
            sprLockOn.gameObject.SetActive(false);
        }
            
    }


    /// <summary>
    /// ���� ����Ʈ ������ ������ �ı�
    /// </summary>
    private void jumpEffectPrefabs()
    {
        Destroy(Instantiate(jumpEffect,transform.position,Quaternion.identity), 0.4f);
    }

    private void hitCameraEffect()
    {
        if (hitDamageCheck == true && hitCountbool == false)
        {
            hitCheckCount += 1;

        }
        else if(hitDamageCheck == false)
        {
            hitCountbool = false;
        }

        if (hitCheckCount == 1)
        {
            hitCheckCount = 0;
            hitCountbool = true;
            cameraInsert.GetComponent<CameraSetting>().ShakeCamera(0.1f);
        }
    }
}
