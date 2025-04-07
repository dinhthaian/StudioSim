using UnityEngine;
using UnityEngine.UI;  // Thêm thư viện để sử dụng Image
using System.IO;      // Thêm thư viện để đọc/ghi tệp

public class SanityCheck : MonoBehaviour
{
    public Image sanityImage;  // Hình ảnh thanh sanity
    public Image hungerImage;  // Hình ảnh thanh hunger
    public Sprite[] sanitySprites;  // Mảng các sprite của thanh sanity (0 - trống, 1 - đầy)
    public Sprite[] hungerSprites;  // Mảng các sprite của thanh hunger (0 - trống, 1 - đầy)
    int day;
    int money;
    private string filePath;

    void Start()
    {
        // Đặt đường dẫn tới tệp playerData.txt trong thư mục AppData
        filePath = Path.Combine(Application.persistentDataPath, "playerData.txt");

        // Lấy giá trị Sanity và Hunger từ playerData.txt
        int sanity = LoadSanityData(); // Mặc định là 4 nếu không tìm thấy dữ liệu
        int hunger = LoadHungerData(); // Mặc định là 4 nếu không tìm thấy dữ liệu

        // Gán sprite tương ứng dựa trên giá trị của Sanity
        if (sanity >= 4)
        {
            sanityImage.sprite = sanitySprites[4];  // Full
        }
        else if (sanity == 3)
        {
            sanityImage.sprite = sanitySprites[3];  // 3/4
        }
        else if (sanity == 2)
        {
            sanityImage.sprite = sanitySprites[2];  // 2/4
        }
        else if (sanity == 1)
        {
            sanityImage.sprite = sanitySprites[1];  // 1/4
        }
        else
        {
            sanityImage.sprite = sanitySprites[0];  // Nếu sanity == 0, vẫn để trống
        }

        // Gán sprite tương ứng dựa trên giá trị của Hunger
        if (hunger >= 4)
        {
            hungerImage.sprite = hungerSprites[4];  // Full
        }
        else if (hunger == 3)
        {
            hungerImage.sprite = hungerSprites[3];  // 3/4
        }
        else if (hunger == 2)
        {
            hungerImage.sprite = hungerSprites[2];  // 2/4
        }
        else if (hunger == 1)
        {
            hungerImage.sprite = hungerSprites[1];  // 1/4
        }
        else
        {
            hungerImage.sprite = hungerSprites[0];  // Nếu hunger == 0, vẫn để trống
        }
    }

    // Đọc dữ liệu Sanity từ playerData.txt
    int LoadSanityData()
    {
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Đọc dữ liệu từ tệp và trả về giá trị Sanity
                int sanity = int.Parse(reader.ReadLine());  // Dòng đầu tiên là Sanity
                reader.ReadLine(); // Bỏ qua Hunger
                reader.ReadLine(); // Bỏ qua Money
                reader.ReadLine(); // Bỏ qua Days Survived
                return sanity;
            }
        }
        else
        {
            // Nếu tệp không tồn tại, trả về giá trị mặc định là 4
            return 4;
        }
    }

    // Đọc dữ liệu Hunger từ playerData.txt
    int LoadHungerData()
    {
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.ReadLine(); // Bỏ qua Sanity
                int hunger = int.Parse(reader.ReadLine());  // Dòng thứ hai là Hunger
                money = int.Parse(reader.ReadLine()); // Bỏ qua Money
                day = int.Parse(reader.ReadLine()); // Bỏ qua Days Survived
                return hunger;
            }
        }
        else
        {
            // Nếu tệp không tồn tại, trả về giá trị mặc định là 4
            return 4;
        }
    }

    void Update()
    {
        // Bạn có thể thêm các logic khác vào đây nếu cần, như cập nhật thanh sanity hoặc hunger khi thay đổi
    }
}
