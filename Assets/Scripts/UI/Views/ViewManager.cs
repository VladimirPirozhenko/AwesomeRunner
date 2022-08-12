using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance { get; private set; }    
    [SerializeField] private List<BaseView> views = new List<BaseView>();
    [SerializeField] private List<BaseView> defaultViews;
    [SerializeField] private bool autoInitialize;
    private void Awake()
    {
        Instance = this;    
    }
    private void Start()
    {
        if (autoInitialize)
            InitAllViews();
    }
    public void InitAllViews()
    {
        foreach (var view in views)
        {
            view.Init();
            view.Show(false);   
        }
        if (defaultViews.IsEmpty())
            return;
        foreach (var view in defaultViews)
        {
            view.Show(true);
        }     
    }
    public void Init<T>() where T : BaseView
    {
        foreach (var view in views)
        {
            if (view is T)
            {
                view.Init();
            }
        }
    }
    public void Show<T>(bool isActive) where T : BaseView
    {
        foreach (var view in views)
        { 
            if (view is T)
            {
                view.Show(isActive);
            } 
        }
    }

    public bool IsActive<T>() where T : BaseView
    {
        foreach (var view in views)
        {
            if (view is T)
            {
                return view.IsActive();
            }
        }
        return false;
    }

    public void Add(BaseView view,bool isActiveByDefault = true)
    {
        view.Init();
        view.Show(isActiveByDefault);
        views.Add(view);       
    }

    public void Remove(BaseView view)
    {
        views.Remove(view);
        view.Destroy(); 
    }
}
