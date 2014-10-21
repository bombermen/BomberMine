using UnityEngine;
using System.Collections;

public class AnimeMiniMap : MonoBehaviour {

    [SerializeField]
    private MiniMapScript _miniMapScrip;

    private bool flag = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (_miniMapScrip.FoudroiementActif)
        {
            if (flag)
                this.animation.Play("MiniMap");
            else
                Debug.Log("anime play");
            if (this.animation["MiniMap"].time > 5f)
            {
                this.animation.Stop("MiniMap");
                Debug.Log("anime stop");
                flag = false;
            }
        }
	
	}
}
