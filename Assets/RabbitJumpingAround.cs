using UnityEngine;
using UnityEngine.UI; // Thêm namespace cho Image
using UnityEngine.EventSystems; // Thêm namespace cho việc xử lý sự kiện click

public class RabbitJumpingAround : MonoBehaviour, IPointerClickHandler
{
    public float jumpCooldown = 1f;  // Thời gian giữa các lần nhảy
    public float jumpDuration = 0.3f; // Thời gian bay khi nhảy
    public float lifeTime = 5f; // Thời gian sống của con thỏ
    public Color startColor = Color.white; // Màu ban đầu
    public Color endColor = Color.red; // Màu đỏ khi sắp nổ

    private RectTransform rt;
    private RectTransform moveArea;   // Vùng giới hạn nhảy (RectTransform của spawner)
    private float timer;
    private float spawnTime; // Thời gian thỏ xuất hiện
    private Image image; // Image để thay đổi màu sắc
    private bool isExploded = false; // Để đảm bảo thỏ chỉ nổ một lần
    private Vector3 lastScale; // Biến lưu lại giá trị scale hiện tại
    private bool isdead = false;
    void Start()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>(); // Lấy Image component thay vì SpriteRenderer

        if (image != null)
        {
            image.color = startColor; // Thiết lập màu ban đầu
        }

        timer = jumpCooldown;

        // Lưu lại scale ban đầu
        lastScale = rt.localScale;

        // Tìm đối tượng có tag "Zone" và gán vào moveArea
        GameObject zoneObject = GameObject.FindWithTag("Zone");
        if (zoneObject != null)
        {
            moveArea = zoneObject.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("Không tìm thấy đối tượng có tag 'Zone'");
        }

        spawnTime = Time.time; // Ghi lại thời gian thỏ xuất hiện ngay khi bắt đầu
    }

    void Update()
    {
        if (moveArea == null) return; // Kiểm tra nếu moveArea chưa được gán

        timer -= Time.deltaTime;
        if (timer <= 0f && !isdead)
        {
            Jump();
            timer = jumpCooldown + Random.Range(0f, 0.5f);
        }

        // Kiểm tra xem thỏ có sống quá 5 giây không
        float elapsedTime = Time.time - spawnTime;
        if (elapsedTime >= lifeTime && !isExploded)
        {
            Explode();
        }
        else
        {
            // Thay đổi màu sắc theo thời gian sống
            if (image != null && !isdead)
            {
                float lerpFactor = Mathf.Clamp01(elapsedTime / lifeTime);
                image.color = Color.Lerp(startColor, endColor, lerpFactor);
            }
            else
            {
                // Đổi màu thỏ thành xám
                if (image != null)
                {
                    image.color = Color.gray;
                }
            }
        }
    }

    void Jump()
    {
        if (moveArea == null) return; // Kiểm tra nếu moveArea chưa được gán

        // Tính toán các giới hạn của vùng di chuyển trong moveArea
        Vector2 minPos = (Vector2)moveArea.position - moveArea.rect.size / 2;
        Vector2 maxPos = (Vector2)moveArea.position + moveArea.rect.size / 3;

        // Chọn một vị trí ngẫu nhiên trong vùng di chuyển
        Vector2 targetPos = new Vector2(
            Random.Range(minPos.x, maxPos.x),
            Random.Range(minPos.y, maxPos.y)
        );

        // Flip thỏ nếu nhảy về bên trái
        if (targetPos.x < rt.localPosition.x)
        {
            if (rt.localScale.x > 0) // Nếu thỏ chưa bị flip, tiến hành flip
            {
                rt.localScale = new Vector3(-2f, 2f, 2f); // Flip thỏ theo trục X
            }
        }
        else
        {
            if (rt.localScale.x < 0) // Nếu thỏ đã bị flip, quay lại bình thường
            {
                rt.localScale = new Vector3(2f, 2f, 2f); // Quay lại bình thường
            }
        }

        // Di chuyển thỏ đến vị trí mới
        LeanTween.move(rt, targetPos, jumpDuration).setEase(LeanTweenType.easeOutQuad)
            .setOnStart(() =>
            {
                // Lưu lại scale hiện tại để đảm bảo không bị reset
                lastScale = rt.localScale;

                // Nếu thỏ nhảy sang trái, scale thỏ theo chiều âm (flip trái)
                if (targetPos.x < rt.localPosition.x)
                {
                    LeanTween.scale(rt, new Vector3(-2f, 2f, 2f), 0.1f); // Scale về phía trái
                }
                else
                {
                    LeanTween.scale(rt, new Vector3(2f, 2f, 2f), 0.1f); // Scale về phía phải
                }
            })
            .setOnComplete(() =>
            {
                // Đảm bảo scale không bị reset và giữ lại giá trị scale đã thay đổi
                rt.localScale = lastScale;
            });
    }

    void Explode2()
    {
        if (!isExploded)
        {
            isdead = true;
            isExploded = true; // Đảm bảo chỉ nổ một lần
                               // Code xử lý khi thỏ nổ (ví dụ, hủy đối tượng hoặc chơi âm thanh nổ)
            Debug.Log("Rabbit exploded!");
            
            // Dừng tất cả các animation LeanTween nếu đang chạy
            LeanTween.cancel(rt.gameObject);
            // Ngừng nhảy bằng cách ngừng mọi thay đổi vị trí hoặc scale
            timer = float.MaxValue;  // Dừng các lần nhảy tiếp theo
                                     // Tắt Animator để dừng mọi animation
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false;
            }

        }
    }
    void Explode()
    {
        if (!isExploded)
        {
            isExploded = true; // Đảm bảo chỉ nổ một lần
                               // Code xử lý khi thỏ nổ (ví dụ, hủy đối tượng hoặc chơi âm thanh nổ)
            Debug.Log("Rabbit exploded!");
            Destroy(gameObject); // Xóa thỏ khi nó nổ

        }
    }

    // Hàm xử lý sự kiện click chuột vào thỏ
    public void OnPointerClick(PointerEventData eventData)
    {
        // Kiểm tra xem thỏ có nổ hay không
        if (Time.time - spawnTime < lifeTime && !isExploded)
        {
            // Nếu thỏ chưa nổ, người chơi đã đập trúng thỏ
            Explode2();
        }
    }
}