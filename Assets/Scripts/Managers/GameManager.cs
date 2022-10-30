using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private EGameState _gameState;
    private int _collectedCoins;
    private float _finalGameScore;
    [SerializeField]
    private Canvas _menuCanvas;
    [SerializeField]
    private Canvas _gameCanvas;
    [SerializeField]
    private Canvas _gameOverCanvas;
    [SerializeField]
    private GameStateChannel _gameStateChannel;
    [SerializeField]
    private ItemCollectedChannel _itemCollectedChannel;

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

    private void Start()
    {
        EnableMenuCanvas();
        SetGameState(EGameState.Menu);
    }

    private void OnDestroy()
    {
        this._itemCollectedChannel.OnItemCollected -= OnItemCollected;
    }

    public void StartGame()
    {
        this._itemCollectedChannel.OnItemCollected += OnItemCollected;
        LevelGenerator.Instance.GenerateInitialBlocks();
        EnableGameCanvas();
        InitCoins();
        SetGameState(EGameState.InTheGame);
    }

    public void GameOver()
    {
        this._itemCollectedChannel.OnItemCollected -= OnItemCollected;
        LevelGenerator.Instance.RemoveAllTheBlocks();
        EnableGameOverCanvas();
        SetGameState(EGameState.GameOver);
    }
    
    public void BackToMainMenu()
    {
        EnableMenuCanvas();
        SetGameState(EGameState.Menu);
    }

    private void EnableGameCanvas()
    {
        this._menuCanvas.enabled = false;
        this._gameCanvas.enabled = true;
        this._gameOverCanvas.enabled = false;
    }

    private void EnableGameOverCanvas()
    {
        this._menuCanvas.enabled = false;
        this._gameCanvas.enabled = false;
        this._gameOverCanvas.enabled = true;
    }

    private void EnableMenuCanvas()
    {
        this._menuCanvas.enabled = true;
        this._gameCanvas.enabled = false;
        this._gameOverCanvas.enabled = false;
    }

    private void OnItemCollected(EItemType itemType)
    {
        if (itemType.Equals(EItemType.Coin))
        {
            CollectCoin();
        }
    }
    
    private void InitCoins()
    {
        this._collectedCoins = 0;
    }

    private void CollectCoin()
    {
        ++this._collectedCoins;
        Debug.Log(_collectedCoins);
    }

    public int GetCollectedCoins()
    {
        return this._collectedCoins;
    }

    public float GetFinalGameScore()
    {
        return this._finalGameScore;
    }

    public void SetFinalGameScore(float finalGameScore)
    {
        this._finalGameScore = finalGameScore;
    }

    private void SetGameState(EGameState gameState)
    {
        this._gameState = gameState;
        this._gameStateChannel.InvokeOnChangeGameState(gameState);
    }

    public EGameState GetGameState()
    {
        return this._gameState;
    }
}
