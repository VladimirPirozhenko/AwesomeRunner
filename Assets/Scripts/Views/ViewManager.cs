using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance { get; private set; }    
    [SerializeField] private List<BaseView> views = new List<BaseView>();
    [SerializeField] private BaseView defaultView;
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
        if (defaultView != null) defaultView.Show(true);
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

    public bool TryGetView<T>(out T baseView) where T : BaseView
    {
        foreach (var view in views)
        {
            if (view is T)
            {
               baseView = (T)view;
               return true;
            }  
        }
        baseView = null;    
        return false;
    }
}
