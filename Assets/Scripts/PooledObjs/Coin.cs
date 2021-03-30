using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : PooledObject
{
    Transform transform;


    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        ReturnToPool();
    }
}
