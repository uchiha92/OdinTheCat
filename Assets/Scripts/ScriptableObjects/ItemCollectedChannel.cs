using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_ItemCollectedChannel",menuName = "Data/Channels/ItemCollectedChannel")]
    public class ItemCollectedChannel : ScriptableObject
    {
        public Action<EItemType, int> OnItemCollected;

        public void InvokeItemCollected(EItemType itemType, int value)
        {
            OnItemCollected?.Invoke(itemType, value);
        }
    }
}
