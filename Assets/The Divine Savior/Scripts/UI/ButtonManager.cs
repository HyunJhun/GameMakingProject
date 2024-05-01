using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour
{
    public static ButtonManager buttonManagerInstance;

    private void Awake()
    {
        if (buttonManagerInstance == null)
            buttonManagerInstance = this;
        else
            Destroy(gameObject);
    }


    public void StartEvent()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void SettingEvent()
    {

    }
}
