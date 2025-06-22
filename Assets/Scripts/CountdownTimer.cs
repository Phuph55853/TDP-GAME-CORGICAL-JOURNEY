using UnityEngine;
using UnityEngine.UI;
using TMPro; // Nếu dùng TextMeshPro

public class CountdownTimer : MonoBehaviour
{
    public Text timerText; // Nếu dùng TextMeshPro, hoặc đổi thành Text nếu dùng UI Text thường
    public Button newGameButton;
    private float currentTime = 60f;
    private bool isCountingDown = false;

    void Start()
    {
        timerText.text = "60";
        newGameButton.onClick.AddListener(StartCountdown);
    }

    void Update()
    {
        if (isCountingDown)
        {
            currentTime -= Time.deltaTime;
            timerText.text = Mathf.Ceil(currentTime).ToString();

            if (currentTime <= 0)
            {
                isCountingDown = false;
                timerText.text = "Time's up!";
                // TODO: Thêm hành động khi hết giờ (ví dụ: hiện canvas thua cuộc)
            }
        }
    }

    void StartCountdown()
    {
        currentTime = 60f;
        isCountingDown = true;
    }
}
