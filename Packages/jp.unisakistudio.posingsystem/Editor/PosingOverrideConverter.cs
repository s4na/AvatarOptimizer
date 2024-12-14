using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace jp.unisakistudio.posingsystem.Editor
{
    public class PosingOverrideConverter
    {
        [MenuItem("Tools/Posing System/Convert Posing Override")]
        public static void ConvertPosingOverride()
        {
            string[] guids = AssetDatabase.FindAssets("t:PosingOverride");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                PosingOverride posingOverride = AssetDatabase.LoadAssetAtPath<PosingOverride>(path);
                if (posingOverride != null)
                {
                    ConvertPosingOverride(posingOverride);
                }
            }
        }

        private static void ConvertPosingOverride(PosingOverride posingOverride)
        {
            string json = JsonUtility.ToJson(posingOverride);
            string path = AssetDatabase.GetAssetPath(posingOverride);
            string directory = Path.GetDirectoryName(path);
            string newPath = Path.Combine(directory, posingOverride.name + ".json");

            try
            {
                File.WriteAllText(newPath, json);
                AssetDatabase.ImportAsset(newPath);
            }
            catch (Exception)
            {
                Debug.LogError($"Failed to convert PosingOverride: {posingOverride.name}");
            }
        }
    }
}
