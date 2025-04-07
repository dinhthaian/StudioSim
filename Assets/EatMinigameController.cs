using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.IO;

public class EatMinigameController : MonoBehaviour
{
    public Image foodImage;
    public Sprite[] healthyFoods;
    public Sprite[] unhealthyFoods;

    public TextMeshProUGUI timerText;
    public float gameDuration = 15f;
    private float currentTime;

    public Image playerImage; // Image nhân vật

    private bool isHealthy;
    private bool canClick = true;

    private string filePath;
    int hunger;
    int money;
    int day;
    int sanity;
    void Start()
    {
        // Đặt đường dẫn tới tệp playerData.txt trong thư mục AppData
        filePath = Path.Combine(Application.persistentDataPath, "playerData.txt");

        // Lấy giá trị Sanity từ playerData.txt
        int currentHunger = LoadSanityData(); // Mặc định là 4 nếu không tìm thấy dữ liệu
        currentHunger = Mathf.Max(currentHunger + 1, 0); // Đảm bảo Sanity không dưới 0
        SaveSanityData(currentHunger); // Lưu lại giá trị mới

        // Debug để kiểm tra
        Debug.Log("Sanity after eating: " + currentHunger);
        currentTime = gameDuration;
        NextFood();
        PlayerPrefs.SetInt("Energy", 0);
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        timerText.text = "" + Mathf.CeilToInt(currentTime);

        if (!canClick) return;

        if (Input.GetMouseButtonDown(0)) // Click trái
        {
            HandleChoice(isHealthy, Vector3.left);
        }
        else if (Input.GetMouseButtonDown(1)) // Click phải
        {
            HandleChoice(!isHealthy, Vector3.right);
        }

        if (currentTime <= 0)
        {
            SceneManager.LoadScene("Room");
        }
    }

    void HandleChoice(bool correct, Vector3 direction)
    {
        canClick = false;

        if (!correct)
        {
            StartCoroutine(HandleWrong());
        }

        StartCoroutine(MoveFood(direction));
    }

    IEnumerator MoveFood(Vector3 direction)
    {
        float moveTime = 0.3f;
        Vector3 startPos = foodImage.rectTransform.localPosition;
        Vector3 targetPos = startPos + direction * 1000f;

        float t = 0;
        while (t < moveTime)
        {
            t += Time.deltaTime;
            foodImage.rectTransform.localPosition = Vector3.Lerp(startPos, targetPos, t / moveTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        foodImage.rectTransform.localPosition = Vector3.zero;
        NextFood();
        canClick = true;
    }

    IEnumerator HandleWrong()
    {
        // Đỏ player
        playerImage.color = Color.red;
        yield return StartCoroutine(ShakeUI());  // Lắc UI (foodImage)

        playerImage.color = Color.white;
    }

    // Hàm lắc UI (foodImage và playerImage)
    IEnumerator ShakeUI()
    {
        float duration = 0.4f;
        float magnitude = 10f;

        Vector3 originalPos = foodImage.rectTransform.localPosition;  // Giữ vị trí ban đầu của foodImage
        Vector3 playerOriginalPos = playerImage.rectTransform.localPosition;  // Giữ vị trí ban đầu của playerImage

        float elapsed = 0f;
        while (elapsed < duration)
        {
            // Tạo độ lệch ngẫu nhiên cho foodImage và playerImage
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            foodImage.rectTransform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);  // Lắc foodImage
            playerImage.rectTransform.localPosition = playerOriginalPos + new Vector3(offsetX, offsetY, 0);  // Lắc playerImage

            elapsed += Time.deltaTime;
            yield return null;
        }

        foodImage.rectTransform.localPosition = originalPos;  // Trở về vị trí ban đầu của foodImage
        playerImage.rectTransform.localPosition = playerOriginalPos;  // Trở về vị trí ban đầu của playerImage
    }

    void NextFood()
    {
        if (Random.value < 0.5f)
        {
            isHealthy = true;
            foodImage.sprite = healthyFoods[Random.Range(0, healthyFoods.Length)];
        }
        else
        {
            isHealthy = false;
            foodImage.sprite = unhealthyFoods[Random.Range(0, unhealthyFoods.Length)];
        }
    }

    // Lưu dữ liệu Sanity vào playerData.txt
    void SaveSanityData(int hunger)
    {
        // Tạo thư mục nếu không tồn tại
        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Ghi các giá trị vào tệp
            writer.WriteLine(sanity);    // Dòng 1: Sanity
            writer.WriteLine(hunger);
            writer.WriteLine(money);
            writer.WriteLine(day);
        }

        Debug.Log("Data saved to: " + filePath);
    }

    // Đọc dữ liệu Sanity từ playerData.txt
    int LoadSanityData()
    {
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                
                sanity = int.Parse(reader.ReadLine());
                hunger = int.Parse(reader.ReadLine());
                money = int.Parse(reader.ReadLine());
                day = int.Parse(reader.ReadLine());
                return hunger;
            }
        }
        else
        {
            // Nếu tệp không tồn tại, trả về giá trị mặc định là 4
            return 4;
        }
    }
}
