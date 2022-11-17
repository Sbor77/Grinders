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

    private Vector3 _defaultBigboxCameraPosition;

    private void Start()
    {
        _defaultBigboxCameraPosition = _bigboxCamera.transform.position;

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

            DOVirtual.DelayedCall(_bigboxCameraTime + 1f, () => 
            {
                _bigboxCamera.transform.position = _defaultBigboxCameraPosition;

                SetJoystickActive(true);
            });
        });
    }

    public void ZoomOutBigboxCamera()
    {
        float defaultFieldOfView = _bigboxCamera.m_Lens.FieldOfView;

        float targetFieldOfView = 90;

        float switchTime = 3f;

        float waitingTime = 1f;

        Sequence sequence = DOTween.Sequence();

        Deactivate(_playCamera);

        Activate(_bigboxCamera);

        sequence.AppendCallback(() => DOVirtual.Float(defaultFieldOfView, targetFieldOfView, switchTime, f => _bigboxCamera.m_Lens.FieldOfView = f));
        sequence.AppendInterval(switchTime + waitingTime);
        sequence.AppendCallback(() => DOVirtual.Float(targetFieldOfView, defaultFieldOfView, switchTime, f => _bigboxCamera.m_Lens.FieldOfView = f));
        sequence.AppendInterval(switchTime);
        sequence.AppendCallback(() =>
        {
            //Deactivate(_bigboxCamera);
            //Activate(_playCamera);
        });
    }

    private void Deactivate(CinemachineVirtualCamera camera, float delay = 0)
    {
        DOVirtual.DelayedCall(delay, () => camera.Priority = 0);        
    }
}