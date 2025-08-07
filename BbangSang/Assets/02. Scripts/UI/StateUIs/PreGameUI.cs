using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGameUI : BaseGameUI
{
    protected override GameState GetUIState()
    {
        return GameState.PreGame;
    }

    private void Awake()
    {
        nextStateButton.onClick.AddListener(this.ShowNextState);
    }
}
