using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_HealthPlayerChannel",menuName = "Data/Channels/HealthPlayerChannel")]
    public class HealthPlayerChannel : ScriptableObject
    {
        public Action OnHealthDecrease;
        public Action<int> OnHealthIncrease;
        public Action OnLowHealth;
        public Action OnHighHealth;

        public void InvokeOnHealthDecrease()
        {
            OnHealthDecrease?.Invoke();
        }

        public void InvokeOnHealthIncrease(int value)
        {
            OnHealthIncrease?.Invoke(value);
        }

        public void InvokeOnLowHealth()
        {
            OnLowHealth?.Invoke();
        }
    
        public void InvokeOnHighHealth()
        {
            OnHighHealth?.Invoke();
        }
    }
}
