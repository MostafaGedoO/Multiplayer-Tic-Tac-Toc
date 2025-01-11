using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Color winColor;
    [SerializeField] private Color loseColor;
    [SerializeField] private Button reMatchButton;

    private void Awake()
    {
        reMatchButton.onClick.AddListener(() =>
        {
            GameManager.Instance.RematchRpc();
        });
    }
    
    private void Start()
    {
        GameManager.Instance.OnGameWin += GameManager_OnGameWin;
        GameManager.Instance.OnRematch += GameManager_OnRematch;
    }

    private void GameManager_OnRematch()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void GameManager_OnGameWin(Vector2 arg1, float arg2, PlayerType _WinPlayerType)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        
        if (_WinPlayerType == PlayerType.None)
        {
            gameOverText.color= Color.white;
            gameOverText.text = "Tie";
        }
        else if (_WinPlayerType == GameManager.Instance.GetLocalPlayerType())
        {
            gameOverText.color = winColor;
            gameOverText.text = "You Win!";
        }
        else
        {
            gameOverText.color = loseColor;
            gameOverText.text = "You Lose!";
        }
    }
}
