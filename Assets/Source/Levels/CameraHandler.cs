using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _bigboxCamera;
    [SerializeField] private CinemachineVirtualCamera _playCamera;
    [SerializeField] private Joystick _josytick;    
    [SerializeField] private Transform _startBigboxCameraPoint;
    [SerializeField] private Transform _targetBigboxCameraPoint;        
    
    public void ActivateStartScenario()
    {
        float startDelay = 1;

        _bigboxCamera.transform.position = _startBigboxCameraPoint.position;

        SetJoystickActive(false);

        DOVirtual.DelayedCall(startDelay, () => MoveBigboxCamera());
    }

    public void ZoomInPlayCamera()
    {
        float defaultFieldOfView = _playCamera.m_Lens.FieldOfView;
        float targetFieldOfView = 35;
        float waitingTime = 2f;

        SetJoystickActive(false);

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
        float defaultFieldOfView = _bigboxCamera.m_Lens.FieldOfView;
        float targetFieldOfView = 45;
        float zoomTime = 1f;
        float waitingTime = 2.5f;

        SetJoystickActive(false);

        Deactivate(_playCamera);

        _bigboxCamera.transform.position = _startBigboxCameraPoint.position;

        Activate(_bigboxCamera);

        Sequence sequence = DOTween.Sequence();

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
        sequence.AppendCallback(() => SetJoystickActive(true));        
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
        float distance = Vector3.Distance(_targetBigboxCameraPoint.position, _startBigboxCameraPoint.position);
        float speed = 22f;
        float joystickDelay = 3f;
        float bigboxCameraDelay = 0.5f;


        _bigboxCamera.transform.DOMove(_targetBigboxCameraPoint.position, distance / speed).OnComplete(() =>
        {
            Deactivate(_bigboxCamera, bigboxCameraDelay);

            DOVirtual.DelayedCall(joystickDelay, () =>
            {
                _bigboxCamera.transform.position = _startBigboxCameraPoint.position;
                SetJoystickActive(true);
            });            
        });
    }

    private void Deactivate(CinemachineVirtualCamera camera, float delay = 0)
    {
        DOVirtual.DelayedCall(delay, () => camera.Priority = 0);        
    }
}