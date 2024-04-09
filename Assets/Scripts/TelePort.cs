using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePort : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject sendObj;

    /// <summary>
    /// 플레이어 확인
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerObj = collision.gameObject;
        }
    }

    /// <summary>
    /// 플레이어 이동
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && sendObj != null)
        {
            StartCoroutine(Teleport());
        }
    }

    IEnumerator Teleport()
    {
        yield return null;
        playerObj.transform.position = sendObj.transform.position;
    }

}
