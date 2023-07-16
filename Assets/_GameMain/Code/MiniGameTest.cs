using System.Collections;
using System.Collections.Generic;
using MiniGame.Base;
using MiniGame.Logger;
using UnityEngine;

public class MiniGameTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MiniGameCore.GetModule<LogModule>().Info("Test Message");
        MiniGameCore.GetModule<LogModule>().Warning("Test warning");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
