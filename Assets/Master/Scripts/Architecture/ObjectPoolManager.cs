using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace RPGTest.Architecture
{
    public class ObjectPoolManager : MonoBehaviour
    {
        private Dictionary<GameObject, ObjectPool<GameObject>> pools = new Dictionary<GameObject, ObjectPool<GameObject>>();

        private void Awake()
        {
            if (!ServiceLocator.Current.Contains<ObjectPoolManager>())
            {
                ServiceLocator.Current.Register<ObjectPoolManager>(this);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (!pools.ContainsKey(prefab))
            {
                pools[prefab] = new ObjectPool<GameObject>(
                    createFunc: () => 
                    {
                        GameObject obj = Instantiate(prefab);
                        PoolableObject poolable = obj.AddComponent<PoolableObject>();
                        poolable.originalPrefab = prefab; 
                        return obj;
                    },
                    actionOnGet: (obj) => obj.SetActive(true),
                    actionOnRelease: (obj) => obj.SetActive(false),
                    actionOnDestroy: (obj) => Destroy(obj),
                    collectionCheck: false, 
                    defaultCapacity: 10,
                    maxSize: 100
                );
            }

            GameObject spawnedObj = pools[prefab].Get();
            
            spawnedObj.transform.position = position;
            spawnedObj.transform.rotation = rotation;
            if (parent != null) spawnedObj.transform.SetParent(parent);

            return spawnedObj;
        }

        public void ReturnToPool(GameObject obj)
        {
            PoolableObject poolable = obj.GetComponent<PoolableObject>();
            
            if (poolable != null && pools.ContainsKey(poolable.originalPrefab))
            {
                obj.transform.SetParent(transform);
                pools[poolable.originalPrefab].Release(obj);
            }
            else
            {
                Destroy(obj);
            }
        }
    }

    public class PoolableObject : MonoBehaviour
    {
        [HideInInspector] public GameObject originalPrefab;
    }
}