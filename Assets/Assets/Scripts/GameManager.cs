using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton instance
    public static GameManager instance;

    //Enum for the directions in the rhythm minigame
    public enum DirEnum
    {
        DirUp,
        DirRight,
        DirLeft,
        DirDown,
        DirLast
    }

    int score = 0;
    float life = 100f;

    bool gameEnded = false;


    //The two players rhythm games managers
    [SerializeField]
    public RhythmMinigameManager player1RhythmManager;
    //The two players rhythm games managers
    [SerializeField]
    public RhythmMinigameManager player2RhythmManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetGame() 
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(life <= 0 && !gameEnded)
        {
            player1RhythmManager.LoseGame();
            player2RhythmManager.LoseGame();
            gameEnded = true;
        }
    }

    public void AddScore(int score) 
    { 
        this.score += score;
    }

    public void AddLife(float life)
    {
        this.life += life;
        Debug.Log(life);
    }

    public void EndGame()
    { 
        
    }
}
