/*
 * Inactif jusqu'à toucher un objet
 * Attend ensuite _seconds secondes avant de détruire tous les objets
 * dans le rayon _explosionMagnitude
 * Les objets dans ce rayon mais qui sont cachés par un autre objet sont protégés
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class BombScript : MonoBehaviour
{
    [SerializeField]
    private HUDScript _hud;
    public float _explosionMagnitude = 5;
    public Rigidbody _rigidbody;
    public float _seconds = 3;
    private NetworkScript _networkScript;

    public NetworkScript NetworkScript
    {
        get { return _networkScript; }
        set { _networkScript = value; }
    }
    private int _bombIndex;
    private bool poser = true;

    public bool Poser
    {
        get { return poser; }
        set {
            poser = value;
            _bombeDeclenchee = value;
        }
    }
    bool _bombeDeclenchee = false;
    private ManagerScript _manager;
    private bool _isMine;
    private bool _isFoudroyee = false;

    void Awake()
    {
        _manager = ManagerScript.Instance;
        _bombeDeclenchee = false;
        poser = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if (!_bombeDeclenchee)
        {
            _bombeDeclenchee = true;
            StartCoroutine(Declencher(_seconds));
        }
    }

    private IEnumerator Declencher(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        destroyAround();
    }

    void destroyAround()
    {

        List<GameObject> hitObjects = new List<GameObject>();
        Vector3 center = transform.position;
        int res = 1000;

        for (int i = 0; i < res; i++)
        {
            Vector3 fwd = new Vector3(Mathf.Cos(i / 2 / Mathf.PI), 0, Mathf.Sin(i / 2 / Mathf.PI));

            Ray ray = new Ray(center, fwd);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _explosionMagnitude))
            {
                GameObject ob = hit.collider.gameObject;

                if (ob.layer != 11 && !hitObjects.Contains(ob))
                {
                    hitObjects.Add(hit.collider.gameObject);
                }
            }
        }

        if (_isMine && !_isFoudroyee)
        {
            _manager.AjouterAuxBombesDuJoueur(BombIndex, +1);
            Hud.AfficherBombesDispo();
        }

        if (Network.isServer)
        {
            NetworkScript.ExploserBombe(_bombIndex, hitObjects);
        }

        poser = true;
        transform.parent.gameObject.SetActive(false);
    }

    public HUDScript Hud
    {
        get { return _hud; }
        set { _hud = value; }
    }

    public bool IsFoudroyee
    {
        get { return _isFoudroyee; }
        set { _isFoudroyee = value; }
    }

    public int BombIndex
    {
        get { return _bombIndex; }
        set
        {
            _bombIndex = value;
            _isMine = _manager.PlayerIndex == _bombIndex;
            if (_isMine)
            {
                _manager.AjouterAuxBombesDuJoueur(BombIndex, -1);
                Hud.AfficherBombesDispo();
            }
        }
    }
}
