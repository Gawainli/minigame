using System.Collections;
using System.Collections.Generic;
using MiniGame.Asset;
using MiniGame.Logger;
using MiniGame.UI;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class TestLogic : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        UIModule.OpenWindowSync<TestMainWindow>("Assets/_GameMain/Prefab/UI/TestMainUI.prefab", 0, true, null);
        // img.color = Color.blue;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
