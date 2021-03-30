using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Transform spawnTransform;
    [Space]
    [SerializeField] int prespawnCount;
    [SerializeField] PooledObject[] prefabs;

    private Dictionary<System.Type, List<PooledObject>> pooledObjs = new Dictionary<System.Type, List<PooledObject>>();


    private void Awake()
    {
        pooledObjs.Add(typeof(Platform), new List<PooledObject>());
        pooledObjs.Add(typeof(Coin), new List<PooledObject>());
        pooledObjs.Add(typeof(IncreaseNumEffect), new List<PooledObject>());
        pooledObjs.Add(typeof(StayEnemy), new List<PooledObject>());
        pooledObjs.Add(typeof(WalkEnemy), new List<PooledObject>());

        for (int i = 0; i < prespawnCount; i++)
        {

            SpawnObject<Platform>(Vector3.zero, Quaternion.identity, setActive: false);
            SpawnObject<Coin>(Vector3.zero, Quaternion.identity, setActive: false);
            SpawnObject<IncreaseNumEffect>(Vector3.zero, Quaternion.identity, setActive: false);
            SpawnObject<StayEnemy>(Vector3.zero, Quaternion.identity, setActive: false);
            SpawnObject<WalkEnemy>(Vector3.zero, Quaternion.identity, setActive: false);
        }
    }

    private T SpawnObject<T>(in Vector3 pos, Quaternion rot, bool setActive = true) where T: PooledObject
    {
        T obj = null;

        foreach (var item in prefabs)
        {
            var component = item.GetComponent<T>();
            if (component != null)
            {
                obj = (T)Instantiate(item, pos, rot, spawnTransform);
                pooledObjs[typeof(T)].Add(obj);

                if (setActive)
                    obj.SetActive();
                else
                    obj.gameObject.SetActive(false);

                return obj;
            }
        }

        if (obj == null)
            Debug.LogWarning($"Prefab {typeof(T).Name} not found");

        return obj;
    }

    public T GetObject<T>(in Vector3 pos, in Quaternion rot) where T : PooledObject
    {
        T obj = null;
        System.Type key = typeof(T);

        if (!pooledObjs.ContainsKey(key))
            pooledObjs.Add(key, new List<PooledObject>());


        foreach (var item in pooledObjs[key])
        {
            if (!item.IsActive)
            {
                obj = (T)item;
                obj.transform.position = pos;
                obj.transform.rotation = rot;
                obj.SetActive();

                return obj;
            }
        }

        if (obj == null)
        {
            return SpawnObject<T>(pos, rot);
        }

        return null;
    }

}
