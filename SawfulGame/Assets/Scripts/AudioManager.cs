using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Represents the game info that is stored across all scenes
    public static AudioManager instance;

    public AudioSource mainMenu;
    public AudioSource inGame;
    public AudioSource death;

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

    // Start is called before the first frame update
    void Start()
    {
        mainMenu = instance.mainMenu;
        inGame = instance.inGame;
        death = instance.death;

        PlayMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Fades out the specified object's audio by decreasing volume over time
    /// </summary>
    /// <param name="src"> The AudioSource being manipulated </param>
    /// <returns>Coroutine to decrease sources volume</returns>
    IEnumerator FadeOut(AudioSource src)
    {
        if (src.volume >= 0.9f)
        {
            src.volume = 1.0f;
        }
        //decrease volume over time
        for (float x = src.volume; x >= 0; x -= 0.3f)
        {
            src.volume = x;
            yield return new WaitForSeconds(.1f);
        }
        src.Stop();
    }

    /// <summary>
    /// Fades in the specified object's audio by increasing volume over time
    /// </summary>
    /// <param name="src"> The AudioSource being manipulated </param>
    /// <returns> Coroutine to increase sources volume </returns>
    IEnumerator FadeIn(AudioSource src)
    {
        if (!src.isPlaying)
        {
            src.volume = 0;
            src.Play();
        }

        //increase volume over time
        for (float x = src.volume; x <= 1f; x += 0.3f)
        {
            src.volume = x;
            yield return new WaitForSeconds(.1f);
        }
    }

    /// <summary>
    /// Plays the main menu music
    /// </summary>
    public void PlayMainMenu()
    {
        //the holy grail of code in this one line. Stops all current fading before switching tracks
        StopAllCoroutines();

        //Fade out in game music
        StartCoroutine(FadeOut(inGame));

        //Fade in main menu music
        StartCoroutine(FadeIn(mainMenu));
    }

    /// <summary>
    /// Plays the in-game music
    /// </summary>
    public void PlayInGame()
    {
        //the holy grail of code in this one line. Stops all current fading before switching tracks
        StopAllCoroutines();

        //Fade out main menu music
        StartCoroutine(FadeOut(mainMenu));

        //Fade in in-game music
        StartCoroutine(FadeIn(inGame));
    }

    /// <summary>
    /// Plays the death sound
    /// </summary>
    public void PlayDeath()
    {
        death.Play();
    }
}
