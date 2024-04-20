using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Boss boss;

    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject sendObj;
    [SerializeField] GameObject cam;

    [SerializeField] GameObject bossObj;
    [SerializeField] GameObject bossTpObj;

    [SerializeField] GameManager gameManager;

    float playerHp;
    float bossHp;

    // Start is called before the first frame update
    void Start()
    {

    }

    
    private void OnTriggerEnter2D(Collider2D collision)//플레이어 추락시 이동
    {
        playerHp = gameManager.PlayerHp;

        player.Damage(playerHp);

        playerObj.transform.position = sendObj.transform.position;
        bossObj.transform.position = new Vector2(bossTpObj.transform.position.x, playerObj.transform.position.y);
    }
}
