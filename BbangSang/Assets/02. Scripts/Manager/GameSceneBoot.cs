using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneBoot : MonoBehaviour
{
    [SerializeField] private bool saveAfterLoad = true;

    private void Start()
    {
        var gm = GameManager.Instance;
        var player = gm?.Player;

        if (player == null)
        {
            return;
        }

        if (gm.IsNewGame)
        {
            player.CreateDataWithName(gm.PendingNewPlayerName);
            player.Save();

            gm.IsNewGame = false;
            gm.PendingNewPlayerName = null;
        }
        else
        {
            player.Load();
            player.Save();
        }

        var p = GameManager.Instance.Player;
        if (p != null && p.playerData != null)
            Debug.Log($"[SAVE CHECK] Player name = {p.playerData.name}");
    }
}
