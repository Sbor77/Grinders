using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _bigboxCamera;
    [SerializeField] private CinemachineVirtualCamera _playCamera;
    [SerializeField] private Joystick _josytick;
    [SerializeField] private float _playCameraPositionY;    
    [SerializeField] private Vector3 _targetBigboxCameraPosition;
    [SerializeField] private float _bigboxCameraTime;
    [SerializeField] private float _bigboxCameraDelay;

    private void Start()
    {
        MoveBigboxCamera();

        SetJoystickActive(false);
    }

    private void Activate (CinemachineVirtualCamera camera)
    {
        camera.Priority = 1;        
    }

    private void SetJoystickActive(bool active)
    {
        _josytick.gameObject.SetActive(active);
    }

    private void MoveBigboxCamera()
    {
        _bigboxCamera.transform.DOMove(_targetBigboxCameraPosition, _bigboxCameraTime).OnComplete(() =>
        {
            Deactivate(_bigboxCamera, _bigboxCameraDelay);

            DOVirtual.DelayedCall(_bigboxCameraTime, () => SetJoystickActive(true));
        });
    }

    private void Deactivate(CinemachineVirtualCamera camera, float delay = 0)
    {
        DOVirtual.DelayedCall(delay, () => camera.Priority = 0);        
    }
}