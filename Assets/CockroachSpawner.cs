using System.Collections;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.IO;
using System.Collections.Generic;
public class CockroachSpawnerUI : MonoBehaviour
{
    public GameObject cockroachPrefab;  // Prefab gián (UI)
    public RectTransform spawnArea;     // RectTransform vùng spawn
    public float spawnInterval = 1f;    // Thời gian giữa mỗi lần spawn
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
        int currentSanity = LoadHungerData(); // Mặc định là 4 nếu không tìm thấy dữ liệu
        currentSanity = Mathf.Max(currentSanity + 1, 0); // Đảm bảo Sanity không dưới 0
        SaveHungerData(currentSanity); // Lưu lại giá trị mới

        // Debug để kiểm tra
        Debug.Log("Hunger after eating: " + currentSanity);
        if (cockroachPrefab == null || spawnArea == null)
        {
            Debug.LogError("Prefab hoặc vùng spawn chưa được gán trong Inspector");
            return;
        }

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnCockroach();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCockroach()
    {
        // Lấy size của vùng spawn
        Vector2 size = spawnArea.rect.size;

        // Random vị trí local trong vùng
        float x = Random.Range(-size.x / 2, size.x / 2);
        float y = Random.Range(-size.y / 2, size.y / 2);
        Vector2 localPos = new Vector2(x, y);

        // Spawn gián
        GameObject cockroach = Instantiate(cockroachPrefab, spawnArea);
        cockroach.GetComponent<RectTransform>().localPosition = localPos;
    }
    void SaveHungerData(int sanity)
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
    int LoadHungerData()
    {
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Đọc dữ liệu từ tệp và trả về giá trị Sanity
                sanity = int.Parse(reader.ReadLine());
                hunger = int.Parse(reader.ReadLine());
                money = int.Parse(reader.ReadLine());
                day = int.Parse(reader.ReadLine());

                return sanity;
            }
        }
        else
        {
            // Nếu tệp không tồn tại, trả về giá trị mặc định là 4
            return 4;
        }
    }
}
