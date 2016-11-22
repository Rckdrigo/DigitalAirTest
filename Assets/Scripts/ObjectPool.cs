using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{

	public static ObjectPool instance;

	[System.Serializable]
	public struct PrefabPool
	{
		public GameObject prefab;
		public int amountInBuffer;
	};

	public PrefabPool[] prefabs;

	public List<GameObject>[] generalPool;
	protected GameObject containerObject;

	void Awake ()
	{
		instance = this; 
		containerObject = new GameObject ("ObjectPool");
		generalPool = new List<GameObject>[prefabs.Length];

		int index = 0;
		foreach (PrefabPool objectPrefab in prefabs) {
			generalPool [index] = new List<GameObject> ();
			for (int i = 0; i < objectPrefab.amountInBuffer; i++) {
				GameObject temp = Instantiate (objectPrefab.prefab) as GameObject;
				temp.name = objectPrefab.prefab.name;
				PoolGameObject (temp);
			}
			index++;
		}
	}
    
	// Update is called once per frame
	public void PoolGameObject (GameObject obj)
	{
		for (int i = 0; i < prefabs.Length; i++) {
			if (prefabs [i].prefab.name == obj.name) {
				obj.SetActive (false);
				obj.transform.SetParent (containerObject.transform);
				obj.transform.position = containerObject.transform.position;
				generalPool [i].Add (obj);
			}
		}
	}

	public void PoolGameObjectActive (GameObject obj)
	{
		for (int i = 0; i < prefabs.Length; i++) {
			if (prefabs [i].prefab.name == obj.name) {
				obj.transform.parent = containerObject.transform;
				generalPool [i].Add (obj);
			}
		}
	}


	public GameObject GetGameObjectOfType (string objectType, bool onlyPooled = true)
	{
		for (int i = 0; i < prefabs.Length; i++) {
			GameObject prefab = prefabs [i].prefab;
			if (prefab.name == objectType) {
				if (generalPool [i].Count > 0) {
					GameObject pooledObject = generalPool [i] [0];
					pooledObject.transform.SetParent (null);
					generalPool [i].RemoveAt (0);
					pooledObject.SetActive (true);
					return pooledObject;
				} else if (!onlyPooled) {
					return Instantiate (prefabs [i].prefab) as GameObject;
				}
				break;
			}
		}
		return null;
	}
}
