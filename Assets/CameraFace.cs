using UnityEngine;

public class CameraFace : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    private Camera _camera;

    private void OnEnable()
    {
        _camera = Camera.main;
        _canvas.worldCamera = _camera;        
    }

    private void Update()
    {
        if (transform.rotation != _camera.transform.rotation)        
            transform.rotation = _camera.transform.rotation;        
    }
}