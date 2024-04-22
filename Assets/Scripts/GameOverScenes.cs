using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScenes : MonoBehaviour
{



    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>
    /// 메인화면으로 이동
    /// </summary>
    public void ReturnGame()
    {
        SceneManager.LoadScene("Main");
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
    }

    /// <summary>
    /// 게임종료
    /// </summary>
    public void ExitGame()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else

        Application.Quit();
        Debug.Log("종료");
#endif

    }
}
