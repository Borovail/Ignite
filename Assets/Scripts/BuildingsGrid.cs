using UnityEngine;

public class BuildingsGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);
    private Building[,] grid;
    private Building flyingBuilding;
    private Camera mainCamera;

    private void Awake()
    {
        grid = new Building[GridSize.x * 2, GridSize.y * 2]; // Подвоюємо розмір сітки, щоб включати негативні координати
        mainCamera = Camera.main;
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        flyingBuilding = Instantiate(buildingPrefab);

        if (flyingBuilding.GetComponentInChildren<Renderer>() == null)
        {
            Debug.LogError("Building prefab не містить Renderer!", flyingBuilding);
        }
    }

    private void Update()
    {
        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool available = IsPositionValid(x, y) && !IsPlaceTaken(x, y);

                flyingBuilding.transform.position = new Vector3(x, 0, y);
                flyingBuilding.SetTransparent(available);

                if (available && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingBuilding(x, y);
                }
            }
        }
    }

    private bool IsPositionValid(int x, int y)
    {
        return x >= -GridSize.x && x <= GridSize.x - flyingBuilding.Size.x &&
               y >= -GridSize.y && y <= GridSize.y - flyingBuilding.Size.y;
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                int gridX = placeX + x + GridSize.x; // Зсув для роботи з негативними координатами
                int gridY = placeY + y + GridSize.y;
                
                if (grid[gridX, gridY] != null) return true;
            }
        }

        return false;
    }

    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                int gridX = placeX + x + GridSize.x;
                int gridY = placeY + y + GridSize.y;

                grid[gridX, gridY] = flyingBuilding;
            }
        }

        flyingBuilding.SetNormal();
        flyingBuilding = null;
    }
}
