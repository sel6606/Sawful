using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script attached to each individual platform.
/// </summary>
public class Platform : MonoBehaviour
{
    public GameObject platform;
    public GameObject target;
    public GameObject text;
    public Color32 highlightColor;
    public Material fadedMat;
    public Material visibleMat;

    private bool isSafe;
    private List<KeyCode> combination;

    public bool IsSafe
    {
        get { return isSafe; }
        set { isSafe = value; }
    }

    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }

    public List<KeyCode> Combination
    {
        get { return combination; }
        set { combination = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        DisplayCombination();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Displays the combination on the platform
    /// </summary>
    private void DisplayCombination()
    {
        string combo = "";

        for (int i = 0; i < combination.Count; i++)
        {
            combo += ConvertKey(combination[i]);
        }

        text.GetComponent<TextMeshPro>().text = combo;
    }

    /// <summary>
    /// Highlights all of the characters in the combination up to the index.
    /// To remove the highlights pass an index less than 0.
    /// </summary>
    /// <param name="comboIndex">Index of the character to stop at</param>
    public void HighlightCharacter(int comboIndex)
    {
        //Special Case: Reset color back to normal
        if (comboIndex < 0)
        {
            DisplayCombination();
            return;
        }

        string combo = "<color=#" + ColorUtility.ToHtmlStringRGBA(highlightColor) + ">";

        for (int i = 0; i < combination.Count; i++)
        {
            //Stop highlighting after the index
            if (i == comboIndex + 1)
            {
                combo += "</color>";
            }

            combo += ConvertKey(combination[i]);
        }

        text.GetComponent<TextMeshPro>().text = combo;
    }

    /// <summary>
    /// Makes the combination more visible.
    /// </summary>
    public void MakeVisible()
    {
        text.GetComponent<TextMeshPro>().fontMaterial = visibleMat;
    }

    /// <summary>
    /// Makes the combination faded.
    /// </summary>
    public void MakeFaded()
    {
        text.GetComponent<TextMeshPro>().fontMaterial = fadedMat;
    }

    /// <summary>
    /// Converts keycodes to their string representation
    /// </summary>
    /// <param name="key">keycode to convert</param>
    /// <returns>the string representation of the keycode</returns>
    private string ConvertKey(KeyCode key)
    {
        string conversion = key.ToString();

        //Special Keys
        switch (key)
        {
            case KeyCode.Exclaim:
                conversion = "!";
                break;
            case KeyCode.At:
                conversion = "@";
                break;
            case KeyCode.Hash:
                conversion = "#";
                break;
            case KeyCode.Dollar:
                conversion = "$";
                break;
            case KeyCode.Percent:
                conversion = "%";
                break;
            case KeyCode.Caret:
                conversion = "^";
                break;
            case KeyCode.Ampersand:
                conversion = "&";
                break;
            case KeyCode.Asterisk:
                conversion = "*";
                break;
            case KeyCode.LeftParen:
                conversion = "(";
                break;
            case KeyCode.RightParen:
                conversion = ")";
                break;
        }

        return conversion;
    }
}
