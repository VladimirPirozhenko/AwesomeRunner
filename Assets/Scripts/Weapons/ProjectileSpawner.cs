using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner 
{
    private BulletProjectile projectilePrefab;
    private Transform projectileParent;
    private ObjectPool<BulletProjectile> projectilePool;
    private int initialPoolCapacity;

    public ProjectileSpawner(BulletProjectile projectilePrefab,Transform projectileParent)
    {
        initialPoolCapacity = 15;
        this.projectilePrefab = projectilePrefab;
        this.projectileParent = projectileParent;
        projectilePool = new ObjectPool<BulletProjectile>(CreateProjectile, GetProjectile, HideProjectile, initialPoolCapacity);
        
    }
    private BulletProjectile CreateProjectile()
    {
        BulletProjectile projectile = GameObject.Instantiate(projectilePrefab, new Vector3(), new Quaternion());  
        projectile.gameObject.SetActive(false);    
        projectile.transform.SetParent(projectileParent);
        return projectile;
    }
    private void GetProjectile(BulletProjectile projectile)
    {
        projectile.gameObject.SetActive(true);
        projectile.OnBulletHit += ReturnToPool;
    }
    private void HideProjectile(BulletProjectile projectile)
    {
        projectile.ResetToDefault();
        projectile.OnBulletHit -= ReturnToPool;
    }
    public void ReturnToPool(BulletProjectile projectile)
    {
        if (projectile == null)
            return;
        projectilePool.ReturnToPool(projectile);
    }
    public BulletProjectile Spawn(Vector3 spawnPos)
    {
        BulletProjectile projectile = projectilePool.Get();
        projectile.transform.position = spawnPos;
        return projectile;
    }
}
