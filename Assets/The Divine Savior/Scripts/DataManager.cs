using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    
    public static DataManager dataManagerInstance;

    private bool isBossScene = false;

    private float bgmVolumeValue;
    private float sfxVolumeValue;
    public float hpData { get; set; }
    public float mpData { get; set; }
    public float staminaData { get; set; }
    void Start()
    {
        if (dataManagerInstance == null)
            dataManagerInstance = this;
        else
            Destroy(gameObject);

        var obj = FindObjectsOfType<DataManager>();
        if (obj.Length == 1)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Boss Scene")
        {
            if(!isBossScene)
            {
                isBossScene = true;
                GameObject.Find("Player").GetComponent<Status>().SetHudStatus(hpData, mpData, staminaData);
                return;             
            }
        }
    }

    public void SetVolumeValues(float bgmValue,float sfxValue)
    {
        bgmVolumeValue = bgmValue;
        sfxVolumeValue = sfxValue;
    }
    public void SetHudValues(float hp,float mp,float stamina)
    {
        hpData = hp;
        mpData = mp;
        staminaData = stamina;
    }

    public float GetBgmValue() { return bgmVolumeValue; }
    public float GetSfxValue() { return sfxVolumeValue; }

}
