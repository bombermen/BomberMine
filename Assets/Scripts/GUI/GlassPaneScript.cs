using UnityEngine;
using System.Collections;

public class GlassPaneScript : MonoBehaviour {
	
	private GameObject _glassPaneGameObject;
	private string _animationLabel = "GlassPaneColorAnimation";
	private AnimationState _animationState;
	
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
	
	void Start ()
	{
		_glassPaneGameObject = gameObject;	
		_animationState = animation[_animationLabel];
		_glassPaneGameObject.SetActive(false);
	}
	
	IEnumerator WaitAndDeactivate()
	{
		yield return new WaitForSeconds(_animationState.length);
		_glassPaneGameObject.SetActive(false);
	}
}
