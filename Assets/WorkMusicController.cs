using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.IO;
using System.Collections.Generic;
public class WorkMusicController : MonoBehaviour
{
    public AudioClip[] tracks;
    public Image[] buttons;
    public Color muteColor = Color.red;
    public Color unmuteColor = Color.green;
    public Color hoverColor = Color.white;
    public Image submitButton;
    public TextMeshProUGUI taskText;
    public TextMeshProUGUI timeText;
    public float timeLimit = 15f;
    public Color flashColor = Color.red;
    public float flashDuration = 0.2f;

    private AudioSource[] sources;
    private int[] currentRequest;
    private bool canInput = true;
    private float timer;
    int hunger;
    int money;
    int day;
    private int completedTasks = 0;  // Biến để đếm số task đã hoàn thành
    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.txt");

        // Lấy giá trị Sanity từ playerData.txt
        int currentSanity = LoadSanityData(); // Mặc định là 4 nếu không tìm thấy dữ liệu
        currentSanity = Mathf.Max(currentSanity - 1, 0); // Đảm bảo Sanity không dưới 0
        // Lấy giá trị Sanity từ playerData.txt
      

        SaveSanityData(currentSanity); // Lưu lại giá trị mới

        PlayerPrefs.SetInt("Energy", 0);
        timer = timeLimit;
        sources = new AudioSource[tracks.Length];

        for (int i = 0; i < tracks.Length; i++)
        {
            GameObject go = new GameObject("Track_" + i);
            go.transform.parent = transform;
            AudioSource src = go.AddComponent<AudioSource>();
            src.clip = tracks[i];
            src.loop = true;
            src.Play();
            src.mute = true;
            sources[i] = src;
        }

        for (int i = 0; i < tracks.Length; i++)
        {
            var btnObj = GameObject.Find("Nút" + (i + 1));
            if (btnObj != null)
            {
                buttons[i] = btnObj.GetComponent<Image>();
                int index = i;

                Button btn = btnObj.GetComponent<Button>();
                if (btn == null)
                    btn = btnObj.AddComponent<Button>();
                btn.onClick.AddListener(() => ToggleTrack(index));

                EventTrigger trigger = btnObj.GetComponent<EventTrigger>();
                if (trigger == null)
                    trigger = btnObj.AddComponent<EventTrigger>();
                trigger.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();

                EventTrigger.Entry enter = new EventTrigger.Entry();
                enter.eventID = EventTriggerType.PointerEnter;
                enter.callback.AddListener((data) => { buttons[index].color = hoverColor; });
                trigger.triggers.Add(enter);

                EventTrigger.Entry exit = new EventTrigger.Entry();
                exit.eventID = EventTriggerType.PointerExit;
                exit.callback.AddListener((data) => { UpdateButtonColors(); });
                trigger.triggers.Add(exit);
            }
        }

        // Submit button hover effect
        EventTrigger submitTrigger = submitButton.GetComponent<EventTrigger>();
        if (submitTrigger == null)
            submitTrigger = submitButton.gameObject.AddComponent<EventTrigger>();

        submitTrigger.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();

        EventTrigger.Entry submitClick = new EventTrigger.Entry();
        submitClick.eventID = EventTriggerType.PointerClick;
        submitClick.callback.AddListener((data) => { CheckAnswer(); });
        submitTrigger.triggers.Add(submitClick);

        EventTrigger.Entry submitHoverEnter = new EventTrigger.Entry();
        submitHoverEnter.eventID = EventTriggerType.PointerEnter;
        submitHoverEnter.callback.AddListener((data) => { submitButton.color = hoverColor; });
        submitTrigger.triggers.Add(submitHoverEnter);

        EventTrigger.Entry submitHoverExit = new EventTrigger.Entry();
        submitHoverExit.eventID = EventTriggerType.PointerExit;
        submitHoverExit.callback.AddListener((data) => { submitButton.color = Color.white; });
        submitTrigger.triggers.Add(submitHoverExit);

        RandomTask();
        UpdateButtonColors();
    }

    void Update()
    {
        if (!canInput) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SceneManager.LoadScene("Room");
        }
        else
        {
            timeText.text = "" + Mathf.Ceil(timer).ToString();  // Hiển thị thời gian còn lại
        }

        for (int i = 0; i < sources.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ToggleTrack(i);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckAnswer();
        }
    }

    void ToggleTrack(int index)
    {
        sources[index].mute = !sources[index].mute;
        UpdateButtonColors();
    }

    void UpdateButtonColors()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null)
                buttons[i].color = sources[i].mute ? muteColor : unmuteColor;
        }
    }

    void RandomTask()
    {
        List<int> availableNumbers = new List<int>();
        for (int i = 1; i <= tracks.Length; i++)
        {
            availableNumbers.Add(i);
        }

        // Bắt đầu với 1 track và mỗi lần hoàn thành task sẽ thêm 1 track vào
        int length = 1 + completedTasks;  // Độ dài task tăng dần theo số task đã hoàn thành
        length = Mathf.Min(length, 5);  // Đảm bảo không vượt quá số lượng track

        currentRequest = new int[length];
        string txt = "";

        for (int i = 0; i < length; i++)
        {
            int randomIndex = Random.Range(0, availableNumbers.Count);
            currentRequest[i] = availableNumbers[randomIndex];
            availableNumbers.RemoveAt(randomIndex);

            txt += currentRequest[i];
            if (i < length - 1) txt += ", ";
        }

        taskText.text = txt;
    }

    void CheckAnswer()
    {
        foreach (var src in sources)
        {
            src.gameObject.GetComponent<AudioSource>();
        }

        for (int i = 0; i < sources.Length; i++)
        {
            bool shouldUnmute = System.Array.Exists(currentRequest, x => x == (i + 1));
            if (sources[i].mute == shouldUnmute)
            {
                StartCoroutine(FlashRed());
                return;
            }
        }

        completedTasks++;  // Tăng số task đã hoàn thành sau mỗi lần trả lời đúng
        RandomTask();
    }

    IEnumerator FlashRed()
    {
        canInput = false;
        Color oriColor = taskText.color;
        taskText.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        taskText.color = oriColor;
        canInput = true;
    }
    // Lưu dữ liệu Sanity vào playerData.txt
    void SaveSanityData(int sanity)
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
            writer.WriteLine(hunger-1);
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
                reader.ReadLine();
                int hunger = int.Parse(reader.ReadLine());
                reader.ReadLine(); // Bỏ qua Hunger
                reader.ReadLine(); // Bỏ qua Money

                return hunger;
            }
        }
        else
        {
            // Nếu tệp không tồn tại, trả về giá trị mặc định là 4
            return 4;
        }
    }
    // Đọc dữ liệu Sanity từ playerData.txt
    int LoadSanityData()
    {
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {

                int sanity = int.Parse(reader.ReadLine());
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
