using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flow {
	
	public class FlowPool : MonoBehaviour {
		public const int DEFAULT_POOL_SIZE = 10;
		
		public const int MAX_OVERFLOW = 30;
		
		static Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();
		
		static Dictionary<GameObject, GameObject> baseGameObject = new Dictionary<GameObject, GameObject>();
		
		static Dictionary<GameObject, GameObject> parentDictionary = new Dictionary<GameObject, GameObject>();
		
		public static Queue<GameObject> CreatePool(GameObject baseObj, int poolSize) {
			var pool = new Queue<GameObject>();
			var parent = new GameObject(baseObj.name);
			parentDictionary.Add(baseObj, parent);
			for(int i = 0; i < poolSize; i++) {
				var obj = Instantiate(baseObj);
				obj.name = obj.name;
				obj.SetActive(false);
				pool.Enqueue(obj);
				obj.transform.parent = parent.transform;
				baseGameObject.Add(obj, baseObj);
			}
			
			pools.Add(baseObj, pool);
			return pool;
		}
	
		public static GameObject Spawn(GameObject baseObj, Vector3 position, Quaternion rotation) {
			Queue<GameObject> pool;
			if(pools.ContainsKey(baseObj) == false) {
				pool = CreatePool(baseObj, DEFAULT_POOL_SIZE);
			} else {
				pool = pools[baseObj];
			}
			
			GameObject obj;
			if(pool.Count == 0) {
				obj = Instantiate(baseObj);
				var parent = parentDictionary[baseObj];
				obj.transform.parent = parent.transform;
				baseGameObject.Add(obj, baseObj);
			} else {
				obj = pool.Dequeue();
				ResetObj(obj, baseObj);
				if(obj.active) {
					obj.SetActive(false);
				}
				obj.SetActive(true);
			}
			obj.transform.position = position;
			obj.transform.rotation = rotation;
			return obj;
		}
		
		public static GameObject SpawnAndRecycle(GameObject baseObj, Vector3 position, Quaternion rotation) {
			var obj = Spawn(baseObj, position, rotation);
			pools[baseObj].Enqueue(obj);
			return obj;
		}
		
		public static void ResetObj(GameObject obj, GameObject baseObj) {
			obj.transform.localScale = baseObj.transform.localScale;
		}
		
		public static void Recycle(GameObject obj) {
			obj.SetActive(false);
			if(baseGameObject.ContainsKey(obj)) {
				var baseObj = baseGameObject[obj];
				if(pools[baseObj].Count < MAX_OVERFLOW) {
					obj.transform.localScale = baseObj.transform.localScale;
					pools[baseObj].Enqueue(obj);
				} else {
					Destroy(obj, .5f);
				}
			} else {
				Destroy(obj, .5f); //Should we create a new pool for this object? probably not cause it's not the base
			}
		}
		
	}

}