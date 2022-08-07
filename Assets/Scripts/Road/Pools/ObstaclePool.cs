using System;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    //[SerializeField] private Obstacle defaultObstaclePrefab;
    private ObjectPool<Obstacle> pool;// { get; private set; } //ВЫНЕСТИ В ОТДЕЛЬНЫЙ КЛАСС, ЗДЕСЬ ХРАНИТЬ СПИСКИ  
    public void CreateObstaclePool(Obstacle obstaclePrefab) // ВЫНЕСТИ В КЛАССЫ
    {
        Func<Obstacle> createObstacle = () =>
        {
            Obstacle obstacle = Instantiate(obstaclePrefab);
            obstacle.gameObject.SetActive(false);
            obstacle.transform.SetParent(gameObject.transform, false);
            return obstacle;
        };
        Action<Obstacle> getObstacle = (Obstacle obstacle) =>
        {
            obstacle.gameObject.SetActive(true);
        };
        Action<Obstacle> releaseObstacle = (Obstacle obstacle) =>
        {
            obstacle.transform.SetParent(gameObject.transform, false);
            obstacle.gameObject.SetActive(false);
        };
        pool = new ObjectPool<Obstacle>(createObstacle, getObstacle, releaseObstacle,100);
    }
    public Obstacle GetFromPool()
    {
        return pool.Get();
    }
    public void ReturnToPool(Obstacle obstacle)
    {
        pool.ReturnToPool(obstacle);
    } 
}
