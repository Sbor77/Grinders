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
    [SerializeField] private Coin _coinPrefab;

    private int _money;
    private float _crushedBoxLivetime = 3f;
    private int _randomMaxAngle = 90;

    public event Action IsCrushedBoxDeactivated;

    public int Money => _money;

    private void OnTriggerEnter(Collider other)
    {
        Crush();    
    }    

    public void Crush()
    {
        _wholeBoxRenderer.enabled = false;
        _boxCollider.enabled = false;
        _fracturedBox.SetActive(true);
        _crashAudioClip.Play();

        DOVirtual.DelayedCall(_crushedBoxLivetime, () =>
        {
            Deactivate();
            IsCrushedBoxDeactivated?.Invoke();
        });
    }    

    public void Activate(int minMoney, int maxMoney)
    {
        gameObject.SetActive(true);
        _wholeBoxRenderer.enabled = true; 
        _boxCollider.enabled = true;
        _fracturedBox.SetActive(false);
        
        GenerateMoney(minMoney, maxMoney);

        GenerateRotationY();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);        
    }

    private void GenerateRotationY()
    {
        int randomAngleY = UnityEngine.Random.Range(0, _randomMaxAngle);

        transform.eulerAngles = new Vector3(0, randomAngleY, 0);
    }

    private void GenerateMoney(int minMoney, int maxMoney)
    {
        _money = UnityEngine.Random.Range(minMoney, maxMoney);
    }
}