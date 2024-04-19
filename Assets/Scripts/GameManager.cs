using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            hpUiObj.SetActive(true);
        }
        else
        {
            hpUiObj.SetActive(false);
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
}
