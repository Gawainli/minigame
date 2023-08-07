using System;
using System.Collections.Generic;
using System.Reflection;
using HybridCLR;
using MiniGame.Asset;
using MiniGame.Logger;

public static class HotUpdateLoader
{
    private static readonly List<string> _aotDllList = new List<string>()
    {
        "Assets/HotUpdateDll/mscorlib.dll.bytes",
        "Assets/HotUpdateDll/System.Core.dll.bytes",
        "Assets/HotUpdateDll/System.dll.bytes"
    };

    private static readonly List<string> _hotUpdateDllList = new List<string>()
    {
        "Assets/HotUpdateDll/ADF_Logic.dll.bytes",
        "Assets/HotUpdateDll/ADF_Proto.dll.bytes",
        "Assets/HotUpdateDll/ADF_UI.dll.bytes",
    };

    public static bool LoadAllHotUpdate()
    {
        LogModule.Info("Load All Hot Update 1111");
        var ret = LoadMetadataForAOTAssemblies();
        if (!ret)
        {
            return false;
        }

        ret = LoadHotUpdateDlls();
        return ret;
    }

    public static bool LoadMetadataForAOTAssemblies()
    {
        foreach (var aotDllName in _aotDllList)
        {
            var dllBytes = AssetModule.LoadRawFileSync(aotDllName);
            var err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
            if (err != LoadImageErrorCode.OK)
            {
                LogModule.Error($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
                return false;
            }
        }

        return true;
    }

    public static bool LoadHotUpdateDlls()
    {
        foreach (var hotDllName in _hotUpdateDllList)
        {
            var dllBytes = AssetModule.LoadRawFileSync(hotDllName);
            if (dllBytes.Length == 0)
            {
                LogModule.Error($"LoadHotUpdateDlls:{hotDllName}. ret: dllBytes.Length == 0");
                return false;
            }

            try
            {
                Assembly.Load(dllBytes);
            }
            catch (Exception e)
            {
                LogModule.Error($"LoadHotUpdateDlls:{hotDllName}. ret:{e.Message}");
                return false;
            }
        }

        LogModule.Info("Load All Hot Update Success");
        return true;
    }
}