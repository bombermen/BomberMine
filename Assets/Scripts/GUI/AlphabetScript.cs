/// <summary>
/// Alphabet script.
/// Ce script permet l'interfaçage entre caractères et leur modèle 3D (préfab)
/// </summary>

using UnityEngine;
using System.Collections;

public class AlphabetScript : MonoBehaviour {
	
	//Minuscules
	public GameObject _minusculeA;
	public GameObject _minusculeB;
	public GameObject _minusculeC;
	public GameObject _minusculeD;
	public GameObject _minusculeE;
	public GameObject _minusculeF;
	public GameObject _minusculeG;
	public GameObject _minusculeH;
	public GameObject _minusculeI;
	public GameObject _minusculeJ;
	public GameObject _minusculeK;
	public GameObject _minusculeL;
	public GameObject _minusculeM;
	public GameObject _minusculeN;
	public GameObject _minusculeO;
	public GameObject _minusculeP;
	public GameObject _minusculeQ;
	public GameObject _minusculeR;
	public GameObject _minusculeS;
	public GameObject _minusculeT;
	public GameObject _minusculeU;
	public GameObject _minusculeV;
	public GameObject _minusculeW;
	public GameObject _minusculeX;
	public GameObject _minusculeY;
	public GameObject _minusculeZ;
	//Majuscules
	public GameObject _majusculeA;
	public GameObject _majusculeB;
	public GameObject _majusculeC;
	public GameObject _majusculeD;
	public GameObject _majusculeE;
	public GameObject _majusculeF;
	public GameObject _majusculeG;
	public GameObject _majusculeH;
	public GameObject _majusculeI;
	public GameObject _majusculeJ;
	public GameObject _majusculeK;
	public GameObject _majusculeL;
	public GameObject _majusculeM;
	public GameObject _majusculeN;
	public GameObject _majusculeO;
	public GameObject _majusculeP;
	public GameObject _majusculeQ;
	public GameObject _majusculeR;
	public GameObject _majusculeS;
	public GameObject _majusculeT;
	public GameObject _majusculeU;
	public GameObject _majusculeV;
	public GameObject _majusculeW;
	public GameObject _majusculeX;
	public GameObject _majusculeY;
	public GameObject _majusculeZ;
	//Chiffres
	public GameObject _chiffre0;
	public GameObject _chiffre1;
	public GameObject _chiffre2;
	public GameObject _chiffre3;
	public GameObject _chiffre4;
	public GameObject _chiffre5;
	public GameObject _chiffre6;
	public GameObject _chiffre7;
	public GameObject _chiffre8;
	public GameObject _chiffre9;
	//Autres caractères
	public GameObject _point;
	public GameObject _virgule;
	public GameObject _pointVirgule;
	public GameObject _doublePoint;
	public GameObject _pointExclamation;
	public GameObject _pointInterrogation;
	public GameObject _slash;
	public GameObject _backSlash;
	public GameObject _aAccentGrave;
	public GameObject _accentCirc;
	public GameObject _accoladeOuvrante;
	public GameObject _accoladeFermante;
	public GameObject _apostrophe;
	public GameObject _arobase;
	public GameObject _chevronOuvrant;
	public GameObject _chevronFermant;
	public GameObject _crochetOuvrant;
	public GameObject _crochetFermant;
	public GameObject _dieze;
	public GameObject _eAccentAigu;
	public GameObject _eAccentGrave;
	public GameObject _egal;
	public GameObject _etoile;
	public GameObject _guillemets;
	public GameObject _paragraphe;
	public GameObject _parantheseOuvrante;
	public GameObject _parantheseFermante;
	public GameObject _plus;
	public GameObject _tilde;
	public GameObject _tiret;
	public GameObject _underscore;
	public GameObject _oAccentCirc;
	
	
	/// <summary>
	/// Char to game object.
	/// Correspondance lettre/préfab du modèle de la lettre
	/// </summary>
	/// <returns>
	/// préfab correspondant à la lettre en paramètre.
	/// </returns>
	/// <param name='letter'>
	/// Lettre dont on cherche le prefab.
	/// </param>
	public GameObject CharToGameObject(char letter)
	{
		if(letter == 'a') return _minusculeA;
		else if(letter == 'b') return _minusculeB;
		else if(letter == 'c') return _minusculeC;
		else if(letter == 'd') return _minusculeD;
		else if(letter == 'e') return _minusculeE;
		else if(letter == 'f') return _minusculeF;
		else if(letter == 'g') return _minusculeG;
		else if(letter == 'h') return _minusculeH;
		else if(letter == 'i') return _minusculeI;
		else if(letter == 'j') return _minusculeJ;
		else if(letter == 'k') return _minusculeK;
		else if(letter == 'l') return _minusculeL;
		else if(letter == 'm') return _minusculeM;
		else if(letter == 'n') return _minusculeN;
		else if(letter == 'o') return _minusculeO;
		else if(letter == 'p') return _minusculeP;
		else if(letter == 'q') return _minusculeQ;
		else if(letter == 'r') return _minusculeR;
		else if(letter == 's') return _minusculeS;
		else if(letter == 't') return _minusculeT;
		else if(letter == 'u') return _minusculeU;
		else if(letter == 'v') return _minusculeV;
		else if(letter == 'w') return _minusculeW;
		else if(letter == 'x') return _minusculeX;
		else if(letter == 'y') return _minusculeY;
		else if(letter == 'z') return _minusculeZ;
		else if(letter == 'A') return _majusculeA;
		else if(letter == 'B') return _majusculeB;
		else if(letter == 'C') return _majusculeC;
		else if(letter == 'D') return _majusculeD;
		else if(letter == 'E') return _majusculeE;
		else if(letter == 'F') return _majusculeF;
		else if(letter == 'G') return _majusculeG;
		else if(letter == 'H') return _majusculeH;
		else if(letter == 'I') return _majusculeI;
		else if(letter == 'J') return _majusculeJ;
		else if(letter == 'K') return _majusculeK;
		else if(letter == 'L') return _majusculeL;
		else if(letter == 'M') return _majusculeM;
		else if(letter == 'N') return _majusculeN;
		else if(letter == 'O') return _majusculeO;
		else if(letter == 'P') return _majusculeP;
		else if(letter == 'Q') return _majusculeQ;
		else if(letter == 'R') return _majusculeR;
		else if(letter == 'S') return _majusculeS;
		else if(letter == 'T') return _majusculeT;
		else if(letter == 'U') return _majusculeU;
		else if(letter == 'V') return _majusculeV;
		else if(letter == 'W') return _majusculeW;
		else if(letter == 'X') return _majusculeX;
		else if(letter == 'Y') return _majusculeY;
		else if(letter == 'Z') return _majusculeZ;
		else if(letter == '0') return _chiffre0;
		else if(letter == '1') return _chiffre1;
		else if(letter == '2') return _chiffre2;
		else if(letter == '3') return _chiffre3;
		else if(letter == '4') return _chiffre4;
		else if(letter == '5') return _chiffre5;
		else if(letter == '6') return _chiffre6;
		else if(letter == '7') return _chiffre7;
		else if(letter == '8') return _chiffre8;
		else if(letter == '9') return _chiffre9;
		else if(letter == '.') return _point;
		else if(letter == ',') return _virgule;
		else if(letter == ':') return _doublePoint;
		else if(letter == ';') return _pointVirgule;
		else if(letter == '!') return _pointExclamation;
		else if(letter == '?') return _pointInterrogation;
		else if(letter == '/') return _slash;
		else if(letter == '\\') return _backSlash;
		else if(letter == 'à') return _aAccentGrave;
		else if(letter == '^') return _accentCirc;
		else if(letter == '{') return _accoladeOuvrante;
		else if(letter == '}') return _accoladeFermante;
		else if(letter == '\'') return _apostrophe;
		else if(letter == '@') return _arobase;
		else if(letter == '<') return _chevronOuvrant;
		else if(letter == '>') return _chevronFermant;
		else if(letter == '[') return _crochetOuvrant;
		else if(letter == ']') return _crochetFermant;
		else if(letter == '#') return _dieze;
		else if(letter == 'é') return _eAccentAigu;
		else if(letter == 'è') return _eAccentGrave;
		else if(letter == '=') return _egal;
		else if(letter == '*') return _etoile;
		else if(letter == '"') return _guillemets;
		else if(letter == '§') return _paragraphe;
		else if(letter == '(') return _parantheseOuvrante;
		else if(letter == ')') return _parantheseFermante;
		else if(letter == '+') return _plus;
		else if(letter == '~') return _tilde;
		else if(letter == '-') return _tiret;
		else if(letter == '_') return _underscore;
		else if(letter == 'ô') return _oAccentCirc;

		return null;
	}
}
