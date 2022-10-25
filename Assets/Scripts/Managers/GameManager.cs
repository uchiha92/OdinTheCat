using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private EGameState _gameState;
    [SerializeField]
    private Canvas _menuCanvas;
    [SerializeField]
    private Canvas _gameCanvas;
    [SerializeField]
    private Canvas _gameOverCanvas;
    [SerializeField]
    private GameStateChannel gameStateChannel;

    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    void Start()
    {
        _menuCanvas.enabled = true;
        _gameCanvas.enabled = false;
        _gameOverCanvas.enabled = false;
        SetGameState(EGameState.Menu);
    }

    public void StartGame()
    {
        _menuCanvas.enabled = false;
        _gameCanvas.enabled = true;
        _gameOverCanvas.enabled = false;
        SetGameState(EGameState.InTheGame);
    }

    public void GameOver()
    {
        _menuCanvas.enabled = false;
        _gameCanvas.enabled = false;
        _gameOverCanvas.enabled = true;
        SetGameState(EGameState.GameOver);
    }
    
    private void BackToMainMenu()
    {
        SetGameState(EGameState.Menu);
    }

    private void SetGameState(EGameState gameState)
    {
        this._gameState = gameState;
        this.gameStateChannel.InvokeOnChangeGameState(gameState);
    }

    public EGameState GetGameState()
    {
        return this._gameState;
    }

   
}
