using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("�� ����")]
    [SerializeField] bool isSpawn = false;//����üũ
    [SerializeField] List<GameObject> listEnemy;//�� ����
    List<GameObject> listSoawnEnemy = new List<GameObject>();//������ ��

    [SerializeField] float spawnTime = 1.0f;
    float sTimer = 0.0f; //Ÿ�̸�
    [SerializeField] Transform[] trsSpawnPoint;//��������
    [SerializeField] GameObject spawnPount;

    void Start()
    {
        
    }

    void Update()
    {
        checkSpawn();
    }

    /// <summary>
    /// �������ð�, ����Ȯ��
    /// </summary>
    private void checkSpawn()
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

        int count = trsSpawnPoint.Length;
        for (int iNum = 0; iNum < count; ++iNum)
        { 
            Vector3 newPos = trsSpawnPoint[iNum].position;
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
}
