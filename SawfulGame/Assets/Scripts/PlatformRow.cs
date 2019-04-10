using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class attached to each row of platforms.
/// </summary>
public class PlatformRow : MonoBehaviour
{
    private Spawning spawner;
    private float extentsY;
    private float cameraUpperBounds;
    private float cameraLowerBounds;

    private bool isActive;
    private List<GameObject> platforms;

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    public List<GameObject> Platforms
    {
        get { return platforms; }
        set { platforms = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        spawner = transform.parent.GetComponent<Spawning>();
        extentsY = spawner.PlatformExtentsY;
        cameraUpperBounds = spawner.CameraUpperBounds;
        cameraLowerBounds = spawner.CameraLowerBounds;
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
        float speed = spawner.MoveSpeed * Time.deltaTime;
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
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Spawns the next row of platforms.
    /// </summary>
    private void SpawnNextRow()
    {
        spawner.SpawnRow();
    }

    /// <summary>
    /// Sends the platforms to the input script
    /// </summary>
    public void SendPlatforms()
    {
        transform.parent.GetComponent<Spawning>().PlayerInstance.GetComponent<Player>().AddRowToQueue(this);
    }

    /// <summary>
    /// Makes the combo text for the platforms more visible.
    /// </summary>
    public void HighlightRow()
    {
        for (int i = 0; i < platforms.Count; i++)
        {
            platforms[i].GetComponent<Platform>().MakeVisible();
        }
    }
}
