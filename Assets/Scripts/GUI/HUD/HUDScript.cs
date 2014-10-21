using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour {

    [SerializeField]
    private TextAreaScript _affichageMines;
    [SerializeField]
    private TextAreaScript _affichageBombes;
    private ManagerScript _manager;
    private int _nbMines;
    [SerializeField]
    private GlassPaneScript _glassPane;
	[SerializeField]
	private GameObject _ecranFinPartie;
    [SerializeField]
    private TimerScript _timer;
    [SerializeField]
    private GameObject _ecranPause;
    //Variable qui indicque les informations de la partie sur les morts et les points gagnés en cours de partie
    [SerializeField]
    private TextAreaScript _InfoKill;
    //Variable qui indicque les informations général de la partie 
    [SerializeField]
    private TextAreaScript _texteCentral;
    [SerializeField]
    private TextAreaScript _Score;
    [SerializeField]
    private DecompteScript _decompte;

    void Start()
    {
        _manager = ManagerScript.Instance;
        _nbMines = 0;
        AjouterMine();
		_ecranFinPartie.SetActive(false);
        _ecranPause.SetActive(false);
    }

    public void AjouterMine()
    {
        _affichageMines.SetText((++_nbMines).ToString());
    }

    public void RetirerMine()
    {
        _affichageMines.SetText((--_nbMines).ToString());
    }

    public void AfficherBombesDispo()
    {
        int nbBombs = _manager.Player.NbBombs;
        _affichageBombes.SetText(nbBombs.ToString());
    }

    public void AfficherEcranFinPartie(string NomJoueurGagnant)
    {
        _timer.Stop = true;
        _glassPane.StartGlassPane();
        _texteCentral.SetText("<FF0000>Partie terminée !\n" + NomJoueurGagnant + " a Gagné");
        _ecranFinPartie.SetActive(true);
    }

    public void AfficherEcranPause(bool afficher)
    {
        if (afficher)
            _glassPane.StartGlassPane();
        else
            _glassPane.StopGlassPane();

        _ecranPause.SetActive(afficher);
        _manager.InGame = !afficher;
    }

    /// <summary>
    /// Affiche les information principale
    /// </summary>
    public void AfficherPhase2()
    {
        _texteCentral.SetText("<FF0000>ATTENTION UNE BOMBE H !\n     PROTEGEZ VOUS !");
        StartCoroutine(EffacerTexteCentral());
    }

    /// <summary>
    /// Affiche les gains de points gagner lors d'une phase de jeu passée
    /// </summary>
    public void AfficherPhasePasse()
    {
        _InfoKill.SetText("+100 Point");
        StartCoroutine(EffacerInfoKill());
    }
    /// <summary>
    /// indique quel joueur a tué quel joueur 
    /// </summary>
    public void AfficherJoueurTuer(string NomJoueurTuer, string NomJoueurTueur, bool Ac, int score)
    {
        _InfoKill.SetText(NomJoueurTueur + " a tué " + NomJoueurTuer + "\n+" + score + " Point");
        StartCoroutine(EffacerInfoKill());
    }
    /// <summary>
    /// indique quel joueur a pris le Bouclier
    /// </summary>
    public void AfficherJoueurAuBouclier(string NomJoueur)
    {
        _texteCentral.SetText(NomJoueur + "<FF0000> a récupéré le bouclier ");
        StartCoroutine(EffacerTexteCentral());
    }
    /// <summary>
    /// Coroutine qui rythme le temps d'affichage des informations à l'écran
    /// </summary>
    private IEnumerator EffacerInfoKill()
    {
        yield return new WaitForSeconds(3f);
        _InfoKill.SetText("");
    }
    private IEnumerator EffacerTexteCentral()
    {
        yield return new WaitForSeconds(3f);
        _texteCentral.SetText("");
    }

     public void Score(int point)
    {
        _manager.Player.Points += point;
        _manager.Player.Score += point;
        _manager.Points += point;
        string score = _manager.Player.Score.ToString();
        _Score.SetText(score);
    }

    void Update()
    {
        if (Input.GetKeyUp(_manager.Pause))
        {
            AfficherEcranPause(_manager.InGame);
        }
    }

    public void LancerDecomptePhase2()
    {
        _decompte.StartDecompte();
    }
}
