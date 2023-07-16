using UnityEngine;
using MiniGame.Logger;

public class MiniGameTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MLogger.Info("Test Info");
        MLogger.Warning("Test Warning");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
