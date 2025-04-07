using UnityEngine;
using UnityEngine.SceneManagement;

public class GameConsole : MonoBehaviour
{
    public Canvas interactionCanvas;
    public string gameSceneName = "Game";
    public Transform player;
    public float interactRadius = 2f;

    private bool canInteract = false;

    void Start()
    {
        interactionCanvas.enabled = false;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= interactRadius)
        {
            if (!canInteract)
            {
                canInteract = true;
                interactionCanvas.enabled = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(gameSceneName);
            }
        }
        else
        {
            if (canInteract)
            {
                canInteract = false;
                interactionCanvas.enabled = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
