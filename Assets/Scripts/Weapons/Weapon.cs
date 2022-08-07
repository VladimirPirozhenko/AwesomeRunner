using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Weapon : MonoBehaviour
{
    [SerializeField] Transform projectileParent;
    [SerializeField] BulletProjectile projectilePrefab;
    [SerializeField] Transform muzzlePoint; 
    ProjectileSpawner projectileSpawner;
    private void Awake()
    {
        projectileSpawner = new ProjectileSpawner(projectilePrefab, projectileParent);
    }
    public void Equip(Transform weaponPoint)
    {
        transform.gameObject.SetActive(true);
        transform.SetParent(weaponPoint, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;  
        transform.localScale = Vector3.one; 
    }
    public IEnumerator Shoot()
    {
        Vector3 spawnPosition = muzzlePoint.transform.position;
        BulletProjectile projectile = projectileSpawner.Spawn(spawnPosition);
        projectile.Launch(Vector3.forward);
        yield return projectile.ProjectileLifetimeWait;
        projectileSpawner.ReturnToPool(projectile);
    }
}
