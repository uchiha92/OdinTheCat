using ScriptableObjects;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpdateGameOverCanvas : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI scorePoints;
    [SerializeField] 
    private TextMeshProUGUI coinsNumber;
    [SerializeField] 
    private Button playAgainButton, returnMenuButton;
    [SerializeField]
    private GameStateChannel gameStateChannel;

    private void Start()
    {
        gameStateChannel.OnChangeGameState += OnChangeGameState;
    }

    private void OnDestroy()
    {
        gameStateChannel.OnChangeGameState -= OnChangeGameState;
    }

    private void SetScorePointsAndCoins()
    {
        scorePoints.text = GameManager.Instance.GetFinalGameScore().ToString("f0");
        coinsNumber.text = GameManager.Instance.GetCollectedCoins().ToString();
    }
    
    private void DisableButtons()
    {
        playAgainButton.interactable = false;
        returnMenuButton.interactable = false;
    }
    
    private void EnableButtons()
    {
        playAgainButton.interactable = true;
        returnMenuButton.interactable = true;
        playAgainButton.Select();
    }
    
    private void OnChangeGameState(EGameState newGameState)
    {
        switch (newGameState)
        {
            case EGameState.InTheGame:
                DisableButtons();
                break;
            case EGameState.Menu:
                DisableButtons();
                break;
            case EGameState.GameOver:
                EnableButtons();
                SetScorePointsAndCoins();
                break;
        }
    }
}
