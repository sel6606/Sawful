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
    public GameObject deathPrefab;
    private float pressCooldown;
    private bool jumping = false;
    public Sprite jumpSprite;
    public Sprite idleSprite;

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
                    if (!jumping)
                    {
                        KeyCode keyPressed = GetCurrentKey();

                        if (keyPressed != KeyCode.None)
                        {
                            currentInput.Add(keyPressed);
                        }
                        ChangeRows();
                    }
                    break;
            }
        }

        if(!jumping && GameInfo.instance.GameOver)
        {
            GameObject temp = Instantiate(deathPrefab);
            temp.transform.position = gameObject.transform.position;
            foreach (Rigidbody2D r in temp.GetComponentsInChildren<Rigidbody2D>())
            {
                r.AddForce(new Vector2(1, 1));
            }
            Destroy(this.gameObject);
        }
    }

    public void AddRowToQueue(PlatformRow platforms)
    {
        activeRows.Add(platforms);
    }

    private void ChangeRows()
    {
        if(activeRows.Count > 1)
        {
            activeRows[1].HighlightRow();
        }
        if (currentInput.Count > 0)
        {
            Platform nextPlat = null;

            bool fullMatch = false;
            bool match = false;
            bool reset = false;

            foreach (GameObject g in activeRows[1].Platforms)
            {
                List<KeyCode> platformCombo = g.GetComponent<Platform>().Combination;

                if (currentInput.Count > platformCombo.Count)
                {
                    reset = true;
                    break;
                }

                if (currentInput.Count > 0)
                {
                    if (platformCombo[currentInput.Count - 1] == currentInput[currentInput.Count - 1])
                    {
                        if (currentInput.Count == platformCombo.Count)
                        {
                            fullMatch = true;
                            g.GetComponent<Platform>().HighlightCharacter(currentInput.Count - 1);
                        }
                        else
                        {
                            match = true;
                            g.GetComponent<Platform>().HighlightCharacter(currentInput.Count - 1);
                        }
                    }

                    if (fullMatch)
                    {
                        nextPlat = g.GetComponent<Platform>();
                        break;
                    }
                }
            }

            if (!fullMatch && !match)
            {
                reset = true;
            }

            if (reset)
            {
                reset = false;
                foreach (GameObject g in activeRows[1].Platforms)
                {
                    g.GetComponent<Platform>().HighlightCharacter(-1);
                }

                GetComponent<CameraShake>().IsShaking = true;
                currentInput.Clear();
            }

            if (nextPlat != null)
            {
                //gameObject.transform.position = nextPlat.Target.transform.position;
                GetComponent<SpriteRenderer>().sprite = jumpSprite;
                StartCoroutine(Jump(nextPlat, !nextPlat.IsSafe));
                GetComponent<Animator>().enabled = false;
                gameObject.transform.parent = null;
                activeRows.RemoveAt(0);

                if (!nextPlat.IsSafe)
                {
                    GameInfo.instance.GameOver = true;
                }
                else
                {
                    GameInfo.instance.Score++;
                }

                currentInput.Clear();

                
            }
        }

    }

    IEnumerator Jump(Platform moveDestination, bool death)
    {
        jumping = true;

        float moveTime = 0.2f;
        //float moveTime = 0.8f;
        while(moveTime > 0.0f)
        {
            moveTime -= Time.deltaTime;
            transform.position = Vector3.Lerp(moveDestination.target.transform.position, transform.position, moveTime);

            if(moveTime <= 0.0f)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }
        
        if(death)
        {
            GameObject temp = Instantiate(deathPrefab);
            temp.transform.position = moveDestination.target.transform.position;
            foreach(Rigidbody2D r in temp.GetComponentsInChildren<Rigidbody2D>())
            {
                r.AddForce(new Vector2(1, 1));
            }
            Destroy(this.gameObject);
        }
        jumping = false;
        GetComponent<SpriteRenderer>().sprite = idleSprite;
        GetComponent<Animator>().enabled = true;
        gameObject.transform.parent = moveDestination.transform;
        yield return null;
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
