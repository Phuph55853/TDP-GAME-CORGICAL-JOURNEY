using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0; // Điểm số toàn cục
    [SerializeField] private Text scoreText; // Tham chiếu đến Text UI

    void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("ScoreText không được gán! Vui lòng gán Text UI trong Inspector.");
        }
        UpdateScoreDisplay();
    }

    public void AddScore(int points)
    {
        score += points; // Tăng điểm
        UpdateScoreDisplay();
    }

    public void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; // Cập nhật nội dung Text UI
        }
    }
}