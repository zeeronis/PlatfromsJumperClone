using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseNumEffect : PooledObject
{
    [SerializeField] Animation animation;


    public void Play()
    {
        animation.Play();
    }
}
