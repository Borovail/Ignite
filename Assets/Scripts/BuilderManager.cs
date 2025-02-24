using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }
    
    public enum BuildingType
    {
        House,
        Barracks
    }

    public BuildingType CurrentType { get; private set; } = BuildingType.House;
    [SerializeField] private Button houseButton;
    [SerializeField] private Button barracksButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        houseButton.onClick.AddListener(() => SetBuildingType(BuildingType.House));
        barracksButton.onClick.AddListener(() => SetBuildingType(BuildingType.Barracks));
    }

    private void SetBuildingType(BuildingType type)
    {
        CurrentType = type;
        Debug.Log($"Building type set to: {CurrentType}");
    }
}