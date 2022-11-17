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
    [SerializeField] private BoxItem _item;

    private List<Vector3> _piecesDefaultPositions = new();    
    private bool _isItemCollectable;    
    private float _boxDeactivationDelay = 3f;
        
    public event Action IsItemCollected;    

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

                if (GetComponent<Coin>())                
                    player.AddMoney(_item.Value);                

                if (GetComponent<Cross>())                
                    player.Heal(_item.Value);

                _isItemCollectable = false;

                IsItemCollected?.Invoke();

                DOVirtual.DelayedCall(_boxDeactivationDelay, DeactivateWholeBox);                
            }

            if (player.CurrentState == State.Attack && _wholeBoxRenderer.enabled)
            {
                Crush();

                //print("������ ����!");
            }
        }
    }    

    public void Crush()
    {
        float crushedBoxLivetime = 3f;

        _wholeBoxRenderer.enabled = false;
        _boxCollider.enabled = true;
        _fracturedBox.SetActive(true);
        _crashAudioClip.Play();

        _item.Activate();

        DOVirtual.DelayedCall(crushedBoxLivetime, () =>
        {
            DeactivateCrushedBox();
            _isItemCollectable = true;
            RestoreDefaultPiecesPositions();
        });
    }    

    public void ActivateWholeBox(int minValue, int maxValue)
    {
        gameObject.SetActive(true);
        _wholeBoxRenderer.enabled = true; 
        _boxCollider.enabled = true;
        _fracturedBox.SetActive(false);

        _item.GenerateValue(minValue, maxValue);

        _item.Deactivate();        

        GenerateRotationY();

        SaveDefaultPiecesPositions();
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
        int randomMaxAngle = 90;

        int randomAngleY = UnityEngine.Random.Range(0, randomMaxAngle);

        transform.eulerAngles = new Vector3(0, randomAngleY, 0);
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
        }
    }
}