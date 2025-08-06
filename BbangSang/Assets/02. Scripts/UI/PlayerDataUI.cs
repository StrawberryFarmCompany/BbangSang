using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataUI : MonoBehaviour
{
    public TextMeshProUGUI nicknameText;
    public Button saveButton;
    public Button loadButton;

    public void OnClickSave()
    {
        if(GameManager.Instance.Player == null)
        {
            Debug.LogError("Player data is not initialized.");
        }
        else
        {
            GameManager.Instance.Player.Save();
        }
    }

    public void OnClickLoad()
    {
        if (GameManager.Instance.Player == null)
        {
            Debug.LogError("Player data is not initialized.");
        }
        else
        {
            GameManager.Instance.Player.Load();
        }

        nicknameText.text = GameManager.Instance.Player.playerData.name;
    }
}
