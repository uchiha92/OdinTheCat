using System;
using UnityEngine;
using UnityEngine.Events;

public class KillTrigger : MonoBehaviour
{
   [SerializeField]
   //private UnityEvent<KillTrigger> m_OnDead;
   public event EventHandler OnDead;
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         KillPlayer();
      }
   }

   private void KillPlayer()
   {
      OnDead?.Invoke(this, EventArgs.Empty);
      //m_OnDead?.Invoke(this);
   }
}
