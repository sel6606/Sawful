using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGame : MonoBehaviour
{
    public GameObject player;
    public GameObject spawn;
    public float spawnInterval;

    private Vector3 cameraCenter;
    private float cameraUpperBounds;

    // Start is called before the first frame update
    void Start()
    {
        cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, Camera.main.nearClipPlane));
        cameraUpperBounds = cameraCenter.y + Camera.main.orthographicSize;

        SetSpawnPos();
        SpawnFirstPlatforms();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Changes the spawn position based on the interval between the platforms.
    /// </summary>
    private void SetSpawnPos()
    {
        float offsetY = cameraUpperBounds + spawnInterval;
        spawn.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y + offsetY, spawn.transform.position.z);
    }

    /// <summary>
    /// Spawns the first set of platforms to start playing the game.
    /// </summary>
    private void SpawnFirstPlatforms()
    {
        Vector3 pos = cameraCenter; 
        Spawning spawner = gameObject.GetComponent<Spawning>();

        //Spawn in the first platform row
        spawner.SpawnRow(pos);

        //Setup the row to be the starting row
        GameObject row = spawner.SpawnedPlatforms[0];
        PlatformRow rowScript = row.GetComponent<PlatformRow>();
        rowScript.IsActive = true;
        rowScript.HasPlayer = true;
        rowScript.SendPlatforms();

        //Save the extents to spawn further platforms
        float extentsY = rowScript.GetExtentsY();

        //Spawn the player on the safe platfrom
        for (int i = 0; i < row.transform.childCount; i++)
        {
            GameObject child = row.transform.GetChild(i).gameObject;

            if (child.CompareTag("Safe"))
            {
                //Set the target since Start() doesn't run in Platform when child is instantiated
                child.GetComponent<Platform>().SetTargetPlayerPosition();
                child.GetComponent<Platform>().DisplayCombination();

                //Spawn the player
                Vector3 playerPos = child.GetComponent<Platform>().Target.transform.position;
                Instantiate(player, playerPos, Quaternion.identity);

                break;
            }
        }

        Debug.Log("Spawned first row and player");

        //Keep spawning rows until conditions are met to spawn a row off screen
        while (pos.y + extentsY + spawnInterval < cameraUpperBounds)
        {
            //Update the position
            pos = new Vector3(pos.x, pos.y + extentsY + spawnInterval, pos.z);

            //Spawn the next row
            spawner.SpawnRow(pos);

            //Setup the row to be on screen
            GameObject nextRow = spawner.SpawnedPlatforms[spawner.SpawnedPlatforms.Count - 1];
            PlatformRow nextRowScript = nextRow.GetComponent<PlatformRow>();
            nextRowScript.IsActive = true;
            nextRowScript.SendPlatforms();

            Debug.Log("Spawned next rows on screen");
        }

        //Update the position
        pos = new Vector3(pos.x, pos.y + extentsY + spawnInterval, pos.z);

        //Spawn the last row off screen
        spawner.SpawnRow(pos);

        Debug.Log("Spawned last row off screen");
    }
}
