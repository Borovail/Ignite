using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Renderer MainRenderer; // Використовуємо [SerializeField] для відображення в Inspector
    public Vector2Int Size = Vector2Int.one;

    private void Awake()
    {
        if (MainRenderer == null) // Якщо не призначено в Inspector, шукаємо компонент
        {
            MainRenderer = GetComponentInChildren<Renderer>();
            if (MainRenderer == null)
            {
                Debug.LogError("Renderer не знайдено в " + gameObject.name);
            }
        }
    }

    public void SetTransparent(bool available)
    {
        if (MainRenderer == null) return; // Уникаємо NullReferenceException
        MainRenderer.material.color = available ? Color.green : Color.red;
    }

    public void SetNormal()
    {
        if (MainRenderer == null) return; // Перевіряємо перед використанням
        MainRenderer.material.color = Color.white;
    }

        
    private void OnDrawGizmosSelected()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                Gizmos.color = (x + y) % 2 == 0 ? new Color(0.88f, 0f, 1f, 0.3f) : new Color(1f, 0.68f, 0f, 0.3f);
                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, .1f, 1));
            }
        }
    }
}
