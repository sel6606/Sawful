using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class menuManager : MonoBehaviour
{
    public int currentScene; //0 - main menu, 1 - main scene
    public GameObject howToPanel;
    public GameObject gameOverPanel;
    public float score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreText2;
    public bool gameOver = false;

    public bool normSelected = true;
    public bool specSelected = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Main Menu
        if (currentScene == 0)
        {
            //Make sure normal is selected when coming from the game scene
            if (GameInfo.instance.Setting == Setting.Normal && !normSelected)
            {
                normalCharacter();
            }

            //Make sure special is selected when coming from the game scene
            else if (GameInfo.instance.Setting == Setting.Special && !specSelected)
            {
                specialCharacter();
            }
        }

        //In Game
        if (currentScene == 1)
        {
            //score++; [DEBUG LEFTOVER]
            score = GameInfo.instance.Score;
            scoreText2.SetText("Score: {0}", score);

            //Show brief instructions if this is the first time playing
            if (GameInfo.instance.FirstTime)
            {
                GameInfo.instance.FirstTime = false;

                toggleHowTo();
            }

            //Get rid of instructions once player started playing
            if (howToPanel.activeInHierarchy && GameInfo.instance.GameStart)
            {
                toggleHowTo();
            }
        }

        if(GameInfo.instance.GameOver && !gameOver)
        {
            StartCoroutine(toggleGameOver());
            gameOver = true;
        }

    }

    public void loadEasy()
    {
        GameInfo.instance.Mode = Difficulty.Easy;
        loadGame();
    }

    public void loadNormal()
    {
        GameInfo.instance.Mode = Difficulty.Normal;
        loadGame();
    }

    public void loadHard()
    {
        GameInfo.instance.Mode = Difficulty.Hard;
        loadGame();
    }

    public void loadGame()
    {
        AudioManager.instance.PlayInGame();

        GameInfo.instance.ResetGame();
        SceneManager.LoadScene("TestSpawningScene", LoadSceneMode.Single);
    }

    public void loadMenu()
    {
        AudioManager.instance.PlayMainMenu();

        GameInfo.instance.ResetGame();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void toggleHowTo()
    {
        if (!howToPanel.activeInHierarchy)
        {
            howToPanel.SetActive(true);
        }
        else
        {
            howToPanel.SetActive(false);
        }
    }

    public IEnumerator toggleGameOver()
    {
        yield return new WaitForSeconds(3);

        if (!gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(false);
        }

        scoreText.SetText("Final Score\r\n{0}", score);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void normalCharacter()
    {
        //highlight clicked button if not already selected
        if (!normSelected)
        {
            //Enables normal characters
            GameInfo.instance.SetSetting(Setting.Normal);

            //highlight normal
            GameObject normChar = GameObject.Find("normalCharacterButton");
            ColorBlock normCol = normChar.GetComponent<Button>().colors;
            Color newCol = new Vector4(0.5943396f, 0.0f, 0.0f, 1.0f);
            normCol.normalColor = newCol;
            normCol.highlightedColor = normCol.normalColor;
            normChar.GetComponent<Button>().colors = normCol;

            //de-highlight special
            GameObject specChar = GameObject.Find("specialCharacterButton");
            ColorBlock specCol = specChar.GetComponent<Button>().colors;
            Color newNormCol = new Vector4(0.8509f, 0.8509f, 0.8509f, 1.0f);
            Color newHighCol = new Vector4(0.2924f, 0.2924f, 0.2924f, 1.0f);
            specCol.normalColor = newNormCol;
            specCol.highlightedColor = newHighCol;
            specChar.GetComponent<Button>().colors = specCol;

            normSelected = true;
            specSelected = false;
        }
    }

    public void specialCharacter()
    {
        //highlight clicked button if not already selected
        if (!specSelected)
        {
            //Enables special characters
            GameInfo.instance.SetSetting(Setting.Special);

            //highlight special
            GameObject specChar = GameObject.Find("specialCharacterButton");
            ColorBlock specCol = specChar.GetComponent<Button>().colors;
            Color newCol = new Vector4(0.5943396f, 0.0f, 0.0f, 1.0f);
            specCol.normalColor = newCol;
            specCol.highlightedColor = specCol.normalColor;
            specChar.GetComponent<Button>().colors = specCol;

            //de-highlight normal
            GameObject normChar = GameObject.Find("normalCharacterButton");
            ColorBlock normCol = normChar.GetComponent<Button>().colors;
            Color newNormCol = new Vector4(0.8509f, 0.8509f, 0.8509f, 1.0f);
            Color newHighCol = new Vector4(0.2924f, 0.2924f, 0.2924f, 1.0f);
            normCol.normalColor = newNormCol;
            normCol.highlightedColor = newHighCol;
            normChar.GetComponent<Button>().colors = normCol;

            specSelected = true;
            normSelected = false;
        }
     
    }
}
