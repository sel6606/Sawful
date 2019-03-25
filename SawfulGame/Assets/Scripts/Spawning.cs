using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    public GameObject platforms;

    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        timer = GameInfo.instance.SpawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Spawns platforms at the given interval
    /// </summary>
    private void SpawnPlatforms()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = GameInfo.instance.SpawnRate;

            //Generate letter or combination

            //Generate 3 platforms

            //Assign each platform a letter or combination
            
        }
    }

    /// <summary>
    /// Generates a combination associated with each platform
    /// </summary>
    private void GenerateCombination()
    {

    }
}
