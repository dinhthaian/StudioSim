using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MENU : MonoBehaviour
{
    public Image backgroundImage;        // drag cái Background vào đây
    public Sprite[] backgroundSprites;   // kéo nhiều background vào array này
    private int currentIndex = 0;

    public void StartGame()
    {
        SceneManager.LoadScene("Room");
    }

    public void Option()
    {
        currentIndex++;
        if (currentIndex >= backgroundSprites.Length)
            currentIndex = 0;

        backgroundImage.sprite = backgroundSprites[currentIndex];
        Debug.Log("Background changed!");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
}
