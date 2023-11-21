using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float m_minimumCameraSize = 3.0f;
    [SerializeField]
    private float m_maximumCameraSize = 40.0f;
    [SerializeField]
    private float m_movementSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        Vector2 vector2 = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            vector2 += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            vector2 += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            vector2 += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            vector2 += Vector2.right;
        }

        transform.Translate(vector2 * m_movementSpeed * Time.deltaTime * Camera.main.orthographicSize);

        if (Input.mouseScrollDelta.y != 0)
        {
            Camera.main.orthographicSize *= (1.0f - Input.mouseScrollDelta.y * 0.1f);
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, m_minimumCameraSize, m_maximumCameraSize);
        }
    }
}
