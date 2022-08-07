using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IObstacle
{
	public void Impact();
}
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Renderer))]
public class Obstacle : MonoBehaviour,IObstacle,IDamageDealer,IResettable
{
    public BoxCollider Collider { get; private set; }
    
    //private Renderer defaultRenderer;
    //private Renderer obstacleRenderer;
    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
        //defaultRenderer = GetComponent<Renderer>();
        //obstacleRenderer = defaultRenderer;
    }
    public void ResetToDefault()
    {
        //if (obstacleRenderer.material.color != defaultRenderer.material.color)
        //     obstacleRenderer.material.color = defaultRenderer.material.color;
        //gameObject.transform.localRotation = Quaternion.identity;
        //gameObject.transform.localScale = Vector3.one;  
        // gameObject.transform.localPosition = Vector3.zero;
        //gameObject.SetActive(false);
        //gameObject.transform.SetParent(null);
    }
    public void Impact()
    {
        //obstacleRenderer.material.color = Color.red;
        gameObject.SetActive(false);
    }

    public void DealDamage(IDamageable target, int amount)
    {
        target.TakeDamage(amount);
    }
}

public class Turret : MonoBehaviour, IObstacle, IDamageDealer, IResettable
{
    public void DealDamage(IDamageable target, int amount)
    {
        target.TakeDamage(amount);
    }

    public void Impact()
    {
        gameObject.SetActive(true);
    }

    public void ResetToDefault()
    {
        gameObject.SetActive(true);
    }
}