using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


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

    private void Awake()
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
        this._menuCanvas.GetComponent<AudioSource>().Stop();
        this._gameCanvas.enabled = true;
        this._gameCanvas.GetComponent<AudioSource>().Play();
        this._gameOverCanvas.enabled = false;
        this._gameOverCanvas.GetComponent<AudioSource>().Stop();
    }

    private void EnableGameOverCanvas()
    {
        this._menuCanvas.enabled = false;
        this._menuCanvas.GetComponent<AudioSource>().Stop();
        this._gameCanvas.enabled = false;
        this._gameCanvas.GetComponent<AudioSource>().Stop();
        this._gameOverCanvas.enabled = true;
        this._gameOverCanvas.GetComponent<AudioSource>().Play();
        
    }

    private void EnableMenuCanvas()
    {
        this._menuCanvas.enabled = true;
        this._menuCanvas.GetComponent<AudioSource>().Play();
        this._gameCanvas.enabled = false;
        this._gameCanvas.GetComponent<AudioSource>().Stop();
        this._gameOverCanvas.enabled = false;
        this._gameOverCanvas.GetComponent<AudioSource>().Stop();
    }

    private void InitCoins()
    {
        this._collectedCoins = 0;
    }

    private void OnItemCollected(EItemType itemType, int value)
    {
        switch (itemType)
        {
            case EItemType.Money:
                this._collectedCoins += value;
                break;
            case EItemType.Health:
                break;
            default:
                break;
        }
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
