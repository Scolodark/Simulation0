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
    [SerializeField] Transform trsSpawnPoint;//��������

    void Start()
    {
        
    }

    void Update()
    {
        //checkSpawn();
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
