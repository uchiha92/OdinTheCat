using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState
{
    Menu,
    InTheGame,
    GameOver
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private EGameState _gameState;
    [SerializeField]
    private Canvas _menuCanvas;
    public event EventHandler OnReset;

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
        SetGameState(EGameState.Menu);
    }

    private void Update()
    {
        
       /* if (Input.GetButtonDown("Fire1") && !_gameState.Equals(EGameState.InTheGame))
        {
            OnReset?.Invoke(this, EventArgs.Empty);
            StartGame();
        }*/
    }
    
    public void StartGame()
    {
        Debug.Log("juego empezado");
        _menuCanvas.enabled = false;
        OnReset?.Invoke(this, EventArgs.Empty);
        SetGameState(EGameState.InTheGame);
        Debug.Log(GetGameState());
    }
    
    public void GameOver()
    {
        _menuCanvas.enabled = false;
        SetGameState(EGameState.GameOver);
    }
    
    private void BackToMainMenu()
    {
        SetGameState(EGameState.Menu);
    }

    private void SetGameState(EGameState gameState)
    {
        this._gameState = gameState;
    }

    public EGameState GetGameState()
    {
        return this._gameState;
    }

   
}
