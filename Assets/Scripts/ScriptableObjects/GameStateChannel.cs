using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_GameStateChannel",menuName = "ScriptableObjects/GameState/GameStateChannel")]
public class GameStateChannel : ScriptableObject
{
    public Action<EGameState> OnChangeGameState;

    public void InvokeOnChangeGameState(EGameState newGameState)
    {
        OnChangeGameState?.Invoke(newGameState);
    }
}