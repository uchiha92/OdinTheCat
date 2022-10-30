using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_GameStateChannel",menuName = "Data/Channels/GameStateChannel")]
public class GameStateChannel : ScriptableObject
{
    public Action<EGameState> OnChangeGameState;

    public void InvokeOnChangeGameState(EGameState newGameState)
    {
        OnChangeGameState?.Invoke(newGameState);
    }
}