using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SettingUI : MonoBehaviour
{
    private SoundMixer soundMixer;
    // Start is called before the first frame update
    void Start()
    {
        soundMixer = GetComponent<SoundMixer>();
        soundMixer.SetSliderValue(DataManager.dataManagerInstance.GetBgmValue(), DataManager.dataManagerInstance.GetSfxValue());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Close() 
    {
        DataManager.dataManagerInstance.SetVolumeValues(soundMixer.bgmSlider.value, soundMixer.sfxSlider.value);

        if (SceneManager.GetActiveScene().name != "EnterScene")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        Destroy(gameObject); 
    }
     
    public void Exit()
    {
        Application.Quit();
    }
}
