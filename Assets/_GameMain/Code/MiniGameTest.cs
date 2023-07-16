using System.Collections;
using System.Collections.Generic;
using MiniGame.Base;
using UnityEngine;
using Logger = MiniGame.Logger.Logger;

public class MiniGameTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MiniGameCore.GetModule<Logger>().Info("Test Message");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
