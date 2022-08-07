using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour, IResettable
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileLifetime;
    private Rigidbody bulletRigidBody;
    public WaitForSeconds ProjectileLifetimeWait { get; private set; }
    public event Action<BulletProjectile> OnBulletHit;
    void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();     
        ProjectileLifetimeWait = new WaitForSeconds(projectileLifetime);
    }
    public void Launch(Vector3 direction)
    {
        bulletRigidBody.velocity = direction * projectileSpeed;    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Obstacle obstacle))
        {
            obstacle.Impact();
            OnBulletHit?.Invoke(this);
        }
    }
    public void ResetToDefault()
    {
        gameObject.SetActive(false);
    }
}
