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
        SceneManager.LoadScene("Main");
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
