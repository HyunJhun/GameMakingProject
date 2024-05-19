using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour
{
    public static ButtonManager buttonManagerInstance;

    [Header("UI")]
    [SerializeField] private GameObject settingUI;

    private void Awake()
    {
        if (buttonManagerInstance == null)
            buttonManagerInstance = this;
        else
            Destroy(gameObject);

        var obj = FindObjectsOfType<ButtonManager>();
        if (obj.Length == 1)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
    }

    private void Start()
    {
    }
    public void StartEvent()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void SettingEvent()
    {
        settingUI.SetActive(true);
    }
    public void CloseSetting()
    {
        settingUI.SetActive(false);
    }
}
