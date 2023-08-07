using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLogic : MonoBehaviour
{
    public Image img;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("TestLogic.Start 22222222");
        img.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
