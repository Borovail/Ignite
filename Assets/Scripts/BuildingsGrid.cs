using Unity.Entities;
using UnityEngine;

public class BuildingsGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);
    public Building[] BuildingPrefabs;
    private Building[,] grid;
    private Building flyingBuilding;
    private Camera mainCamera;
    private EntityManager entityManager;
    private Entity[] buildingEntities;
    private int _index;

    public class Baker : Baker<BuildingsGrid>
    {
        public override void Bake(BuildingsGrid authoring)
        {
            for (int i = 0; i < authoring.BuildingPrefabs.Length; i++)
            {
                authoring.buildingEntities[i] = GetEntity(authoring.BuildingPrefabs[i].gameObject, TransformUsageFlags.Dynamic);
            }
        }
    }


    private void Awake()
    {
        grid = new Building[GridSize.x * 2, GridSize.y * 2]; // Подвоюємо розмір сітки, щоб включати негативні координати
        mainCamera = Camera.main;
    }

    private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void StartPlacingBuilding(int index)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        _index = index;
        flyingBuilding = Instantiate(BuildingPrefabs[index]);

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
                int gridX = placeX + x + GridSize.x;
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

        entityManager.Instantiate(buildingEntities[_index]);

        Destroy(flyingBuilding);
        flyingBuilding = null;
    }
}
