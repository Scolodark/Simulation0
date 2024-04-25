using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;

    public static SoundManager Instance => instance;

    [System.Serializable]
    public class Clips
    {
        public AudioClip clip;
        public float sfxVolume;
    }

    [Header("BGM")]
    [SerializeField] AudioClip[] bgmClips;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("SFX")]
    [SerializeField] AudioClip[] sfxClips;
    public float sfxVolume;
    [SerializeField] int channels;
    AudioSource sfxPlayer;

    [Header("Audio")]
    [SerializeField] List<Clips> listsfx;
    [SerializeField] List<Clips> listbgm;

    int channelIndex;

    public enum Bgm {Main, Lobby, Boss, GameOver, GameClear };
    public enum Sfx {Jump, Run, Walk, Block, Attack, Death, BossAttack, Click, BossShoot, MonserDie, Clear};

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        init();
    }

    private void Update()
    {
        bgmPlayer.volume = bgmVolume / 100;
    }

    private void init()
    {
        //GameObject sfxObject = new GameObject("SfxPlayer");
        //sfxObject.transform.parent = transform;
        //sfxPlayers = new AudioSource[channels];

        //for (int index = 0; index < sfxPlayers.Length; index++)
        //{
        //    sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
        //    sfxPlayers[index].playOnAwake = false;
        //    sfxPlayers[index].volume = sfxVolume;
        //}

        sfxPlayer = transform.Find("SfxPlayer").GetComponent<AudioSource>();
        bgmPlayer = transform.Find("BgmPlayer").GetComponent<AudioSource>();
    }

    public void PlaySfx(Sfx _sfx)
    {
        Clips clipData = listsfx[(int)_sfx];
        //sfxPlayers[(int)_sfx].PlayOneShot(clipData.clip, clipData.sfxVolume);
        sfxPlayer.PlayOneShot(clipData.clip, clipData.sfxVolume*sfxVolume/100);
    }

    public void PlayBgm(Bgm _bgm)
    {
        bgmPlayer.clip = bgmClips[(int)_bgm];
        bgmPlayer.Play();
    }
}
