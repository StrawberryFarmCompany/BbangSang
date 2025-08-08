using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    Title,
    PreGame, // 빵 만드는 시간
    InGame, // 빵 파는 시간
    PostGame // 정산 시간
}

public class GameManager : MonoBehaviour
{
    public GameState CurGameState { get; set; } = GameState.Title;

    public float curInGameTime;
    public float maxInGameTime = 300f; // 5분

    public bool IsNewGame { get; set; } = false;  //새로하기 눌렀는지 판단
    public string PendingNewPlayerName { get; set; } = null;  //입력한 이름

    private static GameManager _instance; 
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("GameManager");
                _instance = obj.AddComponent<GameManager>();                
            }

            return _instance;
        }
    }

    public Player player;
    public Player Player {get; set;}

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if(CurGameState == GameState.InGame)
        {
            curInGameTime += Time.deltaTime;
            if (curInGameTime >= maxInGameTime)
            {
                curInGameTime = 0f;
                CurGameState = GameState.PostGame;
                Debug.Log("하루 끝");
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
                // 게임 시작할 때 시간 초기화
                curInGameTime = 0f;
                return GameState.PostGame;
            case GameState.PostGame:
                return GameState.PreGame;
            default:
                return GameState.Title;
        }
    }
}
