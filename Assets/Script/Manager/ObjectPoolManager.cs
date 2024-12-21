using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ns
{
	public class ObjectPoolManager : SingletonBase<ObjectPoolManager>
	{
		[SerializeField] private string[] initList;
		[SerializeField] private int[] initNumList;
		private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();

		protected override void Awake()
        {
			base.Awake();
			CreatePool();
		}

		private void CreatePool()
        {
			for(int i = 0; i < initList.Length; ++i)
            {
				CreateNewPool(initList[i], initNumList[i]);
            }
        }

		public void CreateNewPool(string name, int num)
        {
			if (objectPool.ContainsKey(name)) return;
			Queue<GameObject> q = new Queue<GameObject>();
			objectPool.Add(name, q);
			for(int i = 0; i < num; ++i)
            {
				GameObject prop = ResourceManager.Instance.GetPrefab(name);
				if (prop == null) return;
				GameObject go = Instantiate(prop, transform);
				PutIntoPool(name, go);
			}
        }

		public void PutIntoPool(string name, GameObject go)
        {
			if (go == null) return;
			if (!objectPool.ContainsKey(name))
            {
				CreateNewPool(name, 0);

			}
			PoolObject po = go.GetComponent<PoolObject>();
			if (po != null) po.StopActive();
			go.transform.SetParent(transform);
			go.SetActive(false);
			objectPool[name].Enqueue(go);
		}

		public GameObject GetObjectFromPool(string name)
        {
			if (!objectPool.ContainsKey(name))
			{
				CreateNewPool(name, 0);

			}
			if (objectPool[name].Count == 0)
			{
				GameObject prop = ResourceManager.Instance.GetPrefab(name);
				if (prop == null) return null;
				GameObject go = Instantiate(prop, transform);
				return go;
			}
			else
            {
				GameObject go = objectPool[name].Dequeue();
				go.SetActive(true);
				PoolObject po = go.GetComponent<PoolObject>();
				if (po != null) po.StartActive();
				return go;
			}
		}
	}
}