using System;
using System.Collections;
using UnityEngine;
public class CoinPool : MonoBehaviour
{
    private ObjectPool<Coin> pool;// { get; private set; } //ВЫНЕСТИ В ОТДЕЛЬНЫЙ КЛАСС, ЗДЕСЬ ХРАНИТЬ СПИСКИ  
    public void CreateCoinPool(Coin coinPrefab) // ВЫНЕСТИ В КЛАССЫ
    {
        Func<Coin> createObstacle = () =>
        {
            Coin coin = Instantiate(coinPrefab);
            coin.gameObject.SetActive(false);
            coin.transform.SetParent(gameObject.transform, false);
            return coin;
        };
        Action<Coin> getCoin = (Coin coin) =>
        {
            coin.gameObject.SetActive(true);
        };
        Action<Coin> releaseCoin = (Coin coin) =>
        {
            coin.transform.SetParent(gameObject.transform, false);
            coin.gameObject.SetActive(false);
        };
        pool = new ObjectPool<Coin>(createObstacle, getCoin, releaseCoin, 5);
    }
    public Coin GetFromPool()
    {
        return pool.Get();
    }
    public void ReturnToPool(Coin coin)
    {
        pool.ReturnToPool(coin);
    }
}
