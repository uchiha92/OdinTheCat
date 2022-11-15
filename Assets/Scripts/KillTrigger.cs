using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

public class KillTrigger : MonoBehaviour
{
   [SerializeField] 
   private AudioClip _deadSound;
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
      AudioSource.PlayClipAtPoint(_deadSound, transform.position);
      killPlayerChannel.InvokeOnDead();
   }
}
