using UnityEngine;
using System.Collections;

public class MineDeclenchementScript : MonoBehaviour {
	
	public MineScript _mineScript;
	public NetworkScript _networkScript;
	public bool _minePoser = false;
	public bool _mineRecuperable = false;
	public int _idMine = 0;
    public GameObject _leJoueur = null;
	
	void OnTriggerEnter(Collider col) 
	{
        if (col.gameObject.layer == 8)
        {
            _mineRecuperable = true;
            _leJoueur = col.gameObject;
        }
	}
    void OnTriggerExit(Collider col) 
	{
        if (_mineScript._mineEnclenchee && col.gameObject.layer == 8)
            _mineScript.Explode(_idMine);
	}
}
