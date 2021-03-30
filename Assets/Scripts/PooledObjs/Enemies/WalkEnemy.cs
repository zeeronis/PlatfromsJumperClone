using System.Collections;
using System.Text;
using UnityEngine;

public class WalkEnemy : EnemyBase
{
    [Space]
    [SerializeField] float moveSpeed;
    [SerializeField] Transform visualTransform;

    private Transform rootTransform;
    private bool isRun;

    private bool m_moveToRight;
    private bool MoveToRight
    {
        get
        {
            return m_moveToRight;
        }
        set
        {
            m_moveToRight = value;
            visualTransform.localScale = new Vector3(
                 visualTransform.localScale.x,
                 visualTransform.localScale.y,
                 m_moveToRight ? -1 : 1);
            visualTransform.localEulerAngles = new Vector3(
                visualTransform.localEulerAngles.x,
                m_moveToRight ? 180 : 0, 
                visualTransform.localEulerAngles.z);
        }
    }

    private void Awake()
    {
        rootTransform = transform;
        moveSpeed += Random.Range(-0.5f, 0.5f);
    }

    private void Update()
    {
        if (!isRun)
            return;

        Move();
    }

    private void Move()
    {
        Vector3 moveDir = new Vector3((MoveToRight ? moveSpeed : -moveSpeed) * Time.deltaTime, 0, 0);
        transform.position += moveDir;

        if (transform.position.x >= maxBoundX)
        {
            MoveToRight = false;
        }
        else if (transform.position.x <= minBoundX)
        {
            MoveToRight = true;
        }
    }

    public override EnemyBase Run()
    {
        MoveToRight = Random.Range(0, 2) == 0;
        base.Run();
        isRun = true;

        return this;
    }

    public override void ReturnToPool()
    {
        isRun = false;
        base.ReturnToPool();
    }
}
