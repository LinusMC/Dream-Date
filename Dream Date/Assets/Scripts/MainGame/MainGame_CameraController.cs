using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame_CameraController : MonoBehaviour
{

    public Transform target;

    public float speed = 2;

    public Rect limitRect;

    public float fixedHeight { get { return Mathf.Clamp(startSize - camera.orthographicSize, 0, camera.orthographicSize); } }
    public float fixedWidth { get { return fixedHeight * camera.aspect; } }
    float startSize;

    Camera camera { get { return _camera != null ? _camera : GetComponent<Camera>(); } }
    Camera _camera;

    private void Awake()
    {
        startSize = camera.orthographicSize;
    }
    public void LateUpdate()

    {
        float z = transform.position.z;
        Vector2 pos = Vector2.Lerp(transform.position, target.position, speed*Time.deltaTime);
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);

        Limiting();
    }

    void Limiting()
    {
        Vector2 pos = new Vector2(Mathf.Clamp(transform.position.x, limitRect.x - fixedWidth, limitRect.width + fixedWidth), Mathf.Clamp(transform.position.y, limitRect.y - fixedHeight, limitRect.height + fixedHeight));

        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }

}
