using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("적 생성")]
    [SerializeField] bool isSpawn = false;//스폰체크
    [SerializeField] List<GameObject> listEnemy;//적 종류
    List<GameObject> listSoawnEnemy = new List<GameObject>();//생성된 적

    [SerializeField] float spawnTime = 1.0f;
    float sTimer = 0.0f; //타이마
    [SerializeField] Transform[] trsSpawnPoint;//스폰지점
    [SerializeField] Transform[] enemySpawnPoint;
    [SerializeField] GameObject spawnPount;

    [Header("체력Ui")]
    [SerializeField] BoxCollider2D bossRoom;
    [SerializeField] GameObject hpUiObj;

    [Header("안내Ui")]
    [SerializeField] BoxCollider2D monitorColl;
    [SerializeField] GameObject pressZ;

    [Header("세팅 ui")]
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

    [Header("ESC메뉴")]
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
    /// 적스폰시간, 조건확인
    /// </summary>
    public void checkSpawn()
    {
        if (isSpawn == false) return;

        sTimer += Time.deltaTime;
        if (sTimer >= spawnTime)
        {
            sTimer = 0.0f;
            spawnEnemy();//적기생산
        }
    }

    /// <summary>
    /// 적 스폰
    /// </summary>
    private void spawnEnemy()
    {
        GameObject objEnemy = listEnemy[0];//만들어질 몬스터

        int count = enemySpawnPoint.Length;
        for (int iNum = 0; iNum < count; ++iNum)
        { 
            Vector3 newPos = enemySpawnPoint[iNum].position;
            GameObject go = Instantiate(objEnemy, newPos, Quaternion.identity);
        }
    }

    /// <summary>
    /// 탄막 스폰
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
    /// 체력Ui숨김 보임
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
    /// 세팅창 뜨기
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
    /// 플레이어 슬라이드값을 저장
    /// </summary>
    public void PlayerStatusChange()
    {
        PlayerHp = playerHpSlider.value;
        PlayerAtk = playerAtkSlider.value;
    }

    /// <summary>
    /// 보스 슬라이더 값을 저장
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
