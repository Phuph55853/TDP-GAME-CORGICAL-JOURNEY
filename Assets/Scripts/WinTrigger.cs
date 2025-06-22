using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public GameObject winPanel; // Kéo WinPanel vào đây trong Inspector

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f; // Dừng game nếu muốn
        }
    }
}
