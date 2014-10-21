using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	
	private AnimationState _buttonAnimation;
	private string _animationLabel = "ButtonAnimation";
	
	void Start ()
	{
		_buttonAnimation = this.animation[_animationLabel];
	}

	void OnMouseDown ()
	{
		_buttonAnimation.time = Mathf.Clamp(_buttonAnimation.time, 0f, _buttonAnimation.clip.length);
		_buttonAnimation.speed = 1f;
		animation.Play(_animationLabel);
	}
	
	void OnMouseUp ()
	{
		_buttonAnimation.time = Mathf.Clamp(_buttonAnimation.time, 0f, _buttonAnimation.clip.length);
		_buttonAnimation.speed = -1f;
		animation.Play(_animationLabel);
	}
}
