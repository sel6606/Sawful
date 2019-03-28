using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spawning : MonoBehaviour
{
    public int numCombo;
    public GameObject spawn;
    public GameObject[] platformPrefabs;
    public List<GameObject> spawnedPlatforms;

    //private KeyCode[] keys = new KeyCode[]
    //{
    //    KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N,
    //    KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z
    //};

    private KeyCode[] keys = new KeyCode[]
    {
        KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow
    };


    public List<GameObject> SpawnedPlatforms
    {
        get { return spawnedPlatforms; }
        set { spawnedPlatforms = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Spawns a row of platforms at the spawn point.
    /// </summary>
    public void SpawnRow()
    {
        SpawnRow(spawn.transform.position);
    }

    /// <summary>
    /// Spawns a row of platforms at the given position.
    /// </summary>
    /// <param name="pos">Position to spawn at</param>
    public void SpawnRow(Vector3 pos)
    {
        //Get a random row type
        int rand = Random.Range(0, platformPrefabs.Length);
        GameObject row = Instantiate(platformPrefabs[rand], pos, Quaternion.identity, transform);

        //Safe combination is always the last one in the list
        List<List<KeyCode>> combinations = GenerateCombinations();

        for (int i = 0; i < row.transform.childCount; i++)
        {
            GameObject child = row.transform.GetChild(i).gameObject;

            //Saw Platform -  Get unsafe combination at the beginning of the list
            if (child.CompareTag("Saw"))
            {
                child.GetComponent<Platform>().IsSafe = false;
                child.GetComponent<Platform>().Combination = combinations[0];

                //Make sure to remove, to not use it again
                combinations.RemoveAt(0);
            }

            //Safe Platform - Get safe combination at the end of the list
            else if (child.CompareTag("Safe"))
            {
                child.GetComponent<Platform>().IsSafe = true;
                child.GetComponent<Platform>().Combination = combinations[combinations.Count - 1];

                //Make sure to remove, to not use it again
                combinations.RemoveAt(combinations.Count - 1);
            }
        }

        spawnedPlatforms.Add(row);
    }

    /// <summary>
    /// Generates a combination associated with each platform.
    /// The safe combination is always the last one in the list.
    /// </summary>
    private List<List<KeyCode>> GenerateCombinations()
    {
        //Three lists for the 3 platforms
        List<List<KeyCode>> combinations = new List<List<KeyCode>>()
        {
            new List<KeyCode>(),
            new List<KeyCode>(),
            new List<KeyCode>()
        };

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
}
