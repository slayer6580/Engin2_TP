using UnityEngine;

public class Zone_Alex : MonoBehaviour
{
    [HideInInspector] public bool m_isExplored = false;
    [SerializeField] private Color32 m_color;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Worker_Alex worker = collision.GetComponent<Worker_Alex>();

        if (worker)
        {
            if (Vector2.Distance(worker.gameObject.transform.position, transform.position) < 1)
            {
                m_isExplored = true;
                GetComponent<SpriteRenderer>().color = m_color;
            }       
        }
    }
}
