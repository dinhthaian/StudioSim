using UnityEngine;
using TMPro;
using UnityEngine.UI; // Thêm để sử dụng Image UI

public class DayCycle : MonoBehaviour
{
    // Enum để định nghĩa các buổi trong ngày
    public enum TimeOfDay
    {
        Morning,
        Afternoon,
        Night
    }

    public static TimeOfDay currentTime;  // Lưu trữ buổi hiện tại
    public static int Day = 1;             // Đếm ngày
    public static int energy = 1;          // Lưu trữ số Energy
    public TextMeshProUGUI timeText;       // Text Mesh Pro để hiển thị giờ

    public Image backgroundImage;          // Thêm Image UI cho background
    public Color morningColor = Color.white;
    public Color afternoonColor = Color.yellow;
    public Color nightColor = Color.black;

    // Start is called before the first frame update
    void Start()
    {
        // Lấy giá trị energy từ PlayerPrefs khi bắt đầu
        energy = PlayerPrefs.GetInt("Energy", 1); // Nếu không có giá trị trong PlayerPrefs, sẽ gán mặc định là 1

        // Kiểm tra Energy, nếu Energy = 0 thì chuyển buổi mới
        if (energy == 0)
        {
            NextTime(); // Chuyển sang buổi tiếp theo
        }

        UpdateBackgroundColor();
        UpdateTimeText();
    }

    // Update is called once per frame
    void Update()
    {
        // Kiểm tra Energy, khi Energy = 0 thì chuyển buổi mới
        if (energy <= 0)
        {
            NextTime(); // Chuyển sang buổi tiếp theo
        }
    }

    // Cập nhật màu sắc background theo buổi
    void UpdateBackgroundColor()
    {
        if (currentTime == TimeOfDay.Morning)
            backgroundImage.color = morningColor; // Buổi sáng
        else if (currentTime == TimeOfDay.Afternoon)
            backgroundImage.color = afternoonColor; // Buổi chiều
        else if (currentTime == TimeOfDay.Night)
            backgroundImage.color = nightColor; // Buổi tối
    }

    // Cập nhật thời gian hiển thị trên màn hình
    void UpdateTimeText()
    {
        if (timeText != null)
        {
            if (currentTime == TimeOfDay.Morning)
                timeText.text = "6AM";
            else if (currentTime == TimeOfDay.Afternoon)
                timeText.text = "4PM";
            else if (currentTime == TimeOfDay.Night)
                timeText.text = "10PM";
        }
    }

    // Hàm này sẽ chuyển qua buổi tiếp theo và cập nhật thông tin
    public static void NextTime()
    {
        // Chuyển qua buổi tiếp theo
        switch (currentTime)
        {
            case TimeOfDay.Morning:
                currentTime = TimeOfDay.Afternoon;
                break;
            case TimeOfDay.Afternoon:
                currentTime = TimeOfDay.Night;
                break;
            case TimeOfDay.Night:
                currentTime = TimeOfDay.Morning;
                Day++; // Qua ngày mới

                break;
        }

        // Reset Energy về 1 sau khi chuyển buổi
        energy = 1;

        // Lưu giá trị energy vào PlayerPrefs để sử dụng ở lần sau
        PlayerPrefs.SetInt("Energy", energy);
        PlayerPrefs.Save();
        
    }
}
