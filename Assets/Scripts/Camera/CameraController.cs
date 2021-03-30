using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [Space]
    [SerializeField] float moveUpSpeed;
    [SerializeField] float moveDownSpeed;
    [SerializeField] float camOffsetY;

    private Transform transform;


    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        float currY = transform.position.y;
        float endY = target.position.y + camOffsetY;
        float t = currY > endY ? moveDownSpeed * Time.deltaTime : moveUpSpeed * Time.deltaTime;

        transform.position = new Vector3(
            transform.position.x,
            Mathf.Lerp(currY, endY, t),
            transform.position.z);
    }
}
