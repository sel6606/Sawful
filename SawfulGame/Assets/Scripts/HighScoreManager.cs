using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public GameObject[] scorePanels;

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
        
    }

    public void LoadHighScores()
    {
        if (!SaveLoad.loaded)
        {
            SaveLoad.Load();
        }

        scoreValues = SaveLoad.highScores.scores;
        nameValues = SaveLoad.highScores.names;
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
