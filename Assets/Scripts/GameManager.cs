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
    [SerializeField] CanvasScaler bossHpBar;

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

    void Start()
    {
        settingObj.SetActive(false);
    }

    void Update()
    {
        hpShowAndHide();
        pressZShowAndHide();
        settingUiShowAndHide();
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
            bossHpBar.scaleFactor = 1;
        }
        else
        {
            bossHpBar.scaleFactor = 0;
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
}
