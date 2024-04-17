using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject sendObj;
    [SerializeField] GameObject cam;

    // Start is called before the first frame update
    void Start()
    {

    }

    
    private void OnTriggerEnter2D(Collider2D collision)//플레이어 추락시 이동
    {
        player.DieEffect();
        playerObj.transform.position = sendObj.transform.position;
        //cam.transform.position = new Vector3(sendObj.transform.position.x, sendObj.transform.position.y + 1.5f, cam.transform.position.z);
    }
}
