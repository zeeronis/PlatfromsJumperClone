using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public delegate void OnCoinCollected(in Vector3 collectPos);
public delegate void OnDie();

public class BirdController: MonoBehaviour
{
    [SerializeField] Transform visualTransform;
    [SerializeField] BirdMovement birdMovement;

    public event OnDie OnDie;
    public event OnCoinCollected OnCoinCollected;
    public BirdMovement Movement => birdMovement;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.Instance.IsRun)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Coin"))
        {
            OnCoinCollected?.Invoke(collision.gameObject.transform.position);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Die();
        }
    }

    private void Die()
    {
        OnDie?.Invoke();

        birdMovement.ResetVelocity();
        visualTransform.localEulerAngles = new Vector3(
          visualTransform.localEulerAngles.x,
          visualTransform.localEulerAngles.y,
          -90);
    }

    public void ResetRotate()
    {
        visualTransform.localEulerAngles = Vector3.zero;
    }
}
