using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Coin : MonoBehaviour, ICollectible
{
    [SerializeField] private int costCoin;

    public static event Action<int> OnCoinCollected;

    public void Collect()
    {
        Destroy(gameObject);

        OnCoinCollected?.Invoke(costCoin);
    }
}
