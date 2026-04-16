using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ShadowMatch
{
    public static class SDFTextureImporterSetup
    {
        public static void Setup(string path)
        {
#if UNITY_EDITOR
            AssetImporter importer = AssetImporter.GetAtPath(path);

            if (importer is TextureImporter texImporter)
            {
                texImporter.textureCompression = TextureImporterCompression.Uncompressed;
                texImporter.filterMode = FilterMode.Bilinear;
                texImporter.sRGBTexture = false;

                texImporter.SaveAndReimport();
            }
#endif
        }
    }
}
