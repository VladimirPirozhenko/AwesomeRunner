using UnityEngine;

public abstract class BaseView : MonoBehaviour
{
    public bool IsInitialized { get; private set; }
    [property: SerializeField] public string Tag { get; private set; }
    virtual public void Init()
    {
        IsInitialized = true;   
    }
    virtual public void Destroy()
    {
        Destroy(this);
    }
    public void Show(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public bool IsActive()
    {
        return gameObject.activeInHierarchy;
    }
}
