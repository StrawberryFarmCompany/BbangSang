using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseGameUI : MonoBehaviour
{
    public Button nextStateButton;

    private UIManager uiManager;

    public void Init(UIManager uiManager)
    {
        this.uiManager = uiManager;
    }

    protected abstract GameState GetUIState();

    public void SetActive(GameState gameState)
    {
        gameObject.SetActive(GetUIState() == gameState);
    }

    public void ShowNextState()
    {
        Debug.Log($"{GetUIState()}");
        GameManager.Instance.CurGameState = GameManager.Instance.GetNextState(GetUIState());
        uiManager.ShowUI(GameManager.Instance.CurGameState);
    }
}
