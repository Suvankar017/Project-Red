using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShadowMatch
{
    public class DropSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField]
        private ItemData itemData;

        public ItemData ItemData => itemData;

        public void SetItemData(ItemData data)
        {
            itemData = data;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData == null || eventData.pointerDrag == null)
                return;

            if (!eventData.pointerDrag.TryGetComponent(out DraggableItem draggableItem))
                return;

            if (!draggableItem.IsDraggingItem)
                return;

            if (itemData == null || itemData != draggableItem.ItemData)
                return;

            PuzzleManager.Instance.RegisterCorrectMatch();

            GameObject go = Instantiate(itemData.ItemPrefab, transform.position, Quaternion.identity, transform.parent);
            go.transform.localScale = transform.localScale;

            gameObject.SetActive(false);

            draggableItem.OnDropSuccess();
            //Debug.Log($"Dropped {draggableItem.name} into {gameObject.name}", eventData.pointerDrag);
        }
    }
}
