using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PreGameUI preGameUi;
    public InGameUI inGameUi;
    public PostGameUI postGameUi;

    private void Awake()
    {
        preGameUi.Init(this);
        inGameUi.Init(this);
        postGameUi.Init(this);
    }


    public void ShowUI(GameState gameState)
    {
        preGameUi.SetActive(gameState);
        inGameUi.SetActive(gameState);
        postGameUi.SetActive(gameState);
    }
}
