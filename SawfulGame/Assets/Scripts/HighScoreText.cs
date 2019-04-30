using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUppercase()
    {
        GetComponent<TMP_InputField>().text = GetComponent<TMP_InputField>().text.ToUpper();
    }
}
