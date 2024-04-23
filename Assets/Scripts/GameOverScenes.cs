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
    /// ����ȭ������ �̵�
    /// </summary>
    public void ReturnGame()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void ExitGame()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else

        Application.Quit();
        Debug.Log("����");
#endif

    }
}
