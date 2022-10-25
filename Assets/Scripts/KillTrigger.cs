using System;
using UnityEngine;
using UnityEngine.Events;

public class KillTrigger : MonoBehaviour
{
   [SerializeField]
   private KillPlayerChannel killPlayerChannel;
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         KillPlayer();
      }
   }

   private void KillPlayer()
   {
      killPlayerChannel.InvokeOnDead();
   }
}
