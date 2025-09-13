using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public int GameSceneNum;

    public void StartGame()
    {
        Debug.Log("Started game");
        SceneManager.LoadScene(GameSceneNum);
    } 

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quited game");
    }
}
