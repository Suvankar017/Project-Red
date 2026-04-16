using System.Collections.Generic;
using UnityEngine;

namespace ShadowMatch
{
    public class ShadowMatchGameManager : MonoBehaviour
    {
        [SerializeField]
        private ShadowMatchGameData gameData;
        [SerializeField]
        private Transform themeContainer;

        private Camera mainCamera;
        private ThemeData currentTheme;
        private GameObject themeInstance;
        private readonly Dictionary<ThemeData, Queue<Puzzle>> themePuzzleQueues = new();

        private static ShadowMatchGameManager instance;

        public ThemeData CurrentTheme => currentTheme;
        public ShadowMatchGameData GameData => gameData;

        public static ShadowMatchGameManager Instance => instance;

        private void Awake()
        {
            instance = this;

            Init();
        }

        private void Start()
        {
            int initialThemeIndex = Random.Range(0, gameData.ThemeCount);
            if (gameData.TryGetTheme(initialThemeIndex, out ThemeData initialTheme))
                SelectTheme(initialTheme);
        }

        private void Init()
        {
            mainCamera = Camera.main;

            themePuzzleQueues.Clear();

            for (int i = 0; i < gameData.ThemeCount; i++)
            {
                if (!gameData.TryGetTheme(i, out ThemeData theme))
                    continue;

                Queue<Puzzle> puzzleQueue = new(theme.PuzzleArray.Length);
                ShufflePuzzles(puzzleQueue, theme);
                themePuzzleQueues[theme] = puzzleQueue;
            }
        }

        public void SelectTheme(ThemeData themeData)
        {
            ThemeData previousTheme = currentTheme;
            currentTheme = themeData;

            if (currentTheme != previousTheme)
            {
                if (themeInstance != null)
                    Destroy(themeInstance);

                themeInstance = Instantiate(currentTheme.ThemePrefab, Vector3.zero, Quaternion.identity, themeContainer);
            }

            mainCamera.backgroundColor = currentTheme.SkyColor;
            LoadNextPuzzle();
        }

        public void LoadNextPuzzle()
        {
            if (!themePuzzleQueues.TryGetValue(currentTheme, out Queue<Puzzle> puzzleQueue))
            {
                Debug.LogError($"No puzzle queue found for theme: {currentTheme.name}");
                return;
            }

            if (puzzleQueue.Count == 0)
                ShufflePuzzles(puzzleQueue, currentTheme);

            Puzzle next = puzzleQueue.Dequeue();
            PuzzleManager.Instance.SetupPuzzle(next);
        }

        private void ShufflePuzzles(Queue<Puzzle> puzzleQueue, ThemeData theme)
        {
            List<Puzzle> shuffled = new(theme.PuzzleArray);

            for (int i = 0; i < shuffled.Count; i++)
            {
                int rand = Random.Range(i, shuffled.Count);
                (shuffled[i], shuffled[rand]) = (shuffled[rand], shuffled[i]);
            }

            puzzleQueue.Clear();
            for (int i = 0; i < shuffled.Count; i++)
                puzzleQueue.Enqueue(shuffled[i]);
        }
    }
}
