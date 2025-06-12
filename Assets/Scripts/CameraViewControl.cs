using UnityEngine;

public class CameraViewControl : MonoBehaviour
{

    [SerializeField] private float rectWidth = 0.5f;
    [SerializeField] private float rectHeight = 0.5f;
    [SerializeField] private float rectX = 0.25f;
    [SerializeField] private float rectY = 0.25f;

    [SerializeField] private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        cam.rect = new Rect(rectX, rectY, rectWidth, rectHeight);
    }

}
