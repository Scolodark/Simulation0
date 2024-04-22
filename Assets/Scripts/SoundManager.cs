using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;

    public static SoundManager Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }

            return instance;
        }
    }

    [Header("SFX")]
    [SerializeField] AudioClip[] sfxClips;
    [SerializeField] float sfxVolume;
    [SerializeField] int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

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

    private void init()
    {
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlaySfx(Sfx _sfx)
    {
        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)_sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }

        
    }
}
