using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Started game");
        SceneManager.LoadScene(0);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // change to game
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quited game");
    }
}
