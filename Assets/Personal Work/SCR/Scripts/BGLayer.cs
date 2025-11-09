using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BGLayer : MonoBehaviour
{
    public Camera targetCamera;
    public bool updateEveryFrame = false;
    private SpriteRenderer sr;
    private Vector3 offset;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void Start()
    {
        if (targetCamera == null || sr == null)
            return;

        FitToCamera();
    }

    void FitToCamera()
    {
        float worldHeight = targetCamera.orthographicSize * 2f;
        float worldWidth = worldHeight * targetCamera.aspect;

        Vector2 spriteSize = sr.sprite.bounds.size;
        spriteSize.x *= transform.localScale.x;
        spriteSize.y *= transform.localScale.y;

        float scaleX = worldWidth / spriteSize.x;
        float scaleY = worldHeight / spriteSize.y;
        float finalScale = Mathf.Max(scaleX, scaleY);

        transform.localScale *= finalScale;
    }
}
