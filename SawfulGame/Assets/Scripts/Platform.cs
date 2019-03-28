using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Platform : MonoBehaviour
{
    public GameObject platform;
    public GameObject target;
    public GameObject player;
    public GameObject text;

    private bool isSafe;
    private List<KeyCode> combination;

    public bool IsSafe
    {
        get { return isSafe; }
        set { isSafe = value; }
    }

    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }

    public List<KeyCode> Combination
    {
        get { return combination; }
        set { combination = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetTargetPlayerPosition();
        DisplayCombination();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Offsets the position on the platform that the player moves to by the player's size.
    /// </summary>
    public void SetTargetPlayerPosition()
    {
        Bounds platformBounds = platform.GetComponent<SpriteRenderer>().bounds;
        Bounds playerBounds = player.GetComponent<SpriteRenderer>().bounds;

        float platformUpperBounds = platformBounds.max.y;
        float playerExtentsY = playerBounds.extents.y;

        float offsetY = platformUpperBounds + playerExtentsY;

        target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + offsetY, target.transform.position.z);
    }

    /// <summary>
    /// Displays the combination on the platform
    /// </summary>
    public void DisplayCombination()
    {
        string combo = "";

        for (int i = 0; i < combination.Count; i++)
        {
            combo += combination[i].ToString();
        }

        text.GetComponent<TextMeshPro>().text = combo;
    }
}
