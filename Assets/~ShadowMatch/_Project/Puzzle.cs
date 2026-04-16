using System.Collections.Generic;
using UnityEngine;

namespace ShadowMatch
{
    public class Puzzle : MonoBehaviour
    {
        private bool hasNotifiedItemData;

        private void Awake()
        {
            hasNotifiedItemData = false;

            NotifyItemData();
        }

        private void Start()
        {
            NotifyItemData();
        }

        private void Update()
        {
            NotifyItemData();
        }

        private void NotifyItemData()
        {
            if (hasNotifiedItemData)
                return;
            
            DropSlot[] slots = GetComponentsInChildren<DropSlot>(true);
            List<ItemData> itemDataList = new();

            if (slots != null)
            {
                itemDataList.Capacity = slots.Length;

                foreach (DropSlot slot in slots)
                {
                    if (slot.ItemData != null)
                        itemDataList.Add(slot.ItemData);
                }
            }

            PuzzleManager.Instance.SetItemData(itemDataList);
            hasNotifiedItemData = true;
            enabled = false;
        }
    }
}
