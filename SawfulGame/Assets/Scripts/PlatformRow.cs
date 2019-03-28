using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class attached to each row of platforms.
/// </summary>
public class PlatformRow : MonoBehaviour
{
    private float extentsY;
    private float cameraUpperBounds;
    private float cameraLowerBounds;

    private bool isActive;
    private bool hasPlayer;

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    public bool HasPlayer
    {
        get { return hasPlayer; }
        set { hasPlayer = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraUpperBounds = transform.parent.GetComponent<Spawning>().CameraUpperBounds;
        cameraLowerBounds = transform.parent.GetComponent<Spawning>().CameraLowerBounds;
        extentsY = transform.parent.GetComponent<Spawning>().PlatformExtentsY;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInfo.instance.GameStart && !GameInfo.instance.Paused && !GameInfo.instance.GameOver)
        {
            Move();

            if (!isActive)
            {
                CheckIfOnScreen();
            }
            else
            {
                CheckIfOffScreen();
            }
        }
    }

    /// <summary>
    /// Moves the row of platforms down the screen.
    /// </summary>
    private void Move()
    {
        float speed = GameInfo.instance.moveSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
    }

    /// <summary>
    /// Checks if the row has entered the camera view.
    /// </summary>
    private void CheckIfOnScreen()
    {
        float rowUpperBounds = transform.position.y + extentsY;

        if (rowUpperBounds <= cameraUpperBounds)
        {
            isActive = true;
            SpawnNextRow();
            SendPlatforms();
        }
    }

    /// <summary>
    /// Checks if the row has left the camera view and destroys it
    /// </summary>
    private void CheckIfOffScreen()
    {
        float rowUpperBounds = transform.position.y + extentsY;

        if (rowUpperBounds <= cameraLowerBounds)
        {
            transform.parent.GetComponent<Spawning>().SpawnedPlatforms.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Spawns the next row of platforms.
    /// </summary>
    private void SpawnNextRow()
    {
        transform.parent.GetComponent<Spawning>().SpawnRow();
    }

    /// <summary>
    /// Sends the platforms to the input script
    /// </summary>
    public void SendPlatforms()
    {
        List<GameObject> platforms = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            platforms.Add(transform.GetChild(i).gameObject);
        }

        //SEND TO INPUT SCRIPT
    }
}
