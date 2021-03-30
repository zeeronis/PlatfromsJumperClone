using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private BirdMovement birdMovement;


    private void Awake()
    {
        birdMovement = GetComponent<BirdMovement>();
    }

    void Update()
    {
        if (!GameManager.Instance.IsRun)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            birdMovement.Jump();
        }
    }
}
