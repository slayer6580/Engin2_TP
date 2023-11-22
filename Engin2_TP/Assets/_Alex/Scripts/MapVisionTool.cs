using UnityEngine;

public class MapVisionTool : MonoBehaviour
{
    [SerializeField] private Color32 m_color;

    void Start()
    {
        SetMapVision();
    }

    private void SetMapVision()
    {
        transform.position = new Vector2(0, 0);
        GetComponent<SpriteRenderer>().color = m_color;
        float mapDimension = MapGenerator.MapDimension.Value;
        transform.localScale = new Vector3(mapDimension, mapDimension, 1);
    }
}
