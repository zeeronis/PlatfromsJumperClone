using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdWiggle : MonoBehaviour
{
    [SerializeField] float wiggleSpeed;
    [SerializeField] Vector3 startAngle;
    [SerializeField] Vector3 endAngle;

    private Transform visualTransform;
    private bool moveBack;
    private float t;


    private void Awake()
    {
        visualTransform = transform;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsRun)
            return;

        t += Time.deltaTime / wiggleSpeed;
        visualTransform.localEulerAngles = moveBack 
            ? Vector3.Lerp(endAngle, startAngle, t) 
            : Vector3.Lerp(startAngle, endAngle, t);

        if (t >= 1)
        {
            t = 0;
            moveBack = !moveBack;
        }
    }
}
