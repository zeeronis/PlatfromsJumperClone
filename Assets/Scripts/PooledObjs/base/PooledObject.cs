using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public bool IsActive { get; private set; }


    public virtual void SetActive()
    {
        IsActive = true;
        gameObject.SetActive(true);
    }

    public virtual void ReturnToPool()
    {
        IsActive = false;
        gameObject.SetActive(false);
    }
}
