using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private bool _isCollected;
    [SerializeField]
    private int _value;
    [SerializeField] 
    private AudioClip _itemSound;
    [SerializeField]
    private EItemType _itemType;
    [SerializeField]
    private ItemCollectedChannel _itemCollectedChannel;

    private void Awake()
    {
        this._isCollected = false;
    }

    private void Show()
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<CircleCollider2D>().enabled = true;
        this._isCollected = false;
    }

    private void Hide()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<CircleCollider2D>().enabled = false;
    }

    void Collect()
    {
        if(this._itemType != EItemType.Health || PlayerController.Instance.GetHealthPlayer() < PlayerController.Instance.GetMaxHealth()){}
        Hide();
        this._isCollected = true;
        this._itemCollectedChannel.InvokeItemCollected(this._itemType, this._value);
        AudioSource.PlayClipAtPoint(this._itemSound, gameObject.transform.position, 1f);
    }

    public int GetValue()
    {
        return this._value;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        bool canCollect = this._itemType != EItemType.Health ||
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
