using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using HybridCLR;
using MiniGame.Asset;
using MiniGame.Logger;
using UnityEngine;

namespace MiniGame.Sample
{
    public static class HotUpdateHelper
    {
        private static readonly List<string> _aotDllList = new List<string>()
        {
            "Assets/HotUpdateDll/mscorlib.dll.bytes",
            "Assets/HotUpdateDll/System.Core.dll.bytes",
            "Assets/HotUpdateDll/System.dll.bytes"
        };
        
        private static readonly List<string> _hotUpdateDllList = new List<string>()
        {
            "Assets/HotUpdateDll/Hotfix.dll.bytes"
        };

        public static async UniTask<bool> LoadAllHotUpdate()
        {
            LogModule.Info("Load All Hot Update");
            var ret = await LoadMetadataForAOTAssemblies();
            if (!ret)
            {
                return false;
            }
            ret = await LoadHotUpdateDlls();
            return ret;
        }

        private static async UniTask<bool> LoadMetadataForAOTAssemblies()
        {
            foreach (var aotDllName in _aotDllList)
            {
                var dllBytes = await AssetModule.LoadRawFileAsync(aotDllName);
                var err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
                if (err != LoadImageErrorCode.OK)
                {
                    LogModule.Error($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
                    return false;
                }
            }
            return true;
        }
        
        private static async UniTask<bool> LoadHotUpdateDlls()
        {
            foreach (var hotDllName in _hotUpdateDllList)
            {
                var dllBytes = await AssetModule.LoadRawFileAsync(hotDllName);
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
            return true;
        }
    }
}