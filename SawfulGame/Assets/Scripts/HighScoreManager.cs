using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    public GameObject[] scorePanels;
    public TMP_InputField input1;
    public TMP_InputField input2;
    public TMP_InputField input3;
    public Button b;

    private List<GameObject> namePanels;
    private List<GameObject> valuePanels;

    private List<int> scoreValues;
    private List<string> nameValues;

    private bool hasHighScore = false;
    private int highScoreIndex = -1;

    public bool HasHighScore
    {
        get { return hasHighScore; }
    }


    private void Awake()
    {
        namePanels = new List<GameObject>();
        valuePanels = new List<GameObject>();

        foreach (GameObject g in scorePanels)
        {
            namePanels.Add(g.transform.GetChild(0).gameObject);
            valuePanels.Add(g.transform.GetChild(1).gameObject);
        }

        LoadHighScores();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(input1.text.Length == 1 && input2.text.Length == 1 && input3.text.Length == 1)
        {
            b.interactable = true;
        }
    }

    public void LoadHighScores()
    {
        if (!SaveLoad.loaded)
        {
            SaveLoad.Load();
        }

        scoreValues = SaveLoad.highScores[(int)GameInfo.instance.Mode].scores;
        nameValues = SaveLoad.highScores[(int)GameInfo.instance.Mode].names;
    }

    public void UpdateHighScores()
    {
        int newScore = GameInfo.instance.Score;

        for(int i = 0; i < scoreValues.Count; i++)
        { 
            if(newScore >= scoreValues[i])
            {
                hasHighScore = true;
                highScoreIndex = i;
                break;
            }
        }
    }

    public void InsertHighScore()
    {
        if(hasHighScore)
        {
            string name = input1.text + input2.text + input3.text;
            name = name.ToUpper();

            scoreValues.Insert(highScoreIndex, GameInfo.instance.Score);
            nameValues.Insert(highScoreIndex, name);

            scoreValues.RemoveAt(10);
            nameValues.RemoveAt(10);

            DisplayHighScores();

            SaveLoad.highScores[(int)GameInfo.instance.Mode].names = nameValues;
            SaveLoad.highScores[(int)GameInfo.instance.Mode].scores = scoreValues;

            SaveLoad.Save();
        }
    }
    public void DisplayHighScores()
    {
        for(int i = 0; i < 10; i++)
        {
            namePanels[i].GetComponent<TextMeshProUGUI>().text = nameValues[i];
            valuePanels[i].GetComponent<TextMeshProUGUI>().text = scoreValues[i] + "";
        }
    }
}
