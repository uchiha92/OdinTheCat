using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerTrigger : MonoBehaviour
{
    [SerializeField] 
    private DamagePlayerTriggerChannel damagePlayerTriggerChannel;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            damagePlayerTriggerChannel.InvokeOnDamagePlayer();
        }
    }
}
