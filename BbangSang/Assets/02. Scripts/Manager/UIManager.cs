using System;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public PreGameUI preGameUi;
    public InGameUI inGameUi;
    public PostGameUI postGameUi;
    public GameObject escapeUi;

    private void Awake()
    {
        preGameUi.Init(this);
        inGameUi.Init(this);
        postGameUi.Init(this);
    }

    private void Start()
    {
        GameManager.Instance.CurGameState = GameState.PreGame;
        ShowUI(GameManager.Instance.CurGameState);
    }


    public void ShowUI(GameState gameState)
    {
        preGameUi.SetActive(gameState);
        inGameUi.SetActive(gameState);
        postGameUi.SetActive(gameState);
    }

    public void ToggleEscape()
    {
        escapeUi.SetActive(!escapeUi.activeSelf);
        if (escapeUi.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
