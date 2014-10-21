using UnityEngine;
using System.Collections;

public class CapsuleScript : MonoBehaviour
{

    public NetworkScript _networkScript;
    public static int _nbCapsules = 0;
    private int _idCapsule = 0;
    private bool _enAttenteDeConfirmation = false;

    [SerializeField]
    private GameObject _door1;
    [SerializeField]
    private GameObject _door2;
    [SerializeField]
    private CapsuleEntreeColliderScript _cecs;

    private AnimationState _door1Animation;
    private string _door1AnimationLabel = "CapsuleDoorAnimation";
    private AnimationState _door2Animation;
    private string _door2AnimationLabel = "CapsuleDoor2Animation";

    private bool _opened = false;

    void Start()
    {
		Reset();
    }
	
	public void Reset()
	{
		//doorAnimations
        _door1Animation = _door1.animation[_door1AnimationLabel];
        _door2Animation = _door2.animation[_door2AnimationLabel];
		
		_cecs.Reset();
	}

    void OnTriggerEnter(Collider col)
    {
        if (Network.isServer && col.gameObject.layer == 8)
        {
            //int playerIndex = col.transform.parent.GetComponent<JoueurScript>().PlayerIndex;
            _networkScript.EntrerDansCapsule(col.transform, IdCapsule);
        }
    }

    public void OpenDoors()
    {
        _door1Animation.time = Mathf.Clamp(_door1Animation.time, 0f, _door1Animation.clip.length);
        _door1Animation.speed = 1f;
        _door1.animation.Play(_door1AnimationLabel);

        _door2Animation.time = Mathf.Clamp(_door2Animation.time, 0f, _door2Animation.clip.length);
        _door2Animation.speed = 1f;
        _door2.animation.Play(_door2AnimationLabel);
    }

    public void CloseDoors()
    {
        _door1Animation.time = Mathf.Clamp(_door1Animation.time, 0f, _door1Animation.clip.length);
        _door1Animation.speed = -1f;
        _door1.animation.Play(_door1AnimationLabel);

        _door2Animation.time = Mathf.Clamp(_door2Animation.time, 0f, _door2Animation.clip.length);
        _door2Animation.speed = -1f;
        _door2.animation.Play(_door2AnimationLabel);
    }

    void OnMouseDown()
    {
        if (_opened)
        {
            CloseDoors();
        }
        else
        {
            OpenDoors();
        }

        _opened = !_opened;
    }

    public int IdCapsule
    {
        get { return _idCapsule; }
        set { _idCapsule = value; }
    }
}
