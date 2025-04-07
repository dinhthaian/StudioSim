using UnityEngine;
using UnityEngine.SceneManagement;

public class EatingTable : MonoBehaviour
{
    public Canvas interactionCanvas;
    public string eatSceneName = "Eat";
    public Transform player;
    public float interactRadius = 2f;

    private bool canInteract = false;

    void Start()
    {
        interactionCanvas.enabled = false;

        // Đăng ký sự kiện khi scene được load
        SceneManager.sceneLoaded += OnSceneLoaded;
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
                SceneManager.LoadScene(eatSceneName);
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }

    // Khi scene được load, giảm Sanity 1
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }
}
