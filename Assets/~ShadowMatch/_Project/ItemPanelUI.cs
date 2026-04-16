using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowMatch
{
    public class ItemPanelUI : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect scrollRect;

        [SerializeField]
        private GameObject itemPrefab;

        private Coroutine spawnCoroutine;
        private Coroutine removeCoroutine;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log($"Horizontal position: {scrollRect.horizontalNormalizedPosition}");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (spawnCoroutine != null)
                    StopCoroutine(spawnCoroutine);

                spawnCoroutine = StartCoroutine(Spawn());
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (removeCoroutine != null)
                    StopCoroutine(removeCoroutine);

                removeCoroutine = StartCoroutine(Remove());
            }
        }

        private void ValidateScrollRect()
        {
            scrollRect.horizontalNormalizedPosition = 0.0f;
        }

        private IEnumerator Spawn()
        {
            if (itemPrefab == null)
                yield break;

            for (int i = 0; i < 10; i++)
            {
                GameObject newItem = Instantiate(itemPrefab, scrollRect.content);
                newItem.name = $"Item {scrollRect.content.childCount}";
            }

            yield return null; // Wait for the next frame

            scrollRect.horizontalNormalizedPosition = 0.0f;
            spawnCoroutine = null;
        }

        private IEnumerator Remove()
        {
            if (scrollRect.content.childCount == 0)
                yield break;

            Transform lastItem = scrollRect.content.GetChild(scrollRect.content.childCount - 1);
            Destroy(lastItem.gameObject);

            yield return null; // Wait for the next frame

            scrollRect.horizontalNormalizedPosition = 0.0f;
            removeCoroutine = null;
        }
    }
}
