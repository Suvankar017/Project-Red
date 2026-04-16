using UnityEngine;

namespace ShadowMatch
{
    [CreateAssetMenu(fileName = "New Theme Data", menuName = "Shadow Match/Theme Data")]
    public class ThemeData : ScriptableObject
    {
        [SerializeField]
        private Color skyColor;

        [SerializeField]
        private GameObject themePrefab;

        [SerializeField]
        private Puzzle[] puzzleArray;

        public Color SkyColor => skyColor;
        public GameObject ThemePrefab => themePrefab;
        public Puzzle[] PuzzleArray => puzzleArray;
    }
}
