using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _bigboxCamera;
    [SerializeField] private CinemachineVirtualCamera _playCamera;
    [SerializeField] private float _playCameraPositionY;    
    [SerializeField] private Vector3 _targetBigboxCameraPosition;

    private float _bigboxCameraTime = 3f;

    private void Start()
    {
        MoveBigboxCamera();   
    }

    private void Activate (CinemachineVirtualCamera camera)
    {
        camera.VirtualCameraGameObject.SetActive(true);       
    }

    private void MoveBigboxCamera()
    {
        _bigboxCamera.transform.DOMove(_targetBigboxCameraPosition, _bigboxCameraTime).OnComplete(() => Deactivate(_bigboxCamera));
    }

    private void Deactivate(CinemachineVirtualCamera camera)
    {
        camera.VirtualCameraGameObject.SetActive(false);
    }
}