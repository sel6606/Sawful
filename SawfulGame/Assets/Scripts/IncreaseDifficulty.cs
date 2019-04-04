﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to gradually increase the difficulty of the game
/// </summary>
public class IncreaseDifficulty : MonoBehaviour
{
    public int maxCombo;
    public float maxSpeed;
    public int maxPlatforms;
    public int increaseInterval; //# of times the countdown will complete before a difficulty increase has fully completed (has to be at least 1)
    public float countdown;

    private Spawning spawner;
    private PrefabVariation prefabVariation;
    private float timer;
    private float speedIncrementor;
    private int counter;
    private int comboIncrementorInterval;

    // Start is called before the first frame update
    void Start()
    {
        spawner = gameObject.GetComponent<Spawning>();
        prefabVariation = gameObject.GetComponent<PrefabVariation>();
        timer = countdown;

        //The 1 line if-statements are used to make sure things don't go wrong
        //if someone accidently put the max value less than the starting value
        int totalIncreases = maxPlatforms <= prefabVariation.NumPlatforms ? increaseInterval : increaseInterval * (maxPlatforms - prefabVariation.NumPlatforms + 1);
        totalIncreases = maxCombo <= spawner.NumCombo ? totalIncreases : totalIncreases * (maxCombo - spawner.NumCombo);
        speedIncrementor = maxSpeed <= spawner.MoveSpeed ? 0 : (maxSpeed - spawner.MoveSpeed) / totalIncreases;

        //Override the interval to have increasing platforms increase at the correct intervals with speed/combo
        increaseInterval = maxPlatforms <= prefabVariation.NumPlatforms ? increaseInterval : totalIncreases / (maxPlatforms - prefabVariation.NumPlatforms);

        counter = totalIncreases;
        comboIncrementorInterval = totalIncreases / (maxCombo - spawner.NumCombo);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInfo.instance.GameStart && !GameInfo.instance.Paused && !GameInfo.instance.GameOver)
        {
            if (counter > 0)
            {
                IncrementDifficulty();
            }
        }
    }

    /// <summary>
    /// Increases the difficulty every few seconds (increases speed/combo/platforms)
    /// </summary>
    private void IncrementDifficulty()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = countdown;

            counter--;

            spawner.MoveSpeed += speedIncrementor;
            spawner.MoveSpeed = Mathf.Clamp(spawner.MoveSpeed, 0, maxSpeed);

            //Increase # of platforms evenly throughout the game and we're not at the max
            if (counter % increaseInterval == 0 && prefabVariation.NumPlatforms < maxPlatforms)
            {
                prefabVariation.NumPlatforms++;
            }

            //Special Case: Not incrementing combo
            if (comboIncrementorInterval == 0)
                return;

            //Increase the combo evenly throughout the game
            else if (counter % comboIncrementorInterval == 0 && spawner.NumCombo < maxCombo)
            {
                spawner.NumCombo++;
            }
        }
    }
}
