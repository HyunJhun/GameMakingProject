using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManagerInstacne;
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixerGroup bgmOutput;
    [SerializeField] private AudioMixerGroup sfxOutput;

    [Header("BGM")]
    public List<AudioClip> bgmSounds = new List<AudioClip>();
    private AudioSource bgmPlayer;
    public float bgmVolume;
    [Header("SFX")]
    public List<AudioClip> enemySounds = new List<AudioClip>();
    public List<AudioClip> bossSounds = new List<AudioClip>();
    public List<AudioClip> playerSounds = new List<AudioClip>();
    public List<AudioClip> otherSounds = new List<AudioClip>();

    public int channels;
    public float sfxVolume;
    private AudioSource[] sfxPlayers;
    private int channelIndex;

    public enum SFX_Player { Walk, Sprint, Dodge, Attack, Defense ,Hit,Skill,Dead = 8}
    public enum SFX_Enemy {Hit, Attack, Dead = 3}
    public enum SFX_Boss { Walk, Hit, Dead, Fly, Attack = 5, Bomb = 9}
    public enum SFX { Click,Die}
    public enum BGM { Title,Main,Boss}

    private void Awake()
    {
        if (soundManagerInstacne == null)
            soundManagerInstacne = this;
        else
        {
            Destroy(gameObject);
        }

        var obj = FindObjectsOfType<SoundManager>();
        if (obj.Length == 1)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);

        initialize();
    }

    private void initialize()
    {
        // BGM
        GameObject bgmObj = new GameObject("BgmPlayer");
        bgmObj.transform.parent = transform;
        bgmPlayer = bgmObj.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.outputAudioMixerGroup = bgmOutput;
        // SFX
        GameObject sfxObj = new GameObject("SfxPlayer");
        sfxObj.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int idx = 0; idx < sfxPlayers.Length; idx++)
        {
            sfxPlayers[idx] = sfxObj.AddComponent<AudioSource>();
            sfxPlayers[idx].playOnAwake = false;
            sfxPlayers[idx].volume = sfxVolume;
            sfxPlayers[idx].outputAudioMixerGroup = sfxOutput;
        }

        PlayBgm(BGM.Title);
    }

    public void PlayBgm(BGM bgm)
    {        
        bgmPlayer.clip = bgmSounds[(int)bgm];
        bgmPlayer.Play();
    }

    public void PlaySfx(SFX_Enemy sfx)
    {
        for (int idx = 0; idx < sfxPlayers.Length; idx++)
        {
            int loopIndex = (idx + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = enemySounds[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }

    }
    public void PlaySfx(SFX_Boss sfx)
    {
        for (int idx = 0; idx < sfxPlayers.Length; idx++)
        {
            int loopIndex = (idx + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = bossSounds[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }

    }
    public void PlaySfx(SFX_Player sfx,bool loop)
    {
        for (int idx = 0; idx < sfxPlayers.Length; idx++)
        {
            int loopIndex = (idx + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = playerSounds[(int)sfx];
            if (loop) sfxPlayers[loopIndex].loop = true;
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
    public void PlaySfx(SFX sfx)
    {
        for (int idx = 0; idx < sfxPlayers.Length; idx++)
        {
            int loopIndex = (idx + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = enemySounds[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }

    }
    public void StopSfx(SFX_Player sfx)
    {
        for (int idx = 0; idx < sfxPlayers.Length; idx++)
        {
            int loopIndex = (idx + channelIndex) % sfxPlayers.Length;

            if (!sfxPlayers[loopIndex].isPlaying)
                continue;

            if(sfxPlayers[loopIndex].clip == playerSounds[(int)sfx])
            {
                channelIndex = loopIndex;
                if (sfxPlayers[loopIndex].loop) sfxPlayers[loopIndex].loop = false;
                sfxPlayers[loopIndex].Stop();
                break;
            }
        }
    }

}
