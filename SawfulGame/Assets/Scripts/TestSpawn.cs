using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    public GameObject testItem;
    public GameObject spawn;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            gameObject.GetComponent<Spawning>().SpawnRow(spawn.transform.position);
            spawn.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y + 3, spawn.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
