/// <summary>
/// Display hosts script.
/// Rafraichi et affiche les N premiers serveurs du meme jeu
/// dans les N boutons radios renseignés.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayHostsScript : MonoBehaviour {
	
	private float _tempsRechercheMax = 10.0f;
	private bool _searching = false;
	private float _debutRecherche;
	
	[SerializeField]
	private ManagerScript _manager;
	
	/// <summary>
	/// The _glass pane.
	/// Script qui gère l'assombrissement de l'ecran pendant la recherche
	/// </summary>
	[SerializeField]
	private GlassPaneScript _glassPane;
	
	//Boutons Radio
	[SerializeField]
	private GameObject[] _radioButtons;
	private RadioButtonScript[] _rbs;
	private TextAreaScript[] _tas;
	
	private HostData[] _hosts;
	
	private const string _gameTypeName = "ESGI_UniTeam_BomberMine";
	
	void Start ()
	{
		//Initialisation des scripts des boutons radio
		_rbs = new RadioButtonScript[_radioButtons.Length];
		_tas = new TextAreaScript[_radioButtons.Length];
		for(int i=0; i<_radioButtons.Length; i++)
		{
			_rbs[i] = _radioButtons[i].GetComponent<RadioButtonScript>();
			_tas[i] = _radioButtons[i].transform.GetChild(2).GetComponent<TextAreaScript>();
		}
		CacherBoutonsRadio();
	}
	
	void OnMouseUp ()
	{
		DisplayHosts();
	}
	
	public void DisplayHosts ()
	{
		MasterServer.RequestHostList(_gameTypeName);
		_searching = true;
		_debutRecherche = Time.time;
		_glassPane.StartGlassPane();
	}
	
	void Update ()
	{
		if(_searching)
		{
			if(MasterServer.PollHostList().Length > 0)
			{
				_searching = false;
				_glassPane.StopGlassPane();
				_hosts = MasterServer.PollHostList();
				AfficherBoutons();
			}
			else if(Time.time > _debutRecherche + _tempsRechercheMax)
			{
				_glassPane.StopGlassPane();
				
				_searching = false;
			}
		}
	}
	
	void AfficherBoutons ()
	{
		CacherBoutonsRadio();
		for(int i=0; i<_hosts.Length; i++)
		{
			AfficherBoutonRadio(i, _hosts[i]);
		}
	}
	
	public void CacherBoutonsRadio ()
	{
		for(int i=0; i<_radioButtons.Length; i++)
		{
			_radioButtons[i].SetActive(false);
			_rbs[i].Selectionne = false;
		}
	}
	
	void AfficherBoutonRadio(int indice, HostData data)
	{
		if(indice < _radioButtons.Length)
		{
			_radioButtons[indice].SetActive(true);
			_rbs[indice].Selectionne = indice == 0;
			_rbs[indice].Data = data;
			_tas[indice].SetText(data.gameName);
		}
	}
	
	public HostData GetHostData()
	{
		for(int i=0; i<_hosts.Length; i++)
			if(_rbs[i].Selectionne)
				return _rbs[i].Data;
		return null;
	}
}
