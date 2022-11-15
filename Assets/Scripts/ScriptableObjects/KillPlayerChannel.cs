using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_KillPlayerChannel",menuName = "Data/Channels/KillPlayerChannel")]
    public class KillPlayerChannel : ScriptableObject
    {
        public Action OnDead;

        public void InvokeOnDead()
        {
            OnDead?.Invoke();
        }
    }
}
