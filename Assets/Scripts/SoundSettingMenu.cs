using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

public class SoundSettingMenu : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] GameObject menu;
    [SerializeField] Slider volume;
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] Image muteButton;
    SoundManager soundManger;
    string volumeKey = "VolumeKey";

    [Header("BGM")]
    [SerializeField] Slider bgmVolume;
    [SerializeField] TextMeshProUGUI Bgmtxt;
    string bgmVolumeKey = "BgmVolumeKey";
    

    bool muteCheck;

    

    void Start()
    {
        menu.SetActive(false);
        soundManger = SoundManager.Instance;

        //볼륨 불러오기
        string value = PlayerPrefs.GetString(volumeKey, "");
        if (value != string.Empty)//저장이 되어있었다면
        {
            volume.value = JsonConvert.DeserializeObject<float>(value);
            soundManger.sfxVolume = volume.value;
        }
        //bgm볼륨 불러오기
        string bgmValue = PlayerPrefs.GetString(bgmVolumeKey, "");
        if(bgmValue != string.Empty)
        {
            bgmVolume.value = JsonConvert.DeserializeObject<float>(value);
            soundManger.bgmVolume = volume.value;
        }

        //볼륨 변경시 기능 예약
        volume.onValueChanged.AddListener((sliderVolume) => //예약
        {
            soundManger.sfxVolume = sliderVolume;
            string json = JsonConvert.SerializeObject(soundManger.sfxVolume);
            PlayerPrefs.SetString(volumeKey, json);
        });
        //bgm볼륨 변경시 예약
        bgmVolume.onValueChanged.AddListener((sliderBgmVolume) =>
        {
            soundManger.bgmVolume = sliderBgmVolume;
            string json = JsonConvert.SerializeObject(soundManger.bgmVolume);
            PlayerPrefs.SetString(bgmVolumeKey, json);
        });
    }

    void Update()
    {
        volumeTxt();
        //setVolume();
        muteButtonColor();
        //setBgmVolum();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(false);
        }
    }

    /// <summary>
    /// 볼륨텍스트
    /// </summary>
    private void volumeTxt()
    {
        float sliderTxt = Mathf.Ceil(volume.value);
        txt.text = sliderTxt.ToString();

        float sliderBgmTxt = Mathf.Ceil(bgmVolume.value);
        Bgmtxt.text = sliderBgmTxt.ToString();
    }

    /// <summary>
    /// SFX볼륨조절
    /// </summary>
    private void setVolume()
    {
        soundManger.sfxVolume =volume.value;
    }
    public void SetVolumeSound()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
    }

    /// <summary>
    /// Bgm볼륨조절
    /// </summary>
    private void setBgmVolum()
    {
        soundManger.bgmVolume = bgmVolume.value;
    }

    /// <summary>
    /// 음소거 버튼
    /// </summary>
    public void MuteSfx()
    {
        volume.value = 0f;
        soundManger.sfxVolume = 0f;
        bgmVolume.value = 0f;
        soundManger.bgmVolume = 0f;

        if(muteCheck == true)
        {
            volume.value = 50f;
            soundManger.sfxVolume = 50f;
            bgmVolume.value = 50f;
            soundManger.bgmVolume = 50f;
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        }
    }
    private void muteButtonColor()
    {
        if(soundManger.sfxVolume == 0f)
        {
            muteButton.color = new Color(1, 0, 0);
            muteCheck = true;
        }
        else
        {
            muteButton.color = new Color(1, 1, 1);
            muteCheck = false;
        }
    }

    /// <summary>
    /// 끄기
    /// </summary>
    public void Exit()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        menu.SetActive(false);
    }
}
