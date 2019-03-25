using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to keep track of various game information required between multiple scenes
/// </summary>
public class GameInfo : MonoBehaviour
{

    //Represents the game info that is stored across all scenes
    public static GameInfo instance;

    public float moveSpeed;
    public float spawnRate;
    public int maxCombination;

    private bool gameStart = false;
    private bool gameOver = false;
    private bool paused = false;

    private float initialMoveSpeed;
    private float initialSpawnRate;
    private int initialCombination;

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

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public float SpawnRate
    {
        get { return spawnRate; }
        set { spawnRate = value; }
    }

    public int MaxCombination
    {
        get { return maxCombination; }
        set { maxCombination = value; }
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
        gameStart = instance.gameStart;
        gameOver = instance.gameOver;
        paused = instance.paused;
        moveSpeed = instance.moveSpeed;
        spawnRate = instance.spawnRate;
        maxCombination = instance.maxCombination;
        initialMoveSpeed = moveSpeed;
        initialSpawnRate = spawnRate;
        initialCombination = maxCombination;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Temporary method to reload the scene.
    /// We probably won't use this for the final build.
    /// </summary>
    public void ReloadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ResetDistance()
    {
        moveSpeed = initialMoveSpeed;
        spawnRate = initialSpawnRate;
    }
}
