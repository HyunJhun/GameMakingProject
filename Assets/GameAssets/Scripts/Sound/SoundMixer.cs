using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundMixer : MonoBehaviour
{
    // 오디오 믹서
    public AudioMixer audioMixer;

    // 슬라이더
    public Slider bgmSlider;
    public Slider sfxSlider;
    // Start is called before the first frame update

    private void Start()
    {
        sfxSlider.value = sfxSlider.maxValue;
        bgmSlider.value = bgmSlider.maxValue;
        Debug.Log($"초기값 : {sfxSlider.value} , 최대값 : {sfxSlider.maxValue}");
    }

    public void SetSliderValue(float bgmVar, float sfxVar)
    {
        Debug.Log($"수치 : {bgmVar}");
        bgmSlider.value = bgmVar;
        sfxSlider.value = sfxVar;
    }

    public void setBgmVolume()
    {
        float volume = bgmSlider.value > 0.0001f
        ? Mathf.Log10(bgmSlider.value) * 20
        : -80f; // Mixer의 최소값(사실상 무음)

        audioMixer.SetFloat("BGM", volume);
    }
    public void setSfxVolume()
    {
        float volume = sfxSlider.value > 0.0001f
        ? Mathf.Log10(sfxSlider.value) * 20
        : -80f; // Mixer의 최소값(사실상 무음)

        audioMixer.SetFloat("SFX", volume);
    }
}
