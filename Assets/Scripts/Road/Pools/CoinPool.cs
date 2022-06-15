using System;
using System.Collections;
using UnityEngine;
public class CoinPool : MonoBehaviour
{
    private ObjectPool<Coin> pool;// { get; private set; } //ВЫНЕСТИ В ОТДЕЛЬНЫЙ КЛАСС, ЗДЕСЬ ХРАНИТЬ СПИСКИ  
    public void CreateCoinPool(Coin coinPrefab) // ВЫНЕСТИ В КЛАССЫ
    {
        Func<Coin> createCoin = () =>
        {
            Coin coin = Instantiate(coinPrefab);
            coin.gameObject.SetActive(false);
            coin.transform.SetParent(gameObject.transform, false);
            return coin;
        };
        Action<Coin> getCoin = (Coin coin) =>
        {
            //Debug.LogError("COIN_POS_GET: " + coin.transform.position);
            coin.gameObject.SetActive(true);
           
        };
        Action<Coin> releaseCoin = (Coin coin) =>
        {
            //Debug.LogError("COIN_POS_RELEASE: " + coin.transform.position);
            coin.gameObject.SetActive(false);
            coin.transform.position = Vector3.zero;
            coin.transform.localPosition = Vector3.zero;      
            coin.transform.rotation = Quaternion.identity;
            coin.transform.SetParent(null);
            
        };
        pool = new ObjectPool<Coin>(createCoin, getCoin, releaseCoin, 100);
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
