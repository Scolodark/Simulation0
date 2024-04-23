using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScenesUi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        SceneManager.LoadScene("Simulatoin0");
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
