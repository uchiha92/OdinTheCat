using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpdateGameOverCanvas : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _scorePoints;
    [SerializeField] 
    private TextMeshProUGUI _coinsNumber;
    [SerializeField]
    private GameStateChannel _gameStateChannel;

    private void Start()
    {
        this._gameStateChannel.OnChangeGameState += OnChangeGameState;
    }

    private void OnDestroy()
    {
        this._gameStateChannel.OnChangeGameState -= OnChangeGameState;
    }

    private void SetScorePointsAndCoins()
    {
        this._scorePoints.text = GameManager.Instance.GetFinalGameScore().ToString("f0");
        this._coinsNumber.text = GameManager.Instance.GetCollectedCoins().ToString();
    }
    
    private void OnChangeGameState(EGameState newGameState)
    {
        switch (newGameState)
        {
            case EGameState.InTheGame:
                break;
            case EGameState.Menu:
                break;
            case EGameState.GameOver:
                SetScorePointsAndCoins();
                break;
            default:
                break;
        }
    }
}
