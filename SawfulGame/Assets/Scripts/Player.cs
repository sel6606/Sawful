using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<PlatformRow> activeRows = new List<PlatformRow>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch(activeRows.Count)
        {
            case 0: //No rows are active
            case 1:
                //If we get here, either nothing has been initialized or we are at the top row and we do not currently need to do any checks
                break;
            default: //Two or more rows are active
                ChangeRows();
                break;
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
            if(Input.GetKeyDown(g.GetComponent<Platform>().Combination[0]))
            {
                nextPlat = g.GetComponent<Platform>();
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
        }

    }
}
