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

    public void ZoomInPlayCamera()
    {
        SetJoystickActive(false);

        float defaultFieldOfView = _playCamera.m_Lens.FieldOfView;

        float targetFieldOfView = 35;                

        float waitingTime = 2f;

        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(waitingTime);
        sequence.AppendCallback(() => DOVirtual.Float(defaultFieldOfView, targetFieldOfView, 2, f => _playCamera.m_Lens.FieldOfView = f));
        sequence.AppendInterval(3);
        sequence.AppendCallback(() => DOVirtual.Float(targetFieldOfView, defaultFieldOfView, 5, f => _playCamera.m_Lens.FieldOfView = f));
        sequence.AppendInterval(6);
        sequence.AppendCallback(() => SetJoystickActive(true));

    }

    public void ZoomInOutBigboxCamera()
    {
        SetJoystickActive(false);

        Vector3 targetPosition = new Vector3(3, 20, -13);

        _bigboxCamera.transform.position = targetPosition;

        float defaultFieldOfView = _bigboxCamera.m_Lens.FieldOfView;

        float targetFieldOfView = 40;

        float zoomTime = 3f;

        float waitingTime = 2.5f;

        Sequence sequence = DOTween.Sequence();

        Deactivate(_playCamera);

        Activate(_bigboxCamera);

        sequence.AppendInterval(waitingTime);
        sequence.AppendCallback(() => DOVirtual.Float(defaultFieldOfView, targetFieldOfView, zoomTime, f => _bigboxCamera.m_Lens.FieldOfView = f));
        sequence.AppendInterval(waitingTime + zoomTime);
        sequence.AppendCallback(() => DOVirtual.Float(targetFieldOfView, defaultFieldOfView, zoomTime, f => _bigboxCamera.m_Lens.FieldOfView = f));        
        sequence.AppendCallback(() =>
            {
                Deactivate(_bigboxCamera);            
                Activate(_playCamera);            
            });
        sequence.AppendInterval(2);
        sequence.AppendCallback( () => SetJoystickActive(true));
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
    private void Deactivate(CinemachineVirtualCamera camera, float delay = 0)
    {
        DOVirtual.DelayedCall(delay, () => camera.Priority = 0);        
    }
}