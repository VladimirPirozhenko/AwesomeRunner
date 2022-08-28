using System.Collections;
using UnityEngine;

public abstract class ImageFilter : MonoBehaviour
{
    [SerializeField] private Shader shader;

    protected Material mat;

    private bool isFilterOn = true;

    private void Awake()
    {
        mat = new Material(shader);
        Init();
    }

    abstract protected void Init();


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            isFilterOn = !isFilterOn;
        }

        Tick();
    }

    protected virtual void Tick()
    {

    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (isFilterOn)
        {
            UseFilter(src, dst);
        }
        else
        {
            Graphics.Blit(src, dst);
        }
    }

    protected virtual void UseFilter(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, mat);
    }
}