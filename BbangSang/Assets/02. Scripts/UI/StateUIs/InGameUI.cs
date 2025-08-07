using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : BaseGameUI
{
    public TextMeshProUGUI remainTimeText;

    protected override GameState GetUIState()
    {
        return GameState.InGame;
    }

    private void Update()
    {
        float remainTime = GameManager.Instance.maxInGameTime - GameManager.Instance.curInGameTime;
        int min = (int)(remainTime / 60);
        int sec = (int)(remainTime % 60);

        string timeString = string.Format($"{min:00}:{sec:00}");
        remainTimeText.text = timeString;
    }


    private void Awake()
    {
        nextStateButton.onClick.AddListener(this.ShowNextState);
    }
}
