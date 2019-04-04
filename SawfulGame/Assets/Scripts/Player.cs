using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<PlatformRow> activeRows = new List<PlatformRow>();
    private List<KeyCode> currentInput = new List<KeyCode>();
    private KeyCode[] possibleKeys = new KeyCode[]
    {
        KeyCode.A,
        KeyCode.B,
        KeyCode.C,
        KeyCode.D,
        KeyCode.E,
        KeyCode.F,
        KeyCode.G,
        KeyCode.H,
        KeyCode.I,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L,
        KeyCode.M,
        KeyCode.N,
        KeyCode.O,
        KeyCode.P,
        KeyCode.Q,
        KeyCode.R,
        KeyCode.S,
        KeyCode.T,
        KeyCode.U,
        KeyCode.V,
        KeyCode.W,
        KeyCode.X,
        KeyCode.Y,
        KeyCode.Z
    };

    public float maxTimeBetweenPresses;
    private float pressCooldown;

    // Start is called before the first frame update
    void Start()
    {
        pressCooldown = maxTimeBetweenPresses;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInfo.instance.GameStart && !GameInfo.instance.Paused && !GameInfo.instance.GameOver)
        {
            switch (activeRows.Count)
            {
                case 0: //No rows are active
                case 1:
                    //If we get here, either nothing has been initialized or we are at the top row and we do not currently need to do any checks
                    break;
                default: //Two or more rows are active
                    KeyCode keyPressed = GetCurrentKey();

                    if(keyPressed != KeyCode.None)
                    {
                        currentInput.Add(keyPressed);
                    }
                    if (currentInput.Count > 0)
                    {
                        pressCooldown -= Time.deltaTime;
                    }
                    else
                    {
                        pressCooldown = maxTimeBetweenPresses;
                        currentInput.Clear();
                    }
                    ChangeRows();
                    break;
            }
        }
    }

    public void AddRowToQueue(PlatformRow platforms)
    {
        activeRows.Add(platforms);
    }

    private void ChangeRows()
    {
        Platform nextPlat = null;

        foreach (GameObject g in activeRows[1].Platforms)
        {
            List<KeyCode> platformCombo = g.GetComponent<Platform>().Combination;

            if (currentInput.Count >= platformCombo.Count)
            {
                bool match = true;
                for (int i = 0; i < platformCombo.Count; i++)
                {
                    if(currentInput[i] != platformCombo[i])
                    {
                        match = false;
                    }
                }

                if(match)
                {
                    nextPlat = g.GetComponent<Platform>();
                }
            }
        }

        if (nextPlat != null)
        {
            gameObject.transform.position = nextPlat.Target.transform.position;
            gameObject.transform.parent = nextPlat.transform;
            activeRows.RemoveAt(0);

            if(!nextPlat.IsSafe)
            {
                GameInfo.instance.GameOver = true;
            }

            currentInput.Clear();
            pressCooldown = maxTimeBetweenPresses;
        }

    }

    private KeyCode GetCurrentKey()
    {
        foreach (KeyCode kcode in possibleKeys)
        {
            if (Input.GetKeyDown(kcode))
            {
                return kcode;
            }
        }

        return KeyCode.None;
    }
}
