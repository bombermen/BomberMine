/*
 * Mouvement de la camera du personnage controllé par la souris
 * La rotation sur Y (dans le repère tridemensionnel d'Unity) s'applique à l'objet Personnage
 * La rotation sur l'axe X local au Personnage est affecté à la caméra
 */

using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {
	
	private Transform _persoTransform;
	private Transform _transform;
	
	private float _sensitivityX = 6F;
	private float _sensitivityY = 6F;

	private float _minimumY = -90F;
	private float _maximumY = 90F;

	float _rotationY = 0F;
	float _rotationX = 0F;
	
	int _gravityDirection;
	
	GameObject _persoAnim;
			
	void Start () {
		_gravityDirection = 1;
		_transform = this.transform;
		_persoTransform = _transform.parent.transform;

	}
	
	void Update () {
		
		/*
		 * Récupération des valeurs de déplacement du curseur sur X et Y de l'écran
		 * La multuplication par gravityDirection permet de changer le sens de rotation sur
		 * l'axe horizontal selon la gravité
		 */
		_rotationX += Input.GetAxis("Mouse X") * _sensitivityX * _gravityDirection;
		_rotationY += Input.GetAxis("Mouse Y") * _sensitivityY;
		
		//restriction de la rotation sur l'axe horizontal entre les valeurs minimales et maximales
		_rotationY = Mathf.Clamp (_rotationY, _minimumY, _maximumY);
		
		
		_transform.localEulerAngles = new Vector3(-_rotationY, 0, 0);
		_persoTransform.localEulerAngles = new Vector3(0, _rotationX, 0);
	}

	public void ChangeGravity() {
		_gravityDirection = -_gravityDirection;
	}
}