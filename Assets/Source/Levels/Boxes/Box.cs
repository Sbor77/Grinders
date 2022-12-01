using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(MeshRenderer))]
public class Box : MonoBehaviour
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _shadedMaterial;
    [SerializeField] private List<GameObject> _pieces;
    [SerializeField] private MeshRenderer _wholeBoxRenderer;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private GameObject _fracturedBox;
    [SerializeField] private AudioSource _crashAudio;
    [SerializeField] private AudioSource _confettiAudio;
    [SerializeField] private BoxItem _item;
    [SerializeField] private ParticleSystem _fogEffect;
    [SerializeField] private ParticleSystem _burstEffect;    

    private MeshRenderer _meshRenderer;
    private List<Vector3> _piecesDefaultPositions = new();    
    private bool _isItemCollectable;    
    private float _boxDeactivationDelay = 3f;    
    private bool _isBigbox;
        
    public event Action IsItemCollected;

    public bool IsBigbox => _isBigbox;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();        
    }

    private void Start()
    {
        SaveDefaultPiecesPositions();        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            if (player.CurrentState == State.Move && _item.gameObject.activeSelf && _isItemCollectable == true)
            {
                _item.AnimateCollection();

                if (GetComponentInChildren<Coin>())                    
                    player.AddMoney(_item.Value);                

                if (GetComponentInChildren<Cross>())                
                    player.Heal(_item.Value);

                _isItemCollectable = false;

                IsItemCollected?.Invoke();

                DOVirtual.DelayedCall(_boxDeactivationDelay, DeactivateWholeBox);                
            }                         
        }
    }    

    public void Crush()
    {
        if (!_wholeBoxRenderer.enabled)
            return;

        float crushedBoxLivetime = 3f;
        float pieceColliderDeactivationDelay = 2f;

        _wholeBoxRenderer.enabled = false;

        _boxCollider.enabled = true;

        _fracturedBox.SetActive(true);

        _burstEffect.gameObject.SetActive(true);

        DOVirtual.DelayedCall(pieceColliderDeactivationDelay, () => 
        {
            DeactivatePiecesColliders();
            _burstEffect.gameObject.SetActive(false);
        });

        _crashAudio.Play();

        DOVirtual.DelayedCall(_crashAudio.time, _confettiAudio.Play);

        _item.Activate();

        DOVirtual.DelayedCall(crushedBoxLivetime, () =>
        {
            DeactivateCrushedBox();
            _isItemCollectable = true;
            RestoreDefaultPiecesPositions();
        });
    }

    public void ActivateWholeBox(int minValue, int maxValue, bool isBigbox = false)
    {
        float fogDelay = 0.75f;

        _wholeBoxRenderer.enabled = false;

        gameObject.SetActive(true);
                
        _fogEffect.Play();

        DOVirtual.DelayedCall(fogDelay, () =>
        {
            _fogEffect.Stop();

            _wholeBoxRenderer.enabled = true;

            _boxCollider.enabled = true;

            _fracturedBox.SetActive(false);

            _item.GenerateValue(minValue, maxValue);

            _item.Deactivate();

            GenerateRotationY();

            SaveDefaultPiecesPositions();            
        });

        if (isBigbox)
        {
            //_isBigbox = true;

            _boxCollider.enabled = false;

            SetShadedMaterial();

            //DOVirtual.DelayedCall(3f, () => SetShadedMaterial());
        }
    }

    public void DeactivateWholeBox()
    {
        gameObject.SetActive(false);        
    }

    public void ShowActiveBox()
    {
        _boxCollider.enabled = false;

        _meshRenderer.material = _defaultMaterial;     
    }

    private void SetShadedMaterial()
    {
        _meshRenderer.material = _shadedMaterial;
    }

    private void DeactivateCrushedBox()
    {
        _fracturedBox.SetActive(false);
    }

    private void GenerateRotationY()
    {
        int randomMaxAngle = 90;

        int randomAngleY = UnityEngine.Random.Range(0, randomMaxAngle);

        transform.eulerAngles = new Vector3(0, randomAngleY, 0);
    }   

    private void DeactivatePiecesColliders()
    {
        for (int i = 0; i < _pieces.Count; i++)
        {
            _pieces[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;

            _pieces[i].gameObject.GetComponent<MeshCollider>().enabled = false;           
        }
    }

    private void SaveDefaultPiecesPositions()
    {
        for (int i = 0; i < _pieces.Count; i++)
        {
            _piecesDefaultPositions.Add(_pieces[i].transform.localPosition);
        }        
    }

    private void RestoreDefaultPiecesPositions()
    {
        for (int i = 0; i < _pieces.Count; i++)
        {
            _pieces[i].transform.localPosition = _piecesDefaultPositions[i];

            _pieces[i].gameObject.GetComponent<Rigidbody>().isKinematic = false;

            _pieces[i].gameObject.GetComponent<MeshCollider>().enabled = true;
        }
    }
}