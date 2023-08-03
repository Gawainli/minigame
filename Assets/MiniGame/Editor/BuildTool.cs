using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MiniGame.Editor
{
    public static class BuildTool 
    {
        [MenuItem("Build Tool/Build All DLLs")]
        public static void BuildAllDLLs()
        {
            BuildHotUpdateDlls();
            BuildAOTDlls();
        }
        
        private static void BuildHotUpdateDlls()
        {
            
        }
        
        private static void BuildAOTDlls()
        {
            
        }
        
        [MenuItem("Build Tool/Build All Assets")]
        public static void BuildAllAssets()
        {
            
        }
        
        [MenuItem("Build Tool/BUild All")]
        public static void BuildAll()
        {
            BuildAllDLLs();
            BuildAllAssets();
        }
    }
}