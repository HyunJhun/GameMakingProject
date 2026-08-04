using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
public class ButtonManager : MonoBehaviour
{
    public static ButtonManager buttonManagerInstance;

    [Header("UI")]
    [SerializeField] private GameObject settingUIPrefab;
    private void Awake()
    {
        if (buttonManagerInstance == null)
            buttonManagerInstance = this;
        else
            Destroy(gameObject);


        DontDestroyOnLoad(gameObject);
    }
    public void StartEvent()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void SettingEvent()
    {
        var obj = FindObjectsOfType<SettingUI>();
        if (obj == null) return;

        if(obj.Length == 1) Destroy(obj[0].gameObject);

        Instantiate(settingUIPrefab, GameObject.Find("Canvas_UI").transform);
    }
    public void ExitEvent()
    {
        Application.Quit();
    }

    public void EndingEvent()
    {
        SceneManager.LoadScene("EndingScene");
    }
}
