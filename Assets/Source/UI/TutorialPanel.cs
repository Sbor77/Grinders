using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private GameObject _infoPanel;
    [Space]    
    [SerializeField] private Image _missionPad;
    [SerializeField] private Image _attackPad;    
    [Space]
    [SerializeField] private TMP_Text _targetGuides;
    [SerializeField] private TMP_Text _bigboxGuides;
    [Space]    
    [SerializeField] private GameObject _playerGuides;
    [SerializeField] private TMP_Text _walkGuides;
    [SerializeField] private TMP_Text _walkGuidesMobile;
    [SerializeField] private TMP_Text _attackGuides;
    [SerializeField] private TMP_Text _attackGuidesMobile;
    [SerializeField] private TMP_Text _attackGuidesCommon;
    [Space]
    [SerializeField] private Button _okButton;
    [SerializeField] private TMP_Text _start;
    [SerializeField] private TMP_Text _next;

    private float _hintDuration = 0.5f;
    private int _hintLoops = -1;
    private int _clickCounts;

    private float _textFadeDuration = 1;
    private float _textPause = 1.5f;

    private void Start()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            Time.timeScale = 0;

            _playerGuides.SetActive(true);

            Fade(_targetGuides, 1, 1);

            FadeOut(_missionPad);            

            DOVirtual.DelayedCall(_textPause, () => Fade(_bigboxGuides, 1, _textFadeDuration));
        });

        //AnimateFade(_coinPad, 0, _hintDuration, _hintLoops);
    }

    private void OnEnable()
    {
        _okButton.onClick.AddListener(OnClickButton);
    }

    private void OnDisable()
    {
        _okButton.onClick.RemoveListener(OnClickButton);
    }

    private void OnClickButton()
    {        
        if (_clickCounts == 0)
        {
            _next.gameObject.SetActive(false);

            _start.gameObject.SetActive(true);

            _targetGuides.gameObject.SetActive(false);            

            _bigboxGuides.gameObject.SetActive(false);

            _missionPad.gameObject.SetActive(false);            

            if (DataHandler.Instance.IsMobile())            {

                /*_attackGuidesMobile.gameObject.SetActive(true);

                _walkGuidesMobile.gameObject.SetActive(true);*/

                DOVirtual.DelayedCall(_textPause, () => Fade(_walkGuidesMobile, 1, _textFadeDuration));                

                DOVirtual.DelayedCall(_textPause * 2, () => Fade(_attackGuidesMobile, 1, _textFadeDuration));

            }
            else
            {
                /*_attackGuides.gameObject.SetActive(true);

                _walkGuides.gameObject.SetActive(true);*/

                Fade(_walkGuides, 1, _textFadeDuration);

                DOVirtual.DelayedCall(_textPause, () => 
                {
                    Fade(_attackGuides, 1, _textFadeDuration);
                    FadeOut(_attackPad);
                });

            }

            //_attackGuidesCommon.gameObject.SetActive(true);

            DOVirtual.DelayedCall(_textPause * 3, () => Fade(_attackGuidesCommon, 1, _textFadeDuration));

            _clickCounts++;
        }

        else if (_clickCounts >= 1)
        {
            gameObject.SetActive(false);

            _infoPanel.gameObject.SetActive(true);

            Time.timeScale = 1;
        }        
    }

    private void FadeOut(Image image)
    {
        image.gameObject.SetActive(true);

        image.DOFade(0, _hintDuration).SetLoops(_hintLoops, LoopType.Yoyo).OnComplete(() =>
        {
            image.gameObject.SetActive(false);
        }).SetUpdate(true);
    }

    private void Fade(TMP_Text text, float alpha, float duration)
    {
        text.gameObject.SetActive(true);

        text.DOFade(alpha, _hintDuration).SetUpdate(true).SetEase(Ease.InSine).SetUpdate(true);
    }
}