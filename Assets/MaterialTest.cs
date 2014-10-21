using UnityEngine;
using System.Collections;

public class MaterialTest : MonoBehaviour {

    public Material _material;

	// Use this for initialization
	void Start ()
    {
        Material[] materials = {this.renderer.materials[0], _material};
        this.renderer.materials = materials;
	}
	
}
