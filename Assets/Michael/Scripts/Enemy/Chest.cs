using System;
using DG.Tweening;
using Michael.Scripts.Controller;
using Michael.Scripts.Manager;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Michael.Scripts.Enemy
{
   public class Chest : MonoBehaviour
   {
      public int  ChestGold;
      [SerializeField] private float _sinkDuration;
      [SerializeField] private bool _canRecovered = false;
      [SerializeField] private GameObject _chestTop; 
      [SerializeField] private GameObject _button;
      [SerializeField] private ParticleSystem _coinpParticles;
      private Sequence _sinkSequence ;
      private void Start()
      {
         Vector3 pos = new Vector3(transform.position.x,transform.position.y,transform.position.z);
         _button.transform.SetParent( GameManager.Instance._canvas.transform);
         _button.transform.SetAsFirstSibling();
         _button.transform.position = GameManager.Instance._mainCamera.WorldToScreenPoint(pos);
      }

      private void Update()
      {
         if (_sinkDuration > 0)
         {
            _sinkDuration -= Time.deltaTime;
         }
         if (_sinkDuration <= 0 && !_canRecovered)
         {
            Destroy(_button);
            SinkChest();
         }
      }

      private void SinkChest()
      {
         _sinkSequence = DOTween.Sequence();
         _sinkSequence.Join(gameObject.transform.DOMove(new Vector3(transform.position.x, -8, transform.position.z), 5f));
         _sinkSequence.Join(gameObject.transform.DORotate(new Vector3(transform.position.x,transform.position.y , 20), 5f));
         _sinkSequence.OnComplete(() => { Destroy(gameObject); });
         _sinkSequence.Play();
      }
      
      

      public void RecoverGold()
      {
         //animation piece 
         //animation coffre qui s'ouvre 
         _canRecovered = true;
         Destroy(_button);
         PlayerData.Instance.CurrentGold += ChestGold;
         ChestGold = 0;
         _chestTop.transform.DOLocalRotate(new Vector3(-60f, 0, 0), 2f).SetEase(Ease.OutBounce).OnComplete(() =>
         {
          // _coinpParticles.Play();
            SinkChest();
         });
      }
      
      
   }
}
