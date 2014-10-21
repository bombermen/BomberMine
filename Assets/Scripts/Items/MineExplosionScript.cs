using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class MineExplosionScript : MonoBehaviour
{
	public float _explosionMagnitude = 2;
	
	private List<GameObject> _objectsToDestroy;

	public List<GameObject> ObjectsToDestroy
	{
		get { return this._objectsToDestroy; }
		set { _objectsToDestroy = value; } 
	}
	
	void Start()
	{
		GetComponent<SphereCollider>().radius = _explosionMagnitude;
		ObjectsToDestroy = new List<GameObject>();
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.layer != 11 && !ObjectsToDestroy.Contains(col.gameObject))
		{
			ObjectsToDestroy.Add(col.gameObject);
            ObjectsToDestroy.Add(this.transform.parent.gameObject);
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		ObjectsToDestroy.Remove(col.gameObject);
	}
}
