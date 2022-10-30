using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_ItemCollectedChannel",menuName = "Data/Channels/ItemCollectedChannel")]
public class ItemCollectedChannel : ScriptableObject
{
    public Action<EItemType> OnItemCollected;

    public void InvokeItemCollected(EItemType itemType)
    {
        OnItemCollected?.Invoke(itemType);
    }
}
