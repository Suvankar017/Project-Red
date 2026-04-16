using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ShadowMatch
{
    public static class SDFTextureSaver
    {
        public static void SaveTextureAsAsset(Texture2D sdfTexture, string path)
        {
#if UNITY_EDITOR
            // Ensure folder exists
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Encode texture to PNG
            byte[] pngData = sdfTexture.EncodeToPNG();

            if (pngData == null)
            {
                Debug.LogError("Failed to encode texture");
                return;
            }

            // Write file
            File.WriteAllBytes(path, pngData);

            // Refresh Unity asset database
            AssetDatabase.Refresh();

            Debug.Log("Texture saved to: " + path);
#endif
        }
    }
}
