using System.Collections;
using UnityEngine;

public class EnemyBase: PooledObject
{
    [SerializeField] protected Collider2D collider;
    [Space]
    [SerializeField] protected float minBoundX;
    [SerializeField] protected float maxBoundX;

    public float ColliderSizeY => collider.bounds.size.y;


    public virtual EnemyBase Run()
    {
        transform.transform.position = new Vector3(
          Random.Range(minBoundX, maxBoundX),
          transform.position.y,
          transform.position.z);

        return this;
    }
}
