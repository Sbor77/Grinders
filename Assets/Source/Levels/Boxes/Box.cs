using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Box : MonoBehaviour
{
    [SerializeField] private MeshRenderer _wholeBoxRenderer;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private GameObject _fracturedBox;
    [SerializeField] private AudioSource _crashAudioClip;
    [SerializeField] private Coin _coin;

    private int _money;
    private float _crushedBoxLivetime = 3f;
    private int _randomMaxAngle = 90;
    private bool _isCoinCollected;

    public event Action IsCrushedBoxDeactivated;

    public GameObject FracturedBox => _fracturedBox;

    public int Money => _money;

    private void OnTriggerEnter(Collider other)
    {
        if (_coin.gameObject.activeSelf && _isCoinCollected == false)
        {
            _coin.Collect();

            _isCoinCollected = true;
                        
            print("�������� �������!");
        }

        if (_wholeBoxRenderer.enabled)            
        {
            Crush();

            print("������ ����!");
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
            IsCrushedBoxDeactivated?.Invoke();
        });
    }    

    public void Activate(int minMoney, int maxMoney)
    {
        gameObject.SetActive(true);
        _wholeBoxRenderer.enabled = true; 
        _boxCollider.enabled = true;
        _fracturedBox.SetActive(false);
        
        _coin.Deactivate();        
        
        GenerateMoney(minMoney, maxMoney);

        GenerateRotationY();
    }

    public void Deactivate()
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
        _money = UnityEngine.Random.Range(minMoney, maxMoney);

        _isCoinCollected = false;
    }
}