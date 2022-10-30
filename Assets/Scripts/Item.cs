using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private EItemType _itemType;
    [SerializeField]
    private ItemCollectedChannel _itemCollectedChannel;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            this._itemCollectedChannel.InvokeItemCollected(this._itemType);
            Destroy(gameObject);
        }
    }
}
