using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    [SerializeField] RectTransform curHbBar;
    float hpBar;
    float hp;

    void Start()
    {
        hpBar = curHbBar.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void hpGage(float _hp, float _damage)
    {
        hp = curHbBar.sizeDelta.x + -_damage*(hpBar / _hp);
        curHbBar.sizeDelta = new Vector2(hp, curHbBar.sizeDelta.y);
    }
}
