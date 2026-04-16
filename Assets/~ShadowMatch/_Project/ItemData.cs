using UnityEngine;

namespace ShadowMatch
{
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Shadow Match/Item Data")]
    public class ItemData : ScriptableObject
    {
        [SerializeField]
        private GameObject itemPrefab;
        [SerializeField]
        private DropSlot slotPrefab;
        [SerializeField]
        private DraggableItem itemUIPrefab;

        public GameObject ItemPrefab => itemPrefab;
        public DropSlot SlotPrefab => slotPrefab;
        public DraggableItem ItemUIPrefab => itemUIPrefab;
    }
}
