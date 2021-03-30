using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Platform : PooledObject
{
    [SerializeField] Transform spriteTransform;
    [SerializeField] Collider2D collider;

    [Header("Bird landing effect")]
    [SerializeField] float moveDownTime;
    [SerializeField] float moveUpTime;
    [SerializeField] float offsetY;

    private List<PooledObject> floorObjsList = new List<PooledObject>();
    private Coroutine effectCoroutine;
    private Transform transform;
    private Vector3 startVisualPos;

    public int PlatformNum { get; private set; }
    public Transform VisualTransform => spriteTransform;


    private void Awake()
    {
        transform = GetComponent<Transform>();
        startVisualPos = VisualTransform.localPosition;
    }

    public override void ReturnToPool()
    {
        ResetVisual();
        ClearFloorObjects();

        base.ReturnToPool();
    }

    private void ResetVisual()
    {
        StopAllCoroutines();
        effectCoroutine = null;
        VisualTransform.localPosition = startVisualPos;
    }

    private IEnumerator EffectCo()
    {
        bool moveBack = false;
        float startY = spriteTransform.position.y;
        float endY = startY + offsetY;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / moveDownTime;
            spriteTransform.position = new Vector3(0, Mathf.Lerp(startY, endY, t), 0);

            yield return null;
        }

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / moveUpTime;
            spriteTransform.position = new Vector3(0, Mathf.Lerp(endY, startY, t), 0);

            yield return null;
        }

        effectCoroutine = null;
    }

    public void RunBirdLandingEffect()
    {
        if (effectCoroutine != null)
            return;

        effectCoroutine = StartCoroutine(EffectCo());
    }

    public void SetPlatformNum(int num)
    {
        PlatformNum = num;
    }

    public void AddFloorObject(PooledObject obj, bool setAsChild = false)
    {
        floorObjsList.Add(obj);

        if (setAsChild)
            obj.transform.parent = VisualTransform;
    }

    public void AlignObjectY(Transform objTransform, float objSizeY)
    {
        objTransform.position = new Vector3(
            objTransform.position.x,
            collider.bounds.max.y + objSizeY / 2,
            objTransform.position.z);
    }

    public void ClearFloorObjects()
    {
        foreach (var item in floorObjsList)
            item.ReturnToPool();

        floorObjsList.Clear();
    }
}
