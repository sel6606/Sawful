using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class HighScores
{
    public static HighScores current;

    public List<string> names;
    public List<int> scores;


    public HighScores()
    {
        names = new List<string>();
        scores = new List<int>();
        names.AddRange(new string[] { "OOF", "YEE", "WHY", "AAA", "GOD", "ATH", "SLE", "NCA", "ABE", "MKA" });
        scores.AddRange(new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 });
    }
    public HighScores(GameObject[] scoreElements)
    {
        names = new List<string>();
        scores = new List<int>();

        for(int i = 0; i < 10; i++)
        {
            names.Add(scoreElements[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            scores.Add(int.Parse(scoreElements[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text));
        }
    }
}
