using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Box : MonoBehaviour
{
    [SerializeField] private MeshRenderer wholeBox;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private GameObject fracturedBox;
    [SerializeField] private AudioSource crashAudioClip;

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
        wholeBox.enabled = false;
        boxCollider.enabled = false;
        fracturedBox.SetActive(true);
        crashAudioClip.Play();        

        DOVirtual.DelayedCall(_crushedBoxLivetime, Deactivate);

        DOVirtual.DelayedCall(_crushedBoxLivetime, () => IsCrushedBoxDeactivated?.Invoke());        
    }    

    public void Activate(int minMoney, int maxMoney)
    {
        gameObject.SetActive(true);
        wholeBox.enabled = true;            
        fracturedBox.SetActive(false);

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