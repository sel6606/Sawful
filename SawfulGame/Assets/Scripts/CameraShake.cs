using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to shake the camera for a set amount of time.
/// </summary>
public class CameraShake : MonoBehaviour
{
    public float amount;
    public float duration;

    private Vector3 originalPos;
    private float shakeThreshold;
    private float timer;
    private float totalTime;
    private bool isShaking;

    public bool IsShaking
    {
        get { return isShaking; }
        set { isShaking = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        originalPos = Camera.main.transform.position;
        shakeThreshold = 0.01f;
        timer = shakeThreshold;
        totalTime = 0;
        isShaking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInfo.instance.GameStart && !GameInfo.instance.Paused && !GameInfo.instance.GameOver)
        {
            if (isShaking)
            {
                Shake();
            }
        }
    }

    /// <summary>
    /// Moves the camera randomly to shake it for a set time.
    /// </summary>
    private void Shake()
    {
        timer -= Time.deltaTime;
        totalTime += Time.deltaTime;

        //Ready to shake - Move camera by random offsets
        if (timer <= 0)
        {
            timer = shakeThreshold;

            float offsetX = Random.value * amount * 2 - amount;
            float offsetY = Random.value * amount * 2 - amount;

            Vector3 pos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(pos.x + offsetX, pos.y + offsetY, pos.z);
        }

        //Finished shaking - Set camera back to orginal position
        if (totalTime >= duration)
        {
            //Reset everything
            isShaking = false;
            timer = shakeThreshold;
            totalTime = 0;

            Camera.main.transform.position = originalPos;
        }
    }
}
