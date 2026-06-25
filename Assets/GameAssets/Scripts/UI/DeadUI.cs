using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DeadUI : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("UI")]
    [SerializeField] private Image background;
    [SerializeField] private Image textBox;
    [SerializeField] private TMP_Text text;

    private bool isFadeInComplete = false;
    void Start()
    {
        StartCoroutine("AlphaFadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        if(isFadeInComplete)
        {
            isFadeInComplete = false;
            Destroy(gameObject);
        }
    }

    IEnumerator AlphaFadeIn()
    {
        Color backgroundColor = background.color;
        Color textBoxColor = textBox.color;
        Color textColor = text.color;


        while (backgroundColor.a < 0.9f && textBoxColor.a < 0.95f && textColor.a < 0.99f)
        {
            backgroundColor.a = Mathf.Lerp(backgroundColor.a, 0.85f, Time.deltaTime);
            textBoxColor.a = Mathf.Lerp(textBoxColor.a, 0.93f, Time.deltaTime);
            textColor.a = Mathf.Lerp(textColor.a, 1f, Time.deltaTime);

            background.color = backgroundColor;
            textBox.color = textBoxColor;
            text.color = textColor;
            yield return null;
        }

        isFadeInComplete = true;
    }

    private void OnDestroy()
    {
        DataManager.dataManagerInstance.InitializeStatusValues();
        SceneManager.LoadScene("MainScene");
    }
}
