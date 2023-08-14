using System.Collections;
using System.Collections.Generic;
using MiniGame.Asset;
using MiniGame.Logger;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class TestLogic : MonoBehaviour
{
    public Image img;
    // Start is called before the first frame update
    void Start()
    {
        img.color = Color.blue;
        var jsonFile = AssetModule.LoadTextFileSync("Assets/_GameMain/Data/item_tbitem.json");
        var tables = new cfg.Tables(file => JSON.Parse(jsonFile));
        LogModule.Info($"table item count: {tables.TbItem.DataList.Count}");
        LogModule.Info($"table item 1 name: {tables.TbItem[10000].Name}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
