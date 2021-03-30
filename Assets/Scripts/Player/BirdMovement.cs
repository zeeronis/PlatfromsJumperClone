using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnPlatformEvent(Platform platform);

public class BirdMovement : MonoBehaviour
{
    [Header("Move settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float xBounds;
    [Space]
    [SerializeField] Transform visualTransform;

    [Header("Jump settings")]
    [SerializeField] int maxJumps = 2;
    [SerializeField] float jumpForce;
    [SerializeField] float bounceForce;


    private Rigidbody2D rigidbody;
    private Collider2D collider2d;
    private Transform transform;
    private int availableJumps = 0;
    private bool isOnPlatform = true;
    private bool moveToRight;
    private bool isBounce;

    public event OnPlatformEvent OnPlatformEvent;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsRun)
            return;

        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Platform"))
            return;

        if (collider2d.bounds.min.y > collision.collider.bounds.max.y || rigidbody.velocity.y <= 0)
        {
            OnPlatform(collision.gameObject.GetComponent<Platform>());
        }
    }

    private void OnPlatform(Platform platform)
    {
        if (!GameManager.Instance.IsRun)
        {
            platform.RunBirdLandingEffect();
            return;
        }

        if (isOnPlatform == false && TryBounce())
        {
            platform.RunBirdLandingEffect();
            OnPlatformEvent?.Invoke(platform);
        }

        isOnPlatform = true;
        availableJumps = maxJumps;
    }

    private void Move()
    {
        Vector3 moveDir = new Vector3((moveToRight ? moveSpeed : -moveSpeed) * Time.deltaTime, 0, 0);
        transform.position += moveDir;

        if (transform.position.x >= xBounds)
        {
            moveToRight = false;
            visualTransform.localEulerAngles = new Vector3(visualTransform.localEulerAngles.x, 0, visualTransform.localEulerAngles.z);
        }
        else if (transform.position.x <= -xBounds)
        {
            moveToRight = true;
            visualTransform.localEulerAngles = new Vector3(visualTransform.localEulerAngles.x, 180, visualTransform.localEulerAngles.z);
        }
    }

    private bool TryBounce()
    {
        if (isBounce)
        {
            isBounce = false;
            return false;
        }

        isBounce = true;
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(Vector2.up * bounceForce);

        return true;
    }

    public void Jump()
    {
        if (availableJumps <= 0)
            return;

        availableJumps--;
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(Vector2.up * jumpForce);

        isBounce = false;
        isOnPlatform = false;
    }

    public void ResetVelocity()
    {
        rigidbody.velocity = Vector2.zero;
    }
}
