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
    private GameState curGameState = GameState.Title;
    public GameState CurGameState { get; set; }

    private float curInGameTime;
    private float maxInGameTime = 300f; // 5분

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

}
