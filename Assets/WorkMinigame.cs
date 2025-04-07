using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkMinigame : MonoBehaviour
{
    public Canvas interactionCanvas; // UI "Press E to Work"
    public string workSceneName;     // tên scene minigame
    public Transform player;         // reference player
    public float interactRadius = 2f; // bán kính tương tác

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
                SceneManager.LoadScene(workSceneName);
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

    // Optional: vẽ radius trong Scene view cho dễ canh chỉnh
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
