using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void NewGame()
    {
        SceneManager.LoadScene("Spiders");
    }

    // Update is called once per frame
    public void ContinueGame()
    {
        //Write the logic to find the load file!
        SceneManager.LoadScene("Spiders");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }
}
