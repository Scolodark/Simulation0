using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class GameOverButtonColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("글자")]
    [SerializeField] TextMeshProUGUI buttonText;

    [Header("바뀔색")]
    [SerializeField] Color touchColor;

    [Header("원래색")]
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
