using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class GameOverButtonColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("����")]
    [SerializeField] TextMeshProUGUI buttonText;

    [Header("�ٲ��")]
    [SerializeField] Color touchColor;

    [Header("������")]
    Color orignTextColor;

    // Start is called before the first frame update
    void Start()
    {
        orignTextColor = buttonText.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        touchColor.a = 1f;
        buttonText.color = touchColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = orignTextColor;
    }
}
