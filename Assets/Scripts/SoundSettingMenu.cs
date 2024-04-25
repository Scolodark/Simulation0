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

        //���� �ҷ�����
        string value = PlayerPrefs.GetString(volumeKey, "");
        if (value != string.Empty)//������ �Ǿ��־��ٸ�
        {
            volume.value = JsonConvert.DeserializeObject<float>(value);
            soundManger.sfxVolume = volume.value;
        }
        //bgm���� �ҷ�����
        string bgmValue = PlayerPrefs.GetString(bgmVolumeKey, "");
        if(bgmValue != string.Empty)
        {
            bgmVolume.value = JsonConvert.DeserializeObject<float>(value);
            soundManger.bgmVolume = volume.value;
        }

        //���� ����� ��� ����
        volume.onValueChanged.AddListener((sliderVolume) => //����
        {
            soundManger.sfxVolume = sliderVolume;
            string json = JsonConvert.SerializeObject(soundManger.sfxVolume);
            PlayerPrefs.SetString(volumeKey, json);
        });
        //bgm���� ����� ����
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
    /// �����ؽ�Ʈ
    /// </summary>
    private void volumeTxt()
    {
        float sliderTxt = Mathf.Ceil(volume.value);
        txt.text = sliderTxt.ToString();

        float sliderBgmTxt = Mathf.Ceil(bgmVolume.value);
        Bgmtxt.text = sliderBgmTxt.ToString();
    }

    /// <summary>
    /// SFX��������
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
    /// Bgm��������
    /// </summary>
    private void setBgmVolum()
    {
        soundManger.bgmVolume = bgmVolume.value;
    }

    /// <summary>
    /// ���Ұ� ��ư
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
    /// ����
    /// </summary>
    public void Exit()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        menu.SetActive(false);
    }
}
