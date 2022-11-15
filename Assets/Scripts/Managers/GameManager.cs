using ScriptableObjects;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private EGameState _gameState;
    private int _collectedCoins;
    private float _finalGameScore;
    [SerializeField]
    private Canvas menuCanvas;
    [SerializeField]
    private Canvas gameCanvas;
    [SerializeField]
    private Canvas gameOverCanvas;
    [SerializeField]
    private GameStateChannel gameStateChannel;
    [SerializeField]
    private ItemCollectedChannel itemCollectedChannel;

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
        itemCollectedChannel.OnItemCollected -= OnItemCollected;
    }

    public void StartGame()
    {
        itemCollectedChannel.OnItemCollected += OnItemCollected;
        LevelGenerator.Instance.GenerateInitialBlocks();
        EnableGameCanvas();
        InitCoins();
        SetGameState(EGameState.InTheGame);
    }

    public void GameOver()
    {
        itemCollectedChannel.OnItemCollected -= OnItemCollected;
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
        menuCanvas.enabled = false;
        menuCanvas.GetComponent<AudioSource>().Stop();
        gameCanvas.enabled = true;
        gameCanvas.GetComponent<AudioSource>().Play();
        gameOverCanvas.enabled = false;
        gameOverCanvas.GetComponent<AudioSource>().Stop();
    }

    private void EnableGameOverCanvas()
    {
        menuCanvas.enabled = false;
        menuCanvas.GetComponent<AudioSource>().Stop();
        gameCanvas.enabled = false;
        gameCanvas.GetComponent<AudioSource>().Stop();
        gameOverCanvas.enabled = true;
        gameOverCanvas.GetComponent<AudioSource>().Play();
        
    }

    private void EnableMenuCanvas()
    {
        menuCanvas.enabled = true;
        menuCanvas.GetComponent<AudioSource>().Play();
        gameCanvas.enabled = false;
        gameCanvas.GetComponent<AudioSource>().Stop();
        gameOverCanvas.enabled = false;
        gameOverCanvas.GetComponent<AudioSource>().Stop();
    }

    private void InitCoins()
    {
        _collectedCoins = 0;
    }

    private void OnItemCollected(EItemType itemType, int value)
    {
        switch (itemType)
        {
            case EItemType.Money:
                _collectedCoins += value;
                break;
            case EItemType.Health:
                break;
            default:
                break;
        }
    }

    public int GetCollectedCoins()
    {
        return _collectedCoins;
    }

    public float GetFinalGameScore()
    {
        return _finalGameScore;
    }

    public void SetFinalGameScore(float finalGameScore)
    {
        _finalGameScore = finalGameScore;
    }

    private void SetGameState(EGameState gameState)
    {
        _gameState = gameState;
        gameStateChannel.InvokeOnChangeGameState(gameState);
    }

    public EGameState GetGameState()
    {
        return _gameState;
    }
}
