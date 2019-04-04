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
            combo += combination[i].ToString();
        }

        text.GetComponent<TextMeshPro>().text = combo.ToLower();
    }
}
