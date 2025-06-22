using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject canvasToShow;  // Canvas cần hiện
    public GameObject canvasToHide;  // Canvas cần ẩn

    // Gọi khi ấn nút "!"
    public void ShowCanvas()
    {
        if (canvasToHide != null)
            canvasToHide.SetActive(false);

        if (canvasToShow != null)
            canvasToShow.SetActive(true);
    }

    // Gọi khi ấn nút "X"
    public void CloseCanvas()
    {
        if (canvasToShow != null)
            canvasToShow.SetActive(false);

        if (canvasToHide != null)
            canvasToHide.SetActive(true);
    }

    public void StartNewGame()
    {
        if (canvasToHide != null)
            canvasToHide.SetActive(false);

        // Bạn có thể thêm các dòng xử lý khởi động game ở đây nếu muốn
        // Ví dụ: SceneManager.LoadScene("Level1");
    }
}
