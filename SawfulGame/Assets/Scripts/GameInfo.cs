using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Easy,
    Normal,
    Hard
}

public enum Setting
{
    Normal,
    Special
}

/// <summary>
/// Script to keep track of various game information required between multiple scenes
/// </summary>
public class GameInfo : MonoBehaviour
{

    //Represents the game info that is stored across all scenes
    public static GameInfo instance;

    private Setting setting = Setting.Normal;
    private Difficulty mode = Difficulty.Easy;
    private KeyCode[] keys = new KeyCode[] { };
    private bool gameStart = false;
    private bool gameOver = false;
    private bool paused = false;
    private int score = 0;

    private KeyCode[] normalKeys = new KeyCode[]
    {
        KeyCode.A,
        KeyCode.B,
        KeyCode.C,
        KeyCode.D,
        KeyCode.E,
        KeyCode.F,
        KeyCode.G,
        KeyCode.H,
        KeyCode.I,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L,
        KeyCode.M,
        KeyCode.N,
        KeyCode.O,
        KeyCode.P,
        KeyCode.Q,
        KeyCode.R,
        KeyCode.S,
        KeyCode.T,
        KeyCode.U,
        KeyCode.V,
        KeyCode.W,
        KeyCode.X,
        KeyCode.Y,
        KeyCode.Z
    };

    private KeyCode[] specialKeys = new KeyCode[]
    {
        KeyCode.Exclaim, KeyCode.At, KeyCode.Hash, KeyCode.Dollar, KeyCode.Percent, KeyCode.Caret, KeyCode.Ampersand, KeyCode.Asterisk, KeyCode.LeftParen,
        KeyCode.RightParen
    };

    public Setting Setting
    {
        get { return setting; }
    }

    public Difficulty Mode
    {
        get { return mode; }
        set { mode = value; }
    }

    public KeyCode[] Keys
    {
        get { return keys; }
    }

    public bool GameStart
    {
        get { return gameStart; }
        set { gameStart = value; }
    }

    public bool GameOver
    {
        get { return gameOver; }
        set { gameOver = value; }
    }

    public bool Paused
    {
        get { return paused; }
        set { paused = value; }
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    void Awake()
    {
        //If there is not already a GameInfo object, set it to this
        if (instance == null)
        {
            //Make sure we keep running when clicking off the screen
            Application.runInBackground = true;

            //Object this is attached to will be preserved between scenes
            DontDestroyOnLoad(gameObject);

            instance = this;
        }
        else if (instance != this)
        {
            //Ensures that there are no duplicate objects being made every time the scene is loaded
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        //Default to normal
        keys = normalKeys;

        setting = instance.setting;
        mode = instance.mode;
        gameStart = instance.gameStart;
        gameOver = instance.gameOver;
        paused = instance.paused;
        score = instance.score;
    }

    // Update is called once per frame
    void Update()
    {
        //NOTE: We may want to move this logic elsewhere since it is being run on the main menu.
        //It works though because when we load the game scene, we reset gameStart back to false.
        //So, nothing is broken but if you want to make the code better you can move it somewhere better, or detect for the main menu.
        if (!gameStart)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                //Start logic for normal
                if (setting == Setting.Normal)
                {
                    if (Input.GetKeyDown(keys[i]))
                    {
                        StartGame();
                        break;
                    }
                }

                //Start logic for special
                else if (setting == Setting.Special)
                {
                    //Implement logic for detecting the special keys
                    //Make sure you call StartGame()
                }
            }
        }

        if(gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            ResetGame();
            ReloadMainMenu();
        }

        ExitGame();
    }

    /// <summary>
    /// Temporary method to reload the scene.
    /// We probably won't use this for the final build.
    /// </summary>
    public void ReloadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Temporary method to start the game.
    /// We probably won't use this for the final build.
    /// </summary>
    public void StartGame()
    {
        gameStart = true;
    }

    public void ResetGame()
    {
        gameStart = false;
        paused = false;
        gameOver = false;
        score = 0;
    }

    public void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// To be used within the UI to switch between normal and special characters.
    /// </summary>
    /// <param name="settingValue">Integer representation of the enum to set.</param>
    public void SetSetting(int settingValue)
    {
        setting = (Setting)settingValue;

        if (setting == Setting.Special)
        {
            keys = specialKeys;
        }
        else
        {
            keys = normalKeys;
        }
    }
}
