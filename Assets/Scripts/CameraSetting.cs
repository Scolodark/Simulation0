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

    [Header("ī�޶� ��ġ �Ѱ輳��")]
    [SerializeField] BoxCollider2D boxCollider;
    Camera cam;
    Bounds bounds;
    
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

        bounds = boxCollider.bounds;//��׶���� ���� �ٿ�� ũ�⸦ �����ؿ�, �ݹ��� ����

        float minX = bounds.min.x + width;//X�� ī�޶� ũ�⸸ŭ ��������
        float maxX = bounds.extents.x - width;//X�� ī�޶� ũ�⸸ŭ ��������

        float minY = bounds.min.y + heigh;//Y�� ī�޶� ũ�⸸ŭ ����
        float maxY = bounds.extents.y - heigh;//Y�� ī�޶� ũ�⸸ŭ �Ʒ���

        bounds.SetMinMax(new Vector3(minX,minY), new Vector3(maxX,maxY));//�ٿ�� ����� ������ ����
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
    /// ��鸮�� ������ �ð�
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
