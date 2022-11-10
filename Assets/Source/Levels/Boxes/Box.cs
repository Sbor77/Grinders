using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Box : MonoBehaviour
{
    [SerializeField] private List<GameObject> _pieces;
    [SerializeField] private MeshRenderer _wholeBoxRenderer;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private GameObject _fracturedBox;
    [SerializeField] private AudioSource _crashAudioClip;
    [SerializeField] private Coin _coin;

    private List<Vector3> _piecesDefaultPositions = new();
    private int _money;
    private float _crushedBoxLivetime = 3f;
    private int _randomMaxAngle = 90;
    private bool _isCoinCollectable;
        
    public event Action IsCoinCollected;

    public GameObject FracturedBox => _fracturedBox;

    public int Money => _money;

    private void Start()
    {
        SaveDefaultPiecesPositions();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            if (player.CurrentState == State.Move && _coin.gameObject.activeSelf && _isCoinCollectable == true)
            {
                _coin.AnimateCollection();

                IsCoinCollected?.Invoke();                    

                print("Собрали монетку номиналом " + _money);

                _isCoinCollectable = false;

                DOVirtual.DelayedCall(3f, DeactivateWholeBox);
            }

            if (player.CurrentState == State.Attack && _wholeBoxRenderer.enabled)
            {
                Crush();

                print("Крашим ящик!");
            }
        }
    }    

    public void Crush()
    {
        _wholeBoxRenderer.enabled = false;
        _boxCollider.enabled = true;
        _fracturedBox.SetActive(true);
        _crashAudioClip.Play();

        _coin.Activate();

        DOVirtual.DelayedCall(_crushedBoxLivetime, () =>
        {
            DeactivateCrushedBox();
            _isCoinCollectable = true;
            RestoreDefaultPiecesPositions();
        });
    }    

    public void ActivateWholeBox(int minMoney, int maxMoney)
    {
        gameObject.SetActive(true);
        _wholeBoxRenderer.enabled = true; 
        _boxCollider.enabled = true;
        _fracturedBox.SetActive(false);
        
        _coin.Deactivate();        
        
        GenerateMoney(minMoney, maxMoney);

        GenerateRotationY();
    }

    public void DeactivateWholeBox()
    {
        gameObject.SetActive(false);        
    }

    private void DeactivateCrushedBox()
    {
        _fracturedBox.SetActive(false);
    }

    private void GenerateRotationY()
    {
        int randomAngleY = UnityEngine.Random.Range(0, _randomMaxAngle);

        transform.eulerAngles = new Vector3(0, randomAngleY, 0);
    }

    private void GenerateMoney(int minMoney, int maxMoney)
    {
        _money = UnityEngine.Random.Range(minMoney, maxMoney + 1);

        _isCoinCollectable = false;
    }

    private void SaveDefaultPiecesPositions()
    {
        for (int i = 0; i < _pieces.Count; i++)
        {
            _piecesDefaultPositions.Add(_pieces[i].transform.position);
        }        
    }

    private void RestoreDefaultPiecesPositions()
    {
        for (int i = 0; i < _pieces.Count; i++)
        {
            _pieces[i].transform.position = _piecesDefaultPositions[i];
        }
    }
}