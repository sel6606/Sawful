using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to create different variations of rows with the prefabs
/// </summary>
public class PrefabVariation : MonoBehaviour
{
    public int numPlatforms;
    public float spacing;

    public GameObject safePrefab;
    public GameObject sawPrefab;

    public int NumPlatforms
    {
        get { return numPlatforms; }
        set { numPlatforms = value; }
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
    /// Creates a row based on the number of platforms
    /// </summary>
    /// <returns>The created row</returns>
    public GameObject CreateRow()
    {
        GameObject row = new GameObject("Platform Row");

        //Make a random platform safe
        int rand = Random.Range(0, numPlatforms);

        //x will move from right to left (positive to negative) to place the platforms
        float x = numPlatforms / 2 * spacing;

        //Even number of platforms - There should be space in the middle (x = 0) of the row
        if (numPlatforms % 2 == 0)
        {
            x -= spacing / 2;
        }

        for (int i = 0; i < numPlatforms; i++)
        {
            //Spawn the safe platform
            if (i == rand)
            {
                Instantiate(safePrefab, new Vector3(x, 0, 0), Quaternion.identity, row.transform);
            }

            //Spawn the saw platform
            else
            {
                Instantiate(sawPrefab, new Vector3(x, 0, 0), Quaternion.identity, row.transform);
            }

            x -= spacing;
        }

        AttachComponents(row);

        return row;
    }

    /// <summary>
    /// Attaches the necessary components to a row.
    /// </summary>
    private void AttachComponents(GameObject row)
    {
        row.AddComponent<PlatformRow>();
    }
}
