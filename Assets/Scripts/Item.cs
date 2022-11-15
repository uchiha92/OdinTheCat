using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private int value;
    [SerializeField] 
    private AudioClip itemSound;
    [SerializeField]
    private EItemType itemType;
    [SerializeField]
    private ItemCollectedChannel itemCollectedChannel;

    private void Show()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
    }

    private void Hide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
    }

    void Collect()
    {
        if (itemType != EItemType.Health ||
            PlayerController.Instance.GetHealthPlayer() < PlayerController.Instance.GetMaxHealth())
        {
            Hide();
            itemCollectedChannel.InvokeItemCollected(itemType, value);
            AudioSource.PlayClipAtPoint(itemSound, gameObject.transform.position, 1f);
        }
    }

    public int GetValue()
    {
        return value;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        bool canCollect = itemType != EItemType.Health ||
                          PlayerController.Instance.GetHealthPlayer() < PlayerController.Instance.GetMaxHealth();
        if (other.tag.Equals("Player"))
        {
            if (canCollect)
            {
                Collect();
            }
        }
    }
}
