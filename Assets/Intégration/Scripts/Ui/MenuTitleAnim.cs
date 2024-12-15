using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MenuTitleAnim : MonoBehaviour
{ 
   
        [SerializeField] private GameObject titleText; 
        [SerializeField] private float animationDuration = 2f;
        [SerializeField] private float bounceDuration = 0.5f;
        [SerializeField] private CanvasGroup mainMenu;
        void Awake() {
            
            ShowTitle();
        }
        void ShowTitle() {
            
            Sequence synthwaveSequence = DOTween.Sequence();
            synthwaveSequence.Append(titleText.transform.DOScale(1.5f, animationDuration / 2).SetEase(Ease.OutExpo));
            synthwaveSequence.Append(titleText.transform.DOScale(1f, animationDuration / 2).SetEase(Ease.InFlash));
            synthwaveSequence.Join(titleText.GetComponent<CanvasGroup>().DOFade(1f, animationDuration/2));
            synthwaveSequence.AppendCallback(TitleEffect);
            synthwaveSequence.Play();
        }
        void TitleEffect()
        {
            StartBounceAnimation();
            MainMenuAnimation();
        }
        void StartBounceAnimation() {
            titleText.transform.DOScale(1.05f, bounceDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
    
        void MainMenuAnimation()
        {
            Sequence menuSequence = DOTween.Sequence();
            menuSequence.Append(mainMenu.transform.DOScale(1.1f, 0.5f).SetEase(Ease.OutExpo));
            menuSequence.Append(mainMenu.transform.DOScale(1f, 0.5f).SetEase(Ease.InFlash));
            menuSequence.Join(mainMenu.DOFade(1, 1f));
            mainMenu.interactable = true;
        }


    
}
