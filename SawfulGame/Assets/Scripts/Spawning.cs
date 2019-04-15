using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spawning : MonoBehaviour
{
    public int numCombo;
    public float spawnInterval;
    public float moveSpeed;
    public GameObject spawn;
    public GameObject playerPrefab;

    private PrefabVariation prefabVariation;
    private GameObject playerInstance;
    private float cameraUpperBounds;
    private float cameraLowerBounds;
    private float platformExtentsY;
    private float playerExtentsY;

    private KeyCode[] keys;

    private KeyCode[] normalKeys = new KeyCode[]
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N,
        KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.W, KeyCode.Y
    };

    private KeyCode[] specialKeys = new KeyCode[] 
    {
        KeyCode.Exclaim, KeyCode.At, KeyCode.Hash, KeyCode.Dollar, KeyCode.Percent, KeyCode.Ampersand, KeyCode.Asterisk, KeyCode.LeftParen, KeyCode.RightParen
    };

    #region Properties
    public int NumCombo
    {
        get { return numCombo; }
        set { numCombo = value; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public GameObject PlayerInstance
    {
        get { return playerInstance; }
        set { playerInstance = value; }
    }

    public float CameraUpperBounds
    {
        get { return cameraUpperBounds; }
        set { cameraUpperBounds = value; }
    }

    public float CameraLowerBounds
    {
        get { return cameraLowerBounds; }
        set { cameraLowerBounds = value; }
    }

    public float PlatformExtentsY
    {
        get { return platformExtentsY; }
        set { platformExtentsY = value; }
    }

    public float PlayerExtentsY
    {
        get { return playerExtentsY; }
        set { playerExtentsY = value; }
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        if (GameInfo.instance.Easy || GameInfo.instance.Normal)
        {
            keys = normalKeys;
        }
        else if (GameInfo.instance.Hard)
        {
            keys = specialKeys;
        }

        prefabVariation = gameObject.GetComponent<PrefabVariation>();
        GetBoundsAndExtents();
        SetupGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Gets the boundsY of the camera as well as the extentsY for a row.
    /// </summary>
    private void GetBoundsAndExtents()
    {
        //Get camera bounds
        cameraUpperBounds = Camera.main.transform.position.y + Camera.main.orthographicSize;
        cameraLowerBounds = Camera.main.transform.position.y - Camera.main.orthographicSize;

        //Get the extents for a row
        Platform platformScript = prefabVariation.safePrefab.GetComponent<Platform>();
        Bounds platformBounds = platformScript.platform.GetComponent<SpriteRenderer>().bounds;
        platformExtentsY = platformBounds.extents.y;

        //Get the extents for the player
        playerExtentsY = playerPrefab.GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    /// <summary>
    /// Spawns a row of platforms at the spawn point.
    /// </summary>
    public GameObject SpawnRow()
    {
        return SpawnRow(spawn.transform.position);
    }

    /// <summary>
    /// Spawns a row of platforms at the given position.
    /// </summary>
    /// <param name="pos">Position to spawn at</param>
    /// <returns>The spawned row</returns>
    public GameObject SpawnRow(Vector3 pos)
    {
        GameObject row = prefabVariation.CreateRow();
        row.transform.parent = transform;
        row.transform.position = pos;

        PlatformRow rowScript = row.GetComponent<PlatformRow>();
        rowScript.Platforms = new List<GameObject>();

        //Safe combination is always the last one in the list
        List<List<KeyCode>> combinations = GenerateCombinations();

        int counter = 0;

        for (int i = 0; i < row.transform.childCount; i++)
        {
            Platform platform = row.transform.GetChild(i).gameObject.GetComponent<Platform>();

            //Saw Platform - Get unsafe combination
            if (platform.CompareTag("Saw"))
            {
                platform.IsSafe = false;
                platform.Combination = combinations[counter];
                platform.Target.transform.position = platform.transform.position + new Vector3(0, platformExtentsY + playerExtentsY, 0);

                counter++;
            }

            //Safe Platform - Get safe combination at the end of the list
            else if (platform.CompareTag("Safe"))
            {
                platform.IsSafe = true;
                platform.Combination = combinations[combinations.Count - 1];
                platform.Target.transform.position = platform.transform.position + new Vector3(0, platformExtentsY + playerExtentsY, 0);
            }

            rowScript.Platforms.Add(platform.gameObject);
        }

        return row;
    }

    /// <summary>
    /// Generates a combination associated with each platform.
    /// The safe combination is always the last one in the list.
    /// </summary>
    private List<List<KeyCode>> GenerateCombinations()
    {
        List<List<KeyCode>> combinations = new List<List<KeyCode>>();

        //Create room for the number of platforms in a row
        for (int i = 0; i < prefabVariation.NumPlatforms; i++)
        {
            combinations.Add(new List<KeyCode>());
        }

        for (int i = 0; i < numCombo; i++)
        {
            //Last key in combo - Make different
            if (i == numCombo - 1)
            {
                List<KeyCode> excludedKeys = new List<KeyCode>();

                for (int j = 0; j < combinations.Count; j++)
                {
                    KeyCode randomkey = GetRandomKeyCode(excludedKeys);

                    //Store key in platform
                    combinations[j].Add(randomkey);

                    //Keep track of keys to exclude
                    excludedKeys.Add(randomkey);
                }
            }

            //Building combo - Make keys the same
            else
            {
                KeyCode randomKey = GetRandomKeyCode();

                for (int j = 0; j < combinations.Count; j++)
                {
                    combinations[j].Add(randomKey);
                }
            }
        }

        return combinations;
    }

    /// <summary>
    /// Gets a random key code based on the supported keys.
    /// </summary>
    /// <param name="excludedKeys">Keys to exclude</param>
    /// <returns>A random key code based on the supported keys.</returns>
    private KeyCode GetRandomKeyCode(List<KeyCode> excludedKeys = null)
    {
        int rand;

        if (excludedKeys != null && excludedKeys.Count > 0)
        {
            //Get a copy of the possible keys
            List<KeyCode> copy = keys.ToList();

            //Remove the excluded keys
            for (int i = 0; i < excludedKeys.Count; i++)
            {
                copy.Remove(excludedKeys[i]);
            }

            //Get a random key code
            rand = Random.Range(0, copy.Count);
            return copy[rand];
        }

        rand = Random.Range(0, keys.Length);
        return keys[rand];
    }

    /// <summary>
    /// Spawns in the inital platforms and the player.
    /// </summary>
    private void SetupGame()
    {
        //Set the spawn position to match the spawn interval
        spawn.transform.position += new Vector3(0, cameraUpperBounds + spawnInterval, 0);

        //Spawn pos to generate the initial platforms
        Vector3 spawnPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);

        //Setup the first row to be the starting row
        GameObject row = SpawnRow(spawnPos);
        PlatformRow rowScript = row.GetComponent<PlatformRow>();
        rowScript.IsActive = true;

        for (int i = 0; i < row.transform.childCount; i++)
        {
            Platform platform = row.transform.GetChild(i).gameObject.GetComponent<Platform>();

            //No combination for first row spawned
            platform.Combination = new List<KeyCode>();

            //Spawn player on safe platform
            if (platform.CompareTag("Safe"))
            {
                Vector3 playerPos = platform.Target.transform.position;
                playerInstance = Instantiate(playerPrefab, playerPos, Quaternion.identity, platform.transform);
            }
        }

        rowScript.SendPlatforms();

        Debug.Log("Spawned first row and player");

        bool spawnedSecond = false;

        //Keep spawning rows until conditions are met to spawn a row off screen
        while (spawnPos.y + platformExtentsY + spawnInterval < cameraUpperBounds)
        {
            //Update the position
            spawnPos += new Vector3(0, platformExtentsY + spawnInterval, 0);

            //Setup the next row to be on screen
            GameObject nextRow = SpawnRow(spawnPos);
            PlatformRow nextRowScript = nextRow.GetComponent<PlatformRow>();
            nextRowScript.IsActive = true;
            nextRowScript.SendPlatforms();

            //Highlight the second row
            if (!spawnedSecond)
            {
                spawnedSecond = true;
                nextRowScript.HighlightRow();
            }

            Debug.Log("Spawned another row on screen");
        }

        //Update the position
        spawnPos += new Vector3(0, platformExtentsY + spawnInterval, 0);

        //Spawn the last row off screen
        GameObject lastRow = SpawnRow(spawnPos);
        PlatformRow lastRowScript = lastRow.GetComponent<PlatformRow>();

        //Highlight the second row
        if (!spawnedSecond)
        {
            spawnedSecond = true;
            lastRowScript.HighlightRow();
        }

        Debug.Log("Spawned last row off screen");
    }
}
