using UnityEngine;

namespace ShadowMatch
{
    public class DragAndDropManager : MonoBehaviour
    {
        private Camera mainCamera;
        private GameObject dragObject;
        private bool isDraggingItem;

        private static DragAndDropManager instance;
        public static DragAndDropManager Instance => instance;

        private void Awake()
        {
            instance = this;
            mainCamera = Camera.main;
            dragObject = null;
        }

        private void Update()
        {
            if (!isDraggingItem)
                return;

            Vector3 pos = GetMousePositionWSWithNoZ();
            dragObject.transform.position = pos;
        }

        private Vector3 GetMousePositionWS() => mainCamera.ScreenToWorldPoint(Input.mousePosition);

        private Vector3 GetMousePositionWSWithNoZ()
        {
            Vector3 mousePos = GetMousePositionWS();
            mousePos.z = 0.0f;
            return mousePos;
        }

        public void OnItemDragBegin(ItemData itemData)
        {
            // Handle any logic needed when an item drag begins
            // For example, you might want to show a drag preview or play a sound effect

            isDraggingItem = true;
            Vector3 spawnPoint = GetMousePositionWSWithNoZ();
            dragObject = Instantiate(itemData.ItemPrefab, spawnPoint, Quaternion.identity, transform);
        }

        public void OnItemDragEnd(ItemData itemData)
        {
            isDraggingItem = false;

            if (dragObject != null)
            {
                Destroy(dragObject);
                dragObject = null;
            }
        }
    }
}
