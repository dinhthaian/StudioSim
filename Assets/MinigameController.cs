using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MinigameController : MonoBehaviour
{
    public float timeLimit = 30f;            // Thời gian giới hạn
    public TextMeshProUGUI timeText;         // Text hiển thị thời gian
    public string backSceneName = "Room";    // Tên scene muốn quay về

    private float timer;

    void Start()
    {
        timer = timeLimit;
        PlayerPrefs.SetInt("Energy", 0);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // Update text
        if (timeText != null)
        {
            timeText.text = "" + Mathf.Ceil(timer).ToString();
        }

        if (timer <= 0f)
        {
            SceneManager.LoadScene(backSceneName);
        }
    }
}
