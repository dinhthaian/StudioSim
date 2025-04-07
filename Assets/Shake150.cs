using UnityEngine;

public class Shake150 : MonoBehaviour
{
    public float bpm = 150f;
    public float shakeAmount = 0.5f;  // Độ rung
    public float shakeDecay = 5f;     // Dịu rung
    public float scaleAmount = 0.1f;  // Phình to khi beat
    public float scaleDecay = 5f;     // Dịu scale

    private float timer;
    private Vector3 originalPos;
    private Vector3 originalScale;
    private Vector3 shakeOffset;
    private float scaleOffset;

    void Start()
    {
        originalPos = transform.localPosition;
        originalScale = transform.localScale;
    }

    void Update()
    {
        float beatInterval = 60f / bpm;
        timer += Time.deltaTime;

        if (timer >= beatInterval)
        {
            timer -= beatInterval;
            shakeOffset = Random.insideUnitCircle * shakeAmount;
            scaleOffset = scaleAmount;
        }

        shakeOffset = Vector3.Lerp(shakeOffset, Vector3.zero, Time.deltaTime * shakeDecay);
        scaleOffset = Mathf.Lerp(scaleOffset, 0f, Time.deltaTime * scaleDecay);

        transform.localPosition = originalPos + shakeOffset;
        transform.localScale = originalScale * (1f + scaleOffset);
    }
}
