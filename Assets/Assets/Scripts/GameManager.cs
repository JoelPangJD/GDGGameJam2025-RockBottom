using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
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
    [SerializeField]
    public TextMeshProUGUI scoreText;
    [SerializeField]
    public TextMeshProUGUI lifeText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lifeText.text = "Life: " + this.life;

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
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
            EndGame();
        }
    }

    public void AddScore(int score) 
    { 
        this.score += score;
        scoreText.text = "Score: " + this.score;
    }

    public void AddLife(float life)
    {
        this.life += life;

        if(this.life > 100f)
            this.life = 100f;

        if (this.life < 0f)
            this.life = 0f;

        lifeText.text = "Life: " + this.life;
    }

    public void EndGame()
    {
        
        StartCoroutine(RestartIn(5.0f));
    }

    
    IEnumerator RestartIn(float time)
    {
        //Wait for 5 seconds before deactivating the note
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
