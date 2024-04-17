using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("탄막 능력치")]
    [SerializeField] float atk;
    [SerializeField] Vector2 atkDistance;
    [SerializeField] Transform atkPos;

    bool isAtk;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    /// <summary>
    /// 탄막의 플레이어 공격 범위
    /// </summary>
    private void atkPlayer()
    {
        if (isAtk != true)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkPos.position, atkDistance, 0);

            int count = collider2Ds.Length;
            for (int iNum = 0; iNum < count; iNum++)
            {
                if (collider2Ds[iNum].gameObject.tag == "Player")
                {
                    collider2Ds[iNum].GetComponent<Player>().Damage(atk);
                }
            }

            isAtk = true;
        }

        if (isAtk == true)
        {
            isAtk = false;
        }
    }

    private void destroyThis()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(atkPos.position, atkDistance);
    }
}
