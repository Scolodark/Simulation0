using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    [Header("ī�޶� ����")]
    [SerializeField] Transform player;
    [SerializeField] float shakeAmount;
    float shakeTime;
    Vector3 mainPosition;
    float camPosY;
    bool isShaking;

    [Header("ī�޶� ��ġ �Ѱ輳��")]
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] BoxCollider2D bossRoomBoxColl;
    [SerializeField] GameObject sendObj;
    [SerializeField] GameObject reSpawnObj;
    Camera cam;
    Bounds bounds;

    bool check;
    
    /// <summary>
    /// ��鸱��
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
        bounds = boxCollider.bounds;//��׶���� ���� �ٿ�� ũ�⸦ �����ؿ�, �ݹ��� ����
        cam = Camera.main;
        checkBound();
    }

    /// <summary>
    /// ī�޶� �̵��ϴ� ��������
    /// </summary>
    private void checkBound()
    {
        float heigh = cam.orthographicSize;
        float width = heigh * cam.aspect;

        float minX = bounds.min.x + width;//X�� ī�޶� ũ�⸸ŭ ��������
        float maxX = bounds.extents.x - width;//X�� ī�޶� ũ�⸸ŭ ��������

        float minY = bounds.min.y + heigh;//Y�� ī�޶� ũ�⸸ŭ ����
        float maxY = bounds.extents.y - heigh;//Y�� ī�޶� ũ�⸸ŭ �Ʒ���

        bounds.SetMinMax(new Vector3(minX,minY), new Vector3(maxX,maxY));//�ٿ�� ����� ������ ����
    }


    private void Update()
    {
        camPosY = player.position.y + 1.5f;
        mainPosition = new Vector3(player.position.x, camPosY, cam.transform.position.z);
        boundsType();
        camMoveDistance();
    }

    private void boundsType()
    {
        if(cam.transform.position.x == sendObj.transform.position.x)
        {
            bounds = bossRoomBoxColl.bounds;
        }

        if(player.transform.position == reSpawnObj.transform.position)
        {
            bounds = boxCollider.bounds;
        }
    }

    /// <summary>
    /// ī�޶� ������ ���� �۵�
    /// </summary>
    private void camMoveDistance()
    {
        
        if (player == null || isShaking == true)
        {
            return;
        }

        cam.transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, bounds.min.x, bounds.max.x),
            Mathf.Clamp(player.transform.position.y+1.5f, bounds.min.y, bounds.max.y), cam.transform.position.z);
    }

    /// <summary>
    /// ��鸮�� ������ �ð�
    /// </summary>
    /// <returns></returns>
    private IEnumerator shakeCamerPositoin()
    {
        while(shakeTime > 0f)
        {
            transform.position = mainPosition + Random.insideUnitSphere * shakeAmount;

            shakeTime -= Time.deltaTime;

            isShaking = true;

            yield return null;
        }

        transform.position = mainPosition;
        isShaking = false;
    }
}
