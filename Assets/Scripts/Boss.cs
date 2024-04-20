using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("�ɷ�ġ")]
    [SerializeField] float hp;
    [SerializeField] float atk;

    [Header("�̵�")]
    [SerializeField] float speed;
    [SerializeField] Collider2D wallCheckColl;
    Vector3 trueScale;
    Vector3 falseScale;
    Rigidbody2D rigid;
    bool isRight;

    Animator anim;

    [Header("�÷��̾� ����")]
    [SerializeField] GameObject player;

    [Header("�������� ���")]
    [SerializeField] Collider2D playerCloseUpCheckColl;
    [SerializeField] Transform CloseAttackPos;
    [SerializeField] Vector2 CloseAttackCover;
    bool closeAttack;
    bool closeAttacking;
    bool backStepCheck;
    bool invincible;

    [Header("���Ÿ����� ���")]
    [SerializeField] Collider2D playerLongDistanceCheckColl;
    [SerializeField] Transform longDistanceAttacPos;
    [SerializeField] Vector2 longDistanceAttackCover;
    bool longDistanceCheck;
    bool longDistanceAttackEnd;

    [Header("ź�� ����")]
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletCount;
    [SerializeField] GameObject spawnPointObj;

    SpriteRenderer spr;

    [SerializeField] GameManager gameManager;

    [Header("�÷��̾ ���� �����̵�")]
    [SerializeField] Collider2D teleportCheckColl;

    [Header("���� �浹�� ����")]
    [SerializeField] BoxCollider2D backCheck;
    [SerializeField] GameObject teleportTpEscape;
    bool isWallCheck;

    [Header("�ǰ�ȿ��")]
    [SerializeField] BarController barController;

    [Header("2������")]
    bool phase;
    float fullHp;

    [Header("���")]
    bool deathCheck;

    [Header("�ɷ�ġ ����")]
    [SerializeField] Button checkButton;
    [SerializeField] GameObject reSpawnObj;
    [SerializeField] ReSpawn reSpawn;
    

    /// <summary>
    /// ���� �ɷ�ġ ����
    /// </summary>
    public void BossStatousSetting()
    {
        gameManager.BossStatusChange();
        hp = gameManager.BossHp;
        atk = gameManager.BossAtk;
        fullHp = hp;
    }

    /// <summary>
    /// ���ݹ�����
    /// </summary>
    /// <param name="damge"></param>
    public void Damage(float damge)
    {
        if(invincible == false)
        {
            hp = hp - damge;
            anim.SetTrigger("isDamage");
            barController.hpGage(fullHp, damge);
            //���ݹ����� ���� �ٲ�
        }
    }

    void Start()
    {
        fullHp = hp;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();

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
        longDistanceAttack();
        teleportToPlayer();
        phaseChack();
        death();
        wallCheck();
        hpReset();
    }

    /// <summary>
    /// �÷��̾ ������ �ɽ� ���� ü�� �ʱ�ȭ
    /// </summary>
    private void hpReset()
    {
        if (player.transform.position.x == reSpawnObj.transform.position.x)
        {
            hp = fullHp;
            reSpawn.BossRecoverHp();
        }
    }

    /// <summary>
    /// ������ ��ȯ
    /// </summary>
    private void phaseChack()
    {
        if (fullHp/2 >= hp)
        {
            phase = true;
        }

        if(phase == true)
        {
            //������ ����
            spr.color = new Color(1, 0, 0);
            //�ִϸ��̼� �ӵ� 2��
            anim.speed = 2f;
            speed =10;
            gameManager.checkSpawn();
        }
    }

    private void death()//���� Ȯ��
    {
        if (hp <= 0 && deathCheck == false)
        {
            anim.SetTrigger("isDeath");
            deathCheck = true;
        }
    }
    private void deathEnd()//���� ����� �Ҹ�
    {
        Destroy(gameObject);
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

        if (backStepCheck == true || longDistanceCheck == true) return;

        if (hit)
        {
            if (hit.transform.tag == "Wall" || hit.transform.tag == "Ground" || closeAttack == true)
            {
                anim.SetBool("isMove", false);
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                return;
            }

            if (hit.transform.CompareTag("Player") == true && backStepCheck != true)
            {
                anim.SetBool("isMove", true);
                rigid.velocity = new Vector2(-speed, rigid.velocity.y);

                //�÷��̾� ������ �����ϰ�
                //�� �������� �÷��̾ �����ִ��� Ȯ��
                //����

                isRight = (player.transform.position - transform.position).x > 0;//�÷��̾ ������ �ڷ� ���� true�� ��
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

    /// <summary>
    /// ������ ���� ����
    /// </summary>
    private void attackCheck()
    {
        if(playerCloseUpCheckColl.IsTouchingLayers(LayerMask.GetMask("Player")) == true)
        {
            closeAttack = true;
        }
        else
        {
            closeAttack = false;
            closeAttacking = true;
        }

        if(closeAttacking == true && closeAttack == true)
        {
            anim.SetTrigger("isAttack");
            closeAttacking = false;
        }

        if (closeAttack == true && closeAttacking == false && backStepCheck == false)//���� ���� �ൿ
        {
            anim.SetTrigger("useSpell");
        }
    }

    /// <summary>
    /// ���� ������
    /// </summary>
    /// <returns></returns>
    IEnumerator attackDelay()
    {
        if(phase == false)
        {
            yield return new WaitForSeconds(0.7f);//���� 2����� ���۵ɶ� �ð� ����
        }
        else if( phase == true)
        {
            yield return new WaitForSeconds(0.35f);
        }
       

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(CloseAttackPos.position, CloseAttackCover, 0);

        int count = collider2Ds.Length;
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (collider2Ds[iNum].gameObject.tag == "Player")
            {
                if (closeAttack == true)
                {
                    collider2Ds[iNum].GetComponent<Player>().Damage(atk);
                }
            }
        }
    }

    /// <summary>
    /// ���� ���� �̵�
    /// </summary>
    private void backStep()
    {
        invincible = true;
        Color color = spr.color;
        color.a = 0f;
        spr.color = color;

        backStepCheck = true;

        if (backStepCheck == true)
        {
            rigid.velocity = new Vector2(-rigid.transform.localScale.x * -10f, rigid.velocity.y);
            if (wallCheckColl.IsTouchingLayers(LayerMask.GetMask("TransparentWall")))
            {
                rigid.velocity = new Vector2(0,rigid.velocity.y);
            }
        }
    }

    /// <summary>
    /// ���Ÿ� ����
    /// </summary>
    private void longDistanceAttack()
    {

        if (playerLongDistanceCheckColl.IsTouchingLayers(LayerMask.GetMask("Player")) == true && backStepCheck == true)
        {

            invincible = false;
            Color color = spr.color;
            color.a = 1f;
            spr.color = color;

            longDistanceCheck = true;

            
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
            longDistanceAttackEnd = true;

            spawnBullet();
        }
        else
        {
            spawnPointObj.SetActive(false);
        }
    }

    /// <summary>
    /// ź�� ��ȯ
    /// </summary>
    private void spawnBullet()
    {

        if (longDistanceAttackEnd == true)
        {
            gameManager.bulletSpawn();

            longDistanceCheck = false;
            backStepCheck = false;
            longDistanceAttackEnd = false;
        }
    }

    /// <summary>
    /// üũ�� �÷��̾ ���� �ڷ���Ʈ
    /// </summary>
    private void teleportToPlayer()
    {
        if (teleportCheckColl.IsTouchingLayers(LayerMask.GetMask("Player"))==true)
        {
            anim.SetTrigger("isCast");
            transform.position = new Vector2(player.transform.position.x, transform.position.y);
        }
    }

    /// <summary>
    /// ������ ���� ����� ���� �ൿ
    /// </summary>
    private void wallCheck()
    {
        if (wallCheckColl.IsTouchingLayers(LayerMask.GetMask("TransparentWall")) == true)
        {
            invincible = false;
            Color color = spr.color;
            color.a = 1f;
            spr.color = color;

            isWallCheck = true;
            backStepCheck = false;

            transform.position = new Vector2(teleportTpEscape.transform.position.x, transform.position.y);
        }
    }

    private void OnDrawGizmos()//�������ݹ���ǥ��
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(CloseAttackPos.position, CloseAttackCover);

        longDistanceGizmos();
    }

    /// <summary>
    /// ���Ÿ� ���� ���� ǥ��
    /// </summary>
    private void longDistanceGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(longDistanceAttacPos.position, longDistanceAttackCover);
    }
}
