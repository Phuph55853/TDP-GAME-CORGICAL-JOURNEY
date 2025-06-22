using UnityEngine;

public class SimpleMapGenerator : MonoBehaviour
{
    public GameObject groundTile;
    public GameObject pawTile;
    public GameObject rock;
    public GameObject cat;
    public float tileSize = 1.2f;

    enum TileType { Ground, Paw, Rock, Cat }

    TileType[,] map =
    {
        { TileType.Ground, TileType.Ground, TileType.Cat },
        { TileType.Ground, TileType.Ground, TileType.Ground },
        { TileType.Paw,    TileType.Rock,   TileType.Ground }
    };

    void Start()
    {
        for (int x = 0; x < map.GetLength(1); x++)
        {
            for (int z = 0; z < map.GetLength(0); z++)
            {
                Vector3 pos = new Vector3(x * tileSize, 0, -z * tileSize); // trục Z âm để nhìn đúng hướng

                // Luôn spawn tile nền
                GameObject tile = Instantiate(groundTile, pos, Quaternion.identity, transform);

                // Spawn thêm tùy loại
                switch (map[z, x])
                {
                    case TileType.Paw:
                        Instantiate(pawTile, pos + Vector3.up * 0.01f, Quaternion.identity, transform);
                        break;
                    case TileType.Rock:
                        Instantiate(rock, pos + Vector3.up * 0.5f, Quaternion.identity, transform);
                        break;
                    case TileType.Cat:
                        Instantiate(cat, pos + Vector3.up * 0.5f, Quaternion.identity, transform);
                        break;
                }
            }
        }
    }
}
