using UnityEngine;
using System.Collections;

public class TextInputChatScript : MonoBehaviour {
	
	private bool _focus = false;
	private TextAreaScript _tas;
	[SerializeField]
	private TextAreaScript _textAreaMessages;
	
	void Start()
	{
		_tas = GetComponent<TextAreaScript>();
	}
	
	void Update()
	{
		string inputString = Input.inputString;
		if(Input.GetKeyUp(KeyCode.Backspace))
		{
			_tas.EffacerDerniereLettre();
		}
		else if(Input.GetKeyDown(KeyCode.Return))
		{
			if(Network.isClient)
			{
				this.networkView.RPC("EnvoyerMessage", RPCMode.Server, Network.player,_tas.GetText());
			}
			else
			{
				EnvoyerMessage(Network.player,_tas.GetText());
			}
			_tas.SetText("");
		}
		else if(inputString != "" && inputString != "\b")
		{
			_tas.Append(inputString);
		}
	}
	
	[RPC]
	void EnvoyerMessage(NetworkPlayer player, string message)
	{
		//Recuperation de commande /
		string[] mots = message.Split(new char[]{' '});
		//groupe
		if(mots[0] == "/g")
		{
			
		}
		else
		{
			
		}
	}
	
	[RPC]
	void MettreAJourChat(string text)
	{
		_textAreaMessages.SetText(text);
	}
}
