using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("카메라 흔들기")]
    [SerializeField] Transform player;
    [SerializeField] float shakeAmount;
    float shakeTime;
    Vector3 mainPosition;
    float camPosY;

    public void ShakeCamera(float _shakeTime)
    {
        shakeTime = _shakeTime;

        StopCoroutine("shakeCamerPositoin");
        StartCoroutine("shakeCamerPositoin");
    }

    private void Update()
    {
        camPosY = player.position.y + 1.5f;
        mainPosition = new Vector3(player.position.x, camPosY, -10f);
    }

    private IEnumerator shakeCamerPositoin()
    {
        while(shakeTime > 0f)
        {
            transform.position = mainPosition + Random.insideUnitSphere * shakeAmount;

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = mainPosition;
    }
}
