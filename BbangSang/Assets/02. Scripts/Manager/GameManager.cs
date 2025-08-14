using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    Title,
    PreGame, 
    InGame, 
    PostGame 
}

public class GameManager : Singleton<GameManager>
{
    public GameState CurGameState { get; set; } = GameState.Title;

    public float curInGameTime;
    public float maxInGameTime = 300f; 

    public bool IsNewGame { get; set; } = false;  
    public string PendingNewPlayerName { get; set; } = null;  


    public Player player;
    public Player Player {get; set;}



    public void Update()
    {
        if(CurGameState == GameState.InGame)
        {
            curInGameTime += Time.deltaTime;
            if (curInGameTime >= maxInGameTime)
            {
                curInGameTime = 0f;
                CurGameState = GameState.PostGame;
                Debug.Log("�Ϸ� ��");
            }
        }
    }

    public GameState GetNextState(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.Title: 
                return GameState.PreGame;
            case GameState.PreGame:
                return GameState.InGame;
            case GameState.InGame:
                // ���� ������ �� �ð� �ʱ�ȭ
                curInGameTime = 0f;
                return GameState.PostGame;
            case GameState.PostGame:
                return GameState.PreGame;
            default:
                return GameState.Title;
        }
    }
}
