using System;
using TMPro;
using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] private GameObject xYouText;
    [SerializeField] private GameObject oYouText;
    [SerializeField] private GameObject xArrow;
    [SerializeField] private GameObject oArrow;

    [Header("Score Text")]
    [SerializeField] private TextMeshProUGUI xScoreText;
    [SerializeField] private TextMeshProUGUI oScoreText;

    private void Start()
    {
        GameManager.Instance.OnGameStarted += GameManager_OnOnGameStarted;
        GameManager.Instance.OnTurnChanged += GameManager_OnTurnChanged;
        GameManager.Instance.xScore.OnValueChanged += HandleXScoreChange;
        GameManager.Instance.oScore.OnValueChanged += HandleOScoreChange;
    }

    private void HandleXScoreChange(int previousValue, int newValue)
    {
        xScoreText.text = newValue.ToString();
    }

    private void HandleOScoreChange(int previousValue, int newValue)
    {
        oScoreText.text = newValue.ToString();
    }

    private void GameManager_OnOnGameStarted()
    {
        if (GameManager.Instance.GetLocalPlayerType() == PlayerType.X)
        {
            xYouText.SetActive(true);
        }
        else
        {
            oYouText.SetActive(true);
        }
        
        GameManager_OnTurnChanged(PlayerType.X);
    }
    
    private void GameManager_OnTurnChanged(PlayerType _playerType)
    {
        if (_playerType == PlayerType.X)
        {
            xArrow.SetActive(true);
            oArrow.SetActive(false);
        }
        else if(_playerType == PlayerType.O)
        {
            xArrow.SetActive(false);
            oArrow.SetActive(true);
        }
        else
        {
            xArrow.SetActive(false);
            oArrow.SetActive(false);
        }
    }
    
}
