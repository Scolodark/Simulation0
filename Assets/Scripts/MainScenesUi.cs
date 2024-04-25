using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScenesUi : MonoBehaviour
{
    [SerializeField] GameObject soundMenu;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBgm(SoundManager.Bgm.Main);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ���ӽ���
    /// </summary>
    public void GameStart()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        SceneManager.LoadScene("Simulatoin0");
    }

    /// <summary>
    /// ����
    /// </summary>
    public void SoundMenu()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        soundMenu.SetActive(true);
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
