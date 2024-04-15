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
    [SerializeField] Transform trsSpawnPoint;//스폰지점

    void Start()
    {
        
    }

    void Update()
    {
        //checkSpawn();
    }

    /// <summary>
    /// 적스폰시간, 조건확인
    /// </summary>
    private void checkSpawn()
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
        GameObject objEnemy = listEnemy[0];

        Vector3 newPos = trsSpawnPoint.position;
        GameObject go = Instantiate(objEnemy, newPos, Quaternion.identity);
    }

    public void bulletSpawn()
    {
        GameObject objBullet = listEnemy[1];

        Vector3 newPos = trsSpawnPoint.position;
        GameObject go = Instantiate(objBullet, newPos, Quaternion.identity);
    }
}
