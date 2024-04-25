using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("�� ����")]
    [SerializeField] bool isSpawn = false;//����üũ
    [SerializeField] List<GameObject> listEnemy;//�� ����
    List<GameObject> listSoawnEnemy = new List<GameObject>();//������ ��

    [SerializeField] float spawnTime = 1.0f;
    float sTimer = 0.0f; //Ÿ�̸�
    [SerializeField] Transform[] trsSpawnPoint;//��������
    [SerializeField] Transform[] enemySpawnPoint;
    [SerializeField] GameObject spawnPount;

    [Header("ü��Ui")]
    [SerializeField] BoxCollider2D bossRoom;
    [SerializeField] GameObject hpUiObj;

    [Header("�ȳ�Ui")]
    [SerializeField] BoxCollider2D monitorColl;
    [SerializeField] GameObject pressZ;

    [Header("���� ui")]
    [SerializeField] GameObject settingObj;
    [SerializeField] GameObject manualObj;
    [SerializeField] GameObject settingMenuObj;
    [SerializeField] Slider playerHpSlider;
    [SerializeField] Slider playerAtkSlider;
    [SerializeField] Slider bossHpSlider;
    [SerializeField] Slider bossAtkSlider;
    public float PlayerHp;
    public float PlayerAtk;
    public float BossHp;
    public float BossAtk;
    int check;

    [Header("ESC�޴�")]
    [SerializeField] GameObject escObj;
    int check2;

    void Start()
    {
        settingObj.SetActive(false);
        escObj.SetActive(false);
        SoundManager.Instance.PlayBgm(SoundManager.Bgm.Lobby);
    }

    void Update()
    {
        hpShowAndHide();
        pressZShowAndHide();
        settingUiShowAndHide();
        escMenu();
    }

    /// <summary>
    /// �������ð�, ����Ȯ��
    /// </summary>
    public void checkSpawn()
    {
        if (isSpawn == false) return;

        sTimer += Time.deltaTime;
        if (sTimer >= spawnTime)
        {
            sTimer = 0.0f;
            spawnEnemy();//�������
        }
    }

    /// <summary>
    /// �� ����
    /// </summary>
    private void spawnEnemy()
    {
        GameObject objEnemy = listEnemy[0];//������� ����

        int count = enemySpawnPoint.Length;
        for (int iNum = 0; iNum < count; ++iNum)
        { 
            Vector3 newPos = enemySpawnPoint[iNum].position;
            GameObject go = Instantiate(objEnemy, newPos, Quaternion.identity);
        }
    }

    /// <summary>
    /// ź�� ����
    /// </summary>
    public void bulletSpawn()
    {
        GameObject objBullet = listEnemy[1];

        int count = trsSpawnPoint.Length;
        for (int iNum = 0; iNum < count; ++iNum)
        {
            Vector3 newPos = trsSpawnPoint[iNum].position;
            GameObject go = Instantiate(objBullet, newPos, Quaternion.identity);
        }
    }
    /// <summary>
    /// ü��Ui���� ����
    /// </summary>
    private void hpShowAndHide()
    {
        if (bossRoom.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            hpUiObj.transform.localScale = new Vector3 (1f, 1f, 1f);
        }
        else
        {
            hpUiObj.transform.localScale = new Vector3(0f, 1f, 1f);
        }
    }

    /// <summary>
    /// Press 'Z'!
    /// </summary>
    private void pressZShowAndHide()
    {
        if (monitorColl.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            pressZ.SetActive(true);
        }
        else
        {
            pressZ.SetActive(false);
        }
    }

    /// <summary>
    /// ����â �߱�
    /// </summary>
    private void settingUiShowAndHide()
    {
        if (monitorColl.IsTouchingLayers(LayerMask.GetMask("Player")) && Input.GetKeyDown(KeyCode.Z))
        {
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
            settingMenuObj.SetActive(false);
            settingObj.SetActive(true);
            manualObj.SetActive(false);
            check++;
        }
        else if(Input.GetKeyUp(KeyCode.Z) && check > 1 || monitorColl.IsTouchingLayers(LayerMask.GetMask("Player")) == false)
        {
            settingObj.SetActive(false);
            check = 0;
        }

        if(settingObj.activeSelf == false)
        {
            check = 0;
        }
    }

    /// <summary>
    /// �÷��̾� �����̵尪�� ����
    /// </summary>
    public void PlayerStatusChange()
    {
        PlayerHp = playerHpSlider.value;
        PlayerAtk = playerAtkSlider.value;
    }

    /// <summary>
    /// ���� �����̴� ���� ����
    /// </summary>
    public void BossStatusChange()
    {
        BossHp = bossHpSlider.value;
        BossAtk = bossAtkSlider.value;
    }

    private void escMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
            escObj.SetActive(true);
            check2++;
        }
        else if(Input.GetKeyUp(KeyCode.Escape) && check2 > 1)
        {
            escObj.SetActive(false);
            check2 = 0;
        }
    }
}
