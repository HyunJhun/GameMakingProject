using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    
    public static DataManager dataManagerInstance;

    private float bgmVolumeValue = 1f;
    private float sfxVolumeValue = 1f;
    public float hpData { get; set; } = 100f;
    public float mpData { get; set; } = 50f;
    public float staminaData { get; set; } = 150f;
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
    //void Update()
    //{
    //    if(SceneManager.GetActiveScene().name == "Boss Scene")
    //    {
    //        if(!isBossScene)
    //        {
    //            isBossScene = true;
    //            GameObject.Find("Player").GetComponent<Status>().SetHudStatus(hpData, mpData, staminaData);
    //            return;             
    //        }
    //    }
    //}

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
    public void InitializeStatusValues()
    {
        hpData = 100f;
        mpData = 50f;
        staminaData = 150f;
    }

    public float GetBgmValue() { return bgmVolumeValue; }
    public float GetSfxValue() { return sfxVolumeValue; }

}
