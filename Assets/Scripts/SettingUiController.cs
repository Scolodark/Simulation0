using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingUiController : MonoBehaviour
{

    [Header("���� �޴���")]
    [SerializeField] GameObject helpManualObj;
    [SerializeField] GameObject settingMenuObj;

    [Header("�÷��̾� ���̵� ��������")]
    [SerializeField] TextMeshProUGUI playerHpTxt;
    [SerializeField] TextMeshProUGUI playerAtkTxt;
    [SerializeField] Slider playerHpSlider;
    [SerializeField] Slider playerAtkSlider;

    [Header("���� ���̵� ��������")]
    [SerializeField] GameObject levelChangeMenuObj;
    [SerializeField] TextMeshProUGUI bossHpTxt;
    [SerializeField] TextMeshProUGUI bossAtkTxt;
    [SerializeField] Slider bossHpSlider;
    [SerializeField] Slider bossAtkSlider;

    private void Start()
    {
        playerHpSlider.onValueChanged.AddListener(delegate { PlayerLevelChangeMenuSetting();});
        playerAtkSlider.onValueChanged.AddListener(delegate { PlayerLevelChangeMenuSetting();});
    }


    /// <summary>
    /// ���̵� ���� �޴� �ѱ�
    /// </summary>
    public void SettingMenu()
    {
        settingMenuObj.SetActive(true);
    }

    /// <summary>
    /// �÷��̾� ���̵� �����޴� ����
    /// </summary>
    public void PlayerLevelChangeMenuSetting()
    {
        playerHpTxt.text = playerHpSlider.value.ToString();
        playerAtkTxt.text = playerAtkSlider.value.ToString();
    }

    /// <summary>
    /// ���� ���̵� �����޴� ����
    /// </summary>
    public void BossLevelChangeMenuSetting()
    {
        bossHpTxt.text = bossHpSlider.value.ToString();
        bossAtkTxt.text = bossAtkSlider.value.ToString();
    }

    /// <summary>
    /// ���̵� �����޴� ����
    /// </summary>
    public void LevelChangeMenuExit()
    {
        levelChangeMenuObj.SetActive(false);
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
        SceneManager.LoadScene("Main");
    }
}
