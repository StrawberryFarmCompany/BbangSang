using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneBoot : MonoBehaviour
{
    [SerializeField] private bool saveAfterLoad = true;
    [SerializeField] private float waitUpToSeconds = 2f;

    private IEnumerator Start()
    {
        var gm = GameManager.Instance;

        // Player 등록될 때까지 최대 waitUpToSeconds 대기
        float t = 0f;
        while (gm.Player == null && t < waitUpToSeconds)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        var player = gm.Player;
        if (player == null)
        {
            Debug.LogWarning("[BOOT] Player가 없어 초기화/로드를 건너뜁니다.");
            yield break;
        }

        if (gm.IsNewGame)
        {
            player.CreateDataWithName(gm.PendingNewPlayerName);
            player.Save();
            Debug.Log($"[NEW GAME] name={player.playerData.name}");
            gm.IsNewGame = false;
            gm.PendingNewPlayerName = null;
        }
        else
        {
            player.Load();
            if (saveAfterLoad) player.Save();
            Debug.Log($"[LOAD GAME] name={player.playerData?.name}");
        }
    }
}
