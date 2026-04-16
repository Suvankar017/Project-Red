using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ShadowMatch
{
    [RequireComponent(typeof(RectTransform))]
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        private ScrollRect scrollRect;
        private ItemData itemData;

        private bool isDraggingItem;
        private Vector2 dragStartPos;

        public bool IsDraggingItem => isDraggingItem;
        public ItemData ItemData => itemData;

        private void Awake()
        {
            scrollRect = GetComponentInParent<ScrollRect>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector2 delta = eventData.position - dragStartPos;

            // Amplify vertical movement to make it more likely to drag the item vertically
            const float verticalAmplification = 3.0f;
            delta.y *= verticalAmplification;

            if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
            {
                isDraggingItem = true;
                OnItemDragBegin(eventData);
                scrollRect.velocity = Vector2.zero;
            }
            else
            {
                isDraggingItem = false;
                scrollRect.OnBeginDrag(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDraggingItem)
            {
                OnItemDrag(eventData);
            }
            else
            {
                scrollRect.OnDrag(eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isDraggingItem)
            {
                OnItemDragEnd(eventData);
            }
            else
            {
                scrollRect.OnEndDrag(eventData);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            dragStartPos = eventData.position;
            isDraggingItem = false;
            scrollRect.velocity = Vector2.zero;
        }

        public void SetItemData(ItemData data)
        {
            itemData = data;
        }

        public void OnDropSuccess()
        {
            // Handle any logic needed when the item is successfully dropped into a slot
            // For example, you might want to disable the item or play a sound effect
            gameObject.SetActive(false);

            DragAndDropManager.Instance.OnItemDragEnd(itemData);
        }

        private void OnItemDragBegin(PointerEventData eventData)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);

            DragAndDropManager.Instance.OnItemDragBegin(itemData);
        }

        private void OnItemDrag(PointerEventData eventData)
        {

        }

        private void OnItemDragEnd(PointerEventData eventData)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(true);

            DragAndDropManager.Instance.OnItemDragEnd(itemData);
        }
    }
}
