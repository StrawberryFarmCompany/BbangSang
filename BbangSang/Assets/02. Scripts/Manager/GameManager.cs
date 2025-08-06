using UnityEngine;

public class GameManager : MonoBehaviour
{
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

}
