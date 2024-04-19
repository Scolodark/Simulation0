using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUiController : MonoBehaviour
{

    [Header("게임 메뉴얼")]
    [SerializeField] GameObject helpManualObj;
    


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 도움말 기능
    /// </summary>
    public void HelpText()
    {
        helpManualObj.SetActive(true);
    }

    /// <summary>
    /// 도움말기능 끄기
    /// </summary>
    public void ExitHelpText()
    {
        helpManualObj.SetActive(false);
    }

    /// <summary>
    /// 게임종료
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else

        Application.Quit();
        Debug.Log("종료");
#endif
        
    }
}
