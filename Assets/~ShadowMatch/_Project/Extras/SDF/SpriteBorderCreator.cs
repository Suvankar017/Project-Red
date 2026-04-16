using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowMatch
{
    public class SpriteBorderCreator : MonoBehaviour
    {
        public Sprite inputSprite;
        public string outputDirectoryPath = "Assets";
        public string outputFileName = "SDF_Output";
        public string outputFileExtension = "png";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Create(inputSprite);
            }
        }

        private void Create(Sprite sprite)
        {
            if (sprite == null)
            {
                Debug.LogError("Input sprite is null.");
                return;
            }

            Texture2D texture = sprite.texture;
            Texture2D sdfTexture = SDFTextureGenerator.GenerateSDF(texture);

            string path = $"{outputDirectoryPath}/{outputFileName}.{outputFileExtension}";

            SDFTextureSaver.SaveTextureAsAsset(sdfTexture, path);
            SDFTextureImporterSetup.Setup(path);
        }
    }
}
