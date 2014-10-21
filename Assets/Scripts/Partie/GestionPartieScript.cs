using UnityEngine;
using System.Collections;

public class GestionPartieScript : MonoBehaviour
{
    [SerializeField]
    private HUDScript _HUD;
    public NetworkScript _networkScript;

    private float _tempsPhase1 = 100f;
    private float _tempsPhase2 = 10f;
    private int _sequenceActuelle = 1;

    public float TempsPhase1
    {
        get { return _tempsPhase1; }
        set { _tempsPhase1 = value; }
    }
    public float TempsPhase2
    {
        get { return _tempsPhase2; }
        set { _tempsPhase2 = value; }
    }

    // Use this for initialization
    void Start()
    {
        if (Network.isServer)
        {
            StartCoroutine(Phase1());
        }
    }

    private IEnumerator Phase1()
    {
        _networkScript.ChangerDePhase(1, _sequenceActuelle);
        yield return new WaitForSeconds(TempsPhase1);
        StartCoroutine(Phase2());
    }

    private IEnumerator Phase2()
    {
        _networkScript.ChangerDePhase(2, _sequenceActuelle);
        _networkScript.DebutDePhase2();
        yield return new WaitForSeconds(TempsPhase2);

        if (_sequenceActuelle < 4)
        {
            _sequenceActuelle++;
            _networkScript.JoueurSaufApresPhase2();
            StartCoroutine(Phase1());
        }
        else
        {
            _networkScript.FinPartie();
        }
    }
}
