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
        names.AddRange(new string[] { "OOF", "YEE", "WHY", "AAA", "GOD", "ATH", "SLE", "NCA", "ABE", "MKA" });
        scores.AddRange(new int[] { 99, 74, 68, 47, 42, 32, 29, 29, 24, 20 });
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
