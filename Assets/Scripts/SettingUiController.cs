using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingUiController : MonoBehaviour
{
    [SerializeField] GameObject thisMenu;

    [Header("게임 메뉴얼")]
    [SerializeField] GameObject helpManualObj;
    [SerializeField] GameObject settingMenuObj;

    [Header("플레이어 난이도 조절세팅")]
    [SerializeField] TextMeshProUGUI playerHpTxt;
    [SerializeField] TextMeshProUGUI playerAtkTxt;
    [SerializeField] Slider playerHpSlider;
    [SerializeField] Slider playerAtkSlider;

    [Header("보스 난이도 조절세팅")]
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
    /// 난이도 조절 메뉴 켜기
    /// </summary>
    public void SettingMenu()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        settingMenuObj.SetActive(true);
    }

    /// <summary>
    /// 플레이어 난이도 조절메뉴 설정
    /// </summary>
    public void PlayerLevelChangeMenuSetting()
    {
        playerHpTxt.text = playerHpSlider.value.ToString();
        playerAtkTxt.text = playerAtkSlider.value.ToString();
    }

    /// <summary>
    /// 보스 난이도 조절메뉴 설정
    /// </summary>
    public void BossLevelChangeMenuSetting()
    {
        bossHpTxt.text = bossHpSlider.value.ToString();
        bossAtkTxt.text = bossAtkSlider.value.ToString();
    }

    /// <summary>
    /// 난이도 조절메뉴 끄기
    /// </summary>
    public void LevelChangeMenuExit()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        levelChangeMenuObj.SetActive(false);
    }

    /// <summary>
    /// 도움말 기능
    /// </summary>
    public void HelpText()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        helpManualObj.SetActive(true);
    }

    /// <summary>
    /// 도움말기능 끄기
    /// </summary>
    public void ExitHelpText()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        helpManualObj.SetActive(false);
    }

    /// <summary>
    /// 게임종료
    /// </summary>
    public void ExitGame()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        thisMenu.SetActive(false);
    }
}
