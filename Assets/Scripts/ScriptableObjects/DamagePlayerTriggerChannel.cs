using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_DamagePlayerTriggerChannel",menuName = "Data/Channels/DamagePlayerTriggerChannel")]
    public class DamagePlayerTriggerChannel : ScriptableObject
    {
        public Action OnDamagePlayer;

        public void InvokeOnDamagePlayer()
        {
            OnDamagePlayer?.Invoke();
        }
    }
}
