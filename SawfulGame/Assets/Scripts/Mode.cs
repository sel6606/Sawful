using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mode : MonoBehaviour
{
    private void Awake()
    {
        switch(GameInfo.instance.Mode)
        {
            case Difficulty.Easy:
                GetComponent<TextMeshProUGUI>().text = "Hall of Fame (Easy)";
                break;
            case Difficulty.Normal:
                GetComponent<TextMeshProUGUI>().text = "Hall of Fame (Normal)";
                break;
            case Difficulty.Hard:
                GetComponent<TextMeshProUGUI>().text = "Hall of Fame (Hard)";
                break;
            default:
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
