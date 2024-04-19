using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUiController : MonoBehaviour
{

    [Header("���� �޴���")]
    [SerializeField] GameObject helpManualObj;
    


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// ���� ���
    /// </summary>
    public void HelpText()
    {
        helpManualObj.SetActive(true);
    }

    /// <summary>
    /// ���򸻱�� ����
    /// </summary>
    public void ExitHelpText()
    {
        helpManualObj.SetActive(false);
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else

        Application.Quit();
        Debug.Log("����");
#endif
        
    }
}
