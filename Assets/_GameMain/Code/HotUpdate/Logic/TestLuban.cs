using System;
using MiniGame.Asset;
using MiniGame.Logger;
using SimpleJSON;
using UnityEngine;

public class TestLuban : MonoBehaviour
{
    private void Start()
    {
        var jsonFile = AssetModule.LoadTextFileSync("Assets/_GameMain/Data/item_tbitem.json");
        var tables = new cfg.Tables(file => JSON.Parse(jsonFile));
        LogModule.Info($"table item count: {tables.TbItem.DataList.Count}");
        LogModule.Info($"table item 1 name: {tables.TbItem[10000].Name}");
    }
}