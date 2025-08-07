using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGameUI : BaseGameUI
{
    protected override GameState GetUIState()
    {
        return GameState.PostGame;
    }

    private void Awake()
    {
        nextStateButton.onClick.AddListener(this.ShowNextState);
    }
}
