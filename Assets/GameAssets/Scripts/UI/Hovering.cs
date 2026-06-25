using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Hovering : MonoBehaviour
{
    private TMP_Text myTxt;
    [SerializeField]private Color32 originalColor;

    [SerializeField] private Color32 hoveredColor;
    [SerializeField] private Color32 clickedColor;

    // Start is called before the first frame update
    void Start()
    {
        myTxt = GetComponent<TMP_Text>();
        originalColor = myTxt.color;
    }

    public void changeTextColorWhenMouseEnter()
    {
        myTxt.color = hoveredColor;
        myTxt.color = new Color(myTxt.color.r, myTxt.color.g, myTxt.color.b, 1);
    }

    public void changeTextColorWhenMouseExit()
    {
        myTxt.color = originalColor;
        myTxt.color = new Color(myTxt.color.r, myTxt.color.g, myTxt.color.b, 1);
    }

    public void changeTextColorWhenMouseClicked()
    {
        // Implement color change when clicked if needed
    }
}