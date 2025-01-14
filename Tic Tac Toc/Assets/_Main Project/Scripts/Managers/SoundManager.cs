using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;

    private AudioSource soundPlayer;

    private void Awake()
    {
        soundPlayer = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameManager.Instance.OnGameWin += GameManager_OnGameWin;
    }

    private void GameManager_OnGameWin(Vector2 arg1, float arg2, PlayerType _winPlayer)
    {
        if (GameManager.Instance.GetLocalPlayerType() == _winPlayer)
        {
            soundPlayer.PlayOneShot(winSound);
        }
        else if(_winPlayer == PlayerType.None)
        {
        }
        else
        {
            soundPlayer.PlayOneShot(loseSound);
        }
    }
}
