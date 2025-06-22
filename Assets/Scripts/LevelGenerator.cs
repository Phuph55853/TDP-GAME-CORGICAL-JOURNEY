using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public int maxRooms = 10;
    private Vector3 currentPosition = Vector3.zero;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for (int i = 0; i < maxRooms; i++)
        {
            int randomIndex = Random.Range(0, roomPrefabs.Length);
            Instantiate(roomPrefabs[randomIndex], currentPosition, Quaternion.identity);
            // Di chuyển vị trí để đặt phòng tiếp theo
            currentPosition += new Vector3(20, 0, 0); // Giả sử mỗi phòng rộng 20 unit
        }
    }
}