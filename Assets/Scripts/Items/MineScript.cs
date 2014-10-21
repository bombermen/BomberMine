using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MineScript : MonoBehaviour
{

    public bool _mineEnclenchee = false;
	public float _seconds = 3;
	public MineExplosionScript _mineExplosionScript;
	public NetworkScript _networkScript;
	
	Transform _transform;
	
	public bool MineEnclenchee
	{
		get { return this._mineEnclenchee; }
		set { _mineEnclenchee = value; }
	}	
	void Start()
	{
		_transform = this.transform;

	}
	
	void OnTriggerEnter(Collider col)
	{
        if (col.gameObject.layer == 11)
			MineEnclenchee = true;
	}

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == 11)
            MineEnclenchee = false;
        if (col.gameObject.layer == 8)
            MineEnclenchee = true;
    }

    public void Explode(int _idMine)
	{
		List<GameObject> objectsToDestroy = _mineExplosionScript.ObjectsToDestroy;

        _networkScript.ExploserMine(_idMine, objectsToDestroy);

		this.transform.parent.gameObject.SetActive(false);
	}
}
