using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class menuManager : MonoBehaviour
{
    public int currentScene; //0 - main menu, 1 - main scene
    public GameObject howToPanel;
    public GameObject gameOverPanel;
    public float score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreText2;
    public bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentScene == 1)
        {
            //score++; [DEBUG LEFTOVER]
            score = GameInfo.instance.Score;
            scoreText2.SetText("Score: {0}", score);
        }

        //DEBUG (Remove when UI is implemented)
        else if (currentScene == 0)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                LoadEasy();
            }
            else if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                LoadNormal();
            }
            else if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                LoadHard();
            }
            else if (Input.GetKeyDown(KeyCode.Backslash))
            {
                LoadInsane();
            }
        }

        if(GameInfo.instance.GameOver && !gameOver)
        {
            StartCoroutine(toggleGameOver());
            gameOver = true;
        }

    }

    public void LoadInsane()
    {
        GameInfo.instance.SetDifficulty(Difficulty.Insane);
        loadGame();
    }

    public void LoadHard()
    {
        GameInfo.instance.SetDifficulty(Difficulty.Hard);
        loadGame();
    }

    public void LoadNormal()
    {
        GameInfo.instance.SetDifficulty(Difficulty.Normal);
        loadGame();
    }

    public void LoadEasy()
    {
        GameInfo.instance.SetDifficulty(Difficulty.Easy);
        loadGame();
    }

    public void loadGame()
    {
        GameInfo.instance.ResetGame();
        SceneManager.LoadScene("TestSpawningScene", LoadSceneMode.Single);
    }

    public void loadMenu()
    {
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

        scoreText.SetText("Final Score: {0}", score);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
