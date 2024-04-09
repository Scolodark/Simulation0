using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    [Header("카메라 흔들기")]
    [SerializeField] Transform player;
    [SerializeField] float shakeAmount;
    float shakeTime;
    Vector3 mainPosition;
    float camPosY;

    [Header("카메라 위치 한계설정")]
    [SerializeField] BoxCollider2D boxCollider;
    Camera cam;
    Bounds bounds;
    
    /// <summary>
    /// 흔들릴때
    /// </summary>
    /// <param name="_shakeTime"></param>
    public void ShakeCamera(float _shakeTime)
    {


        shakeTime = _shakeTime;

        StopCoroutine("shakeCamerPositoin");
        StartCoroutine("shakeCamerPositoin");
    }

    private void Start()
    {
        cam = Camera.main;
        checkBound();
    }

    /// <summary>
    /// 카메라가 이동하는 영역만듬
    /// </summary>
    private void checkBound()
    {
        float heigh = cam.orthographicSize;
        float width = heigh * cam.aspect;

        bounds = boxCollider.bounds;//백그라운드로 부터 바운드 크기를 복사해옴, 콜바이 벨류

        float minX = bounds.min.x + width;//X를 카메라 크기만큼 우측으로
        float maxX = bounds.extents.x - width;//X를 카메라 크기만큼 좌측으로

        float minY = bounds.min.y + heigh;//Y를 카메라 크기만큼 위로
        float maxY = bounds.extents.y - heigh;//Y를 카메라 크기만큼 아래로

        bounds.SetMinMax(new Vector3(minX,minY), new Vector3(maxX,maxY));//바운즈를 계산한 값으로 수정
    }


    private void Update()
    {
        camPosY = player.position.y + 1.5f;
        mainPosition = new Vector3(player.position.x, camPosY, -10f);
        //camMoveDistance();
    }

    private void camMoveDistance()
    {
        if (player == null)
        {
            return;
        }

        cam.transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, bounds.min.x, bounds.max.x),
            Mathf.Clamp(player.transform.position.y, bounds.min.y, bounds.max.y), cam.transform.position.z);
    }

    /// <summary>
    /// 흔들리는 범위와 시간
    /// </summary>
    /// <returns></returns>
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
