using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_HealthPlayerChannel",menuName = "Data/Channels/HealthPlayerChannel")]
public class HealthPlayerChannel : ScriptableObject
{
    public Action OnHealthDecrease;
    public Action<int> OnHealthIncrease;

    public void InvokeOnHealthDecrease()
    {
        OnHealthDecrease?.Invoke();
    }

    public void InvokeOnHealthIncrease(int value)
    {
        OnHealthIncrease?.Invoke(value);
    }
}
