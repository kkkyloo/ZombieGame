using UnityEngine;
using UnityEngine.SceneManagement;
public class OldMainMenu : MonoBehaviour
{
   public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ExitGame()
    {
        Debug.Log("���� ���������.");
        Application.Quit();
    }
}