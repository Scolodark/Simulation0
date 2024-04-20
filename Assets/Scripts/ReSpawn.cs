using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawn : MonoBehaviour
{
    [Header("체력바 회복")]
    [SerializeField] GameManager gameManager;
    [SerializeField] BarController playerBar;
    [SerializeField] BarController bossBar;
    BoxCollider2D boxColl;

    // Start is called before the first frame update
    void Start()
    {
        boxColl = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 플레이어 체력바 회복
    /// </summary>
    public void RecoverHp()
    {
        if (boxColl.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            playerBar.hpGage(gameManager.PlayerHp, gameManager.PlayerHp*-1);
            Debug.Log(gameManager.PlayerHp);
        }
    }

    public void BossRecoverHp()
    {
        if (boxColl.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            bossBar.hpGage(gameManager.BossHp, gameManager.BossHp * -1);
        }
    }
}
