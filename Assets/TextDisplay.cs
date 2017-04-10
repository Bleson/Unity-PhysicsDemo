using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TextDisplay : MonoBehaviour {

    Text txt;
    int textLength = 11;
    string TXT_TOP      = "==========";
    string TXT_SIDE     = "||";

    void Awake()
    {
        txt = GetComponent<Text>();
    }

    internal void UpdateText(string textToShow, int offset)
    {
        string newText = "";
        newText += TXT_TOP + "\n" + TXT_SIDE;

        int spaces = textLength - textToShow.Length + offset;
        for (int i = 0; i < spaces / 2; i++)
        {
            newText += " ";
        }
        if (spaces % 2 != 0)
        {
            newText += " ";
        }

        newText += textToShow;

        for (int i = 0; i < spaces / 2; i++)
        {
            newText += " ";
        }

        newText += TXT_SIDE + "\n" + TXT_TOP;

        txt.text = newText;
    }

    internal void UpdateTextSimple(string textToShow)
    {
        txt.text = textToShow;
    }
}
