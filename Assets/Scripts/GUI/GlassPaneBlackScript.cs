using UnityEngine;
using System.Collections;

public class GlassPaneBlackScript : MonoBehaviour {

	private GameObject _glassPaneGameObject;
	private string _animationLabel = "GlassPaneBlackAnimation";
	private AnimationState _animationState;
	private float _duration;

	public float Duration {
		get {
			return this._duration;
		}
		set {
			_duration = value;
		}
	}	
	
	public void StartGlassPane ()
	{
		_glassPaneGameObject.SetActive(true);
		_animationState.time = Mathf.Clamp(_animationState.time, 0, _animationState.length);
		_animationState.speed = 1f;
		animation.Play();
	}
	
	public void StopGlassPane ()
	{
		_animationState.time = Mathf.Clamp(_animationState.time, 0, _animationState.length);
		_animationState.speed = -1f;
		animation.Play();
		StartCoroutine(WaitAndDeactivate());
	}
	
	void Start () {
		_glassPaneGameObject = gameObject;	
		_animationState = animation[_animationLabel];
		_glassPaneGameObject.SetActive(false);
		_duration = _animationState.length;
	}
	
	IEnumerator WaitAndDeactivate()
	{
		yield return new WaitForSeconds(_animationState.length);
		_glassPaneGameObject.SetActive(false);
	}
}
