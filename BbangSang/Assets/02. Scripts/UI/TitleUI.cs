using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    public Button nextSceneButton;


    private void Awake()
    {
        nextSceneButton.onClick.AddListener(GoToGameScene);
    }

    void GoToGameScene()
    {
        GameManager.Instance.CurGameState = GameManager.Instance.GetNextState(GameState.Title);
        SceneManager.LoadScene("KitchenScene");
    }
}
