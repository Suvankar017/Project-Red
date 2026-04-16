using System.Collections.Generic;
using UnityEngine;

namespace ShadowMatch
{
    public class PuzzleManager : MonoBehaviour
    {
        [SerializeField]
        private Puzzle puzzlePrefab;
        [SerializeField]
        private Transform puzzleContainer;
        [SerializeField]
        private Transform itemUIContainer;

        private Puzzle puzzleInstance;
        private int totalSlots;
        private int correctMatches;

        private static PuzzleManager instance;

        public static PuzzleManager Instance => instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            if (puzzlePrefab != null)
                SetupPuzzle(puzzlePrefab);
        }

        public void SetupPuzzle(Puzzle puzzle)
        {
            Clear();

            totalSlots = -1;
            correctMatches = 0;

            // Spawn Puzzle
            puzzleInstance = Instantiate(puzzle, Vector3.zero, Quaternion.identity, puzzleContainer);
        }

        private void Clear()
        {
            foreach (Transform t in itemUIContainer)
                Destroy(t.gameObject);

            if (puzzleInstance != null)
                Destroy(puzzleInstance.gameObject);
        }

        public void RegisterCorrectMatch()
        {
            correctMatches++;

            if (correctMatches >= totalSlots)
            {
                //GameManager.Instance.OnPuzzleCompleted();
                Debug.Log("Puzzle completed!");
                ShadowMatchGameManager.Instance.LoadNextPuzzle();
            }
        }

        public void SetItemData(List<ItemData> itemDataList)
        {
            if (itemDataList == null || itemDataList.Count == 0)
                return;

            totalSlots = itemDataList.Count;

            // Spawn Items UI
            foreach (var itemData in itemDataList)
            {
                DraggableItem item = Instantiate(itemData.ItemUIPrefab, itemUIContainer);
                item.SetItemData(itemData);
            }
        }
    }
}
