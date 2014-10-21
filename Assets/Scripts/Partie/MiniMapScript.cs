using UnityEngine;
using System.Collections;

public class MiniMapScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _objetsAFoudroyer;
    private int choixDeLObjet = 0;
    private int clic = 0;
    private float _seconds = 10;
    private bool _foudroiementActif = false;
    private const int NB_BOMBS = 5;
    private ManagerScript _manager;
    private int _playerIndex;
    [SerializeField]
    private HUDScript _hud;
    [SerializeField]
    private GameObject[] _bombesAFoudroyer;
    [SerializeField]
    private BombScript[] _bombesScript;
    [SerializeField]
    private string _nameAnime;
    [SerializeField]
    private NetworkScript _networkScript;

    void Start()
    {
        _manager = ManagerScript.Instance;
        _playerIndex = _manager.PlayerIndex;

        foreach (GameObject bomb in BombesAFoudroyer)
            bomb.SetActive(false);
    }

    void Update()
    {

        if (_foudroiementActif)
        {
            this.animation.Play(_nameAnime);
            if (Input.GetKeyUp(_manager.Bombe) && clic < NB_BOMBS)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray))
                {
                    Vector3 positionBombe = ray.GetPoint(10);
                    //LancerBombe(positionBombe, _playerIndex);
                    this.networkView.RPC("LancerBombe", RPCMode.All, positionBombe, _playerIndex, clic);
                    clic++;
                }
            }
        }

        if (this.animation[_nameAnime].time > 5f)
        {
            this.animation[_nameAnime].time = 0.30f;
            this.animation[_nameAnime].speed = -1f;
        }
    }

    [RPC]
    private void LancerBombe(Vector3 positionBombe, int playerIndex, int bombeIndex)
    {
        GameObject bomb = BombesAFoudroyer[bombeIndex];
        BombScript bombScript = _bombesScript[bombeIndex];

        bomb.SetActive(true);
        bomb.transform.position = positionBombe;
        bombScript.NetworkScript = _networkScript;
        bombScript.IsFoudroyee = true;
        bombScript.Hud = _hud;
        bombScript.BombIndex = playerIndex;
    }

    public bool FoudroiementActif
    {
        get { return _foudroiementActif; }
        set
        {
            _foudroiementActif = value;
            if (_foudroiementActif)
            {
                clic = 0;
                _manager.Player.NbBombs += NB_BOMBS;
                _hud.AfficherBombesDispo();
            }
        }
    }

    public GameObject[] BombesAFoudroyer
    {
        get { return _bombesAFoudroyer; }
        set { _bombesAFoudroyer = value; }
    }
}
