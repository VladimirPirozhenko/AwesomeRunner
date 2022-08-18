using System.Collections;
using UnityEngine;


//based on: https://grrava.blogspot.com/2015/05/bending-world-with-unity.html
[RequireComponent(typeof(Camera))]  
public class CullingCamera : MonoBehaviour
{
    [Range(0, 0.5f)]
    [SerializeField] private float extraCullHeight;
    [Range(0, 0.5f)]
    [SerializeField] private float extraCullWidth;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    public static float HorizontalToVerticalFOV(float horizontalFOV, float aspect)
    {
        return Mathf.Rad2Deg * 2 * Mathf.Atan(Mathf.Tan((horizontalFOV * Mathf.Deg2Rad) / 2f) / aspect);
    }
    private void OnPreCull()
    {
        float aspectRatio = cam.aspect;
        float fov = cam.fieldOfView;

        float viewPortHeight = Mathf.Tan(Mathf.Deg2Rad * fov * 0.5f);
        float viewPortWidth = viewPortHeight * aspectRatio;
        
        float newFow = fov * (1 + extraCullHeight);
        float newHeight = Mathf.Tan(Mathf.Deg2Rad * newFow * 0.5f);
        float newWidth = viewPortWidth * (1 + extraCullWidth);
        float newAspectRatio = newWidth / newHeight;

        cam.projectionMatrix = Matrix4x4.Perspective(newFow, newAspectRatio, cam.nearClipPlane, cam.farClipPlane);
    }

    private void OnPreRender()
    {
        cam.ResetProjectionMatrix();
    }
}