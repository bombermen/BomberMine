using UnityEngine;
using System.Collections;

public class JoueurScript : MonoBehaviour 
{
	private int _playerIndex = -1;
	private bool _dansUneCapsule = false;
	private bool _jAiLeBouclier = false;
	private bool _vivant = true;
	private int _points = 0;
	
	
	public int PlayerIndex {
		get {
			return this._playerIndex;
		}
		set {
			_playerIndex = value;
		}
	}
	
	public bool DansUneCapsule {
		get {
			return this._dansUneCapsule;
		}
		set {
			_dansUneCapsule = value;
		}
	}

	public bool JAiLeBouclier {
		get {
			return this._jAiLeBouclier;
		}
		set {
			_jAiLeBouclier = value;
		}
	}
	/*
	public int[] Mines {
		get {
			return this._mines;
		}
		set {
			_mines = value;
		}
	}*/

	public int Points {
		get {
			return this._points;
		}
		set {
			_points = value;
		}
	}

	public bool Vivant {
		get {
			return this._vivant;
		}
		set {
			_vivant = value;
		}
	}
}
