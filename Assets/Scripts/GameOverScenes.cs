using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScenes : MonoBehaviour
{
    [SerializeField] GameObject soundMenu;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 사운드 켜기
    /// </summary>
    public void SoundMenu()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        soundMenu.SetActive(true);
    }

    /// <summary>
    /// 메인화면으로 이동
    /// </summary>
    public void ReturnGame()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        SceneManager.LoadScene("Main");
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
