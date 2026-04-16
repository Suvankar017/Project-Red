using UnityEngine;

namespace ShadowMatch
{
    [CreateAssetMenu(fileName = "New Game Data", menuName = "Shadow Match/Game Data")]
    public class ShadowMatchGameData : ScriptableObject
    {
        [SerializeField]
        private ThemeData[] themeArray;

        public int ThemeCount => (themeArray == null) ? 0 : themeArray.Length;

        public ThemeData GetTheme(int index)
        {
            return (themeArray == null || index < 0 || index >= themeArray.Length) ? null : themeArray[index];
        }

        public bool TryGetTheme(int index, out ThemeData theme)
        {
            if (themeArray == null || index < 0 || index >= themeArray.Length)
            {
                theme = null;
                return false;
            }

            theme = themeArray[index];
            return true;
        }
    }
}
