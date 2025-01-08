using System;
using DG.Tweening;
using Michael.Scripts.Controller;
using Michael.Scripts.Manager;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Michael.Scripts.Enemy
{
   public class Chest : MonoBehaviour
   {
      public int  ChestGold;
      [SerializeField] private float _sinkDuration;
      [SerializeField] private bool _canRecovered = false;
      [SerializeField] private GameObject _chestTop; 
      [SerializeField] private GameObject _button;
      [SerializeField] private ParticleSystem _coinParticles;
      [SerializeField] private GameObject _chestParticles;
      private Sequence _sinkSequence;

      [Header("coin animation")]
      [SerializeField] private GameObject _coinPrefabs;
      [SerializeField] private Vector3 _startCoinPosition;
      [SerializeField] private Vector3 _endCoinPosition;
      [SerializeField] private float _coinAnimDuration;
      [SerializeField] private int _coinAmount; 
      [SerializeField] private float _coinTotalDelay;
     [SerializeField] private float _coinPerDelay;
      
      private void Start()
      {
         Vector3 pos = new Vector3(transform.position.x,transform.position.y,transform.position.z);
         _button.transform.SetParent( GameManager.Instance._canvas.transform);
         _button.transform.SetAsFirstSibling();
         _button.transform.position = Camera.main.WorldToScreenPoint(pos);

         _startCoinPosition = Camera.main.WorldToScreenPoint(pos);
         _endCoinPosition = Camera.main.WorldToScreenPoint(Vector3.zero);
      }
      
      private void GoldCoinAnimation(float delay)
      {
         GameObject coinObject = Instantiate(_coinPrefabs, GameManager.Instance._canvas.transform);
         coinObject.transform.SetAsFirstSibling();
         Vector3 offset = new Vector3(Random.Range(-50, 50), Random.Range(-60, 60), 0f);
         Vector3 startPos = offset + _startCoinPosition;
         coinObject.transform.position = startPos; 
         
         coinObject.transform.localScale = Vector3.one;
         coinObject.transform.DOScale(Vector3.one, delay);

         coinObject.transform.DOMove(_endCoinPosition, _coinAnimDuration).SetEase(Ease.InBack).SetDelay(delay).OnComplete(() =>
            {
               PlayerData.Instance.CurrentGold += ChestGold;
               if ( PlayerData.Instance.CurrentGold >  PlayerData.Instance.MaxGoldCapacity)
               {
                  PlayerData.Instance.CurrentGold = PlayerData.Instance.MaxGoldCapacity;
               }
               PlayerData.Instance.UpdatePlayerGold();
               ChestGold = 0;
               SinkChest();
               Destroy(coinObject);
            });
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
            _canRecovered = true;
         }
      }

      private void SinkChest()
      {
         _chestParticles.SetActive(false);
         _sinkSequence = DOTween.Sequence();
        // SoundManager.PlaySound(SoundType.Sinking);
         _sinkSequence.Join(gameObject.transform.DOMove(new Vector3(transform.position.x, -8, transform.position.z), 2f));
         _sinkSequence.Join(gameObject.transform.DORotate(new Vector3(transform.position.x,transform.position.y , 20), 2f));
         _sinkSequence.OnComplete(() =>
         {
            WaveManager.Instance.ChestGameObjects.Remove(gameObject);
            Destroy(gameObject);
         });
         _sinkSequence.Play();
      }


   
      
      
      
      

      public void RecoverGold()
      {
         //animation piece 
         //animation coffre qui s'ouvre 
         _canRecovered = true;
         Destroy(_button);
         SoundManager.PlaySound(SoundType.OpenChest);
         _chestParticles.SetActive(false);
         _coinParticles.Play();
         _chestTop.transform.DOLocalRotate(new Vector3(-60f, 0, 0), 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
         { 
            SoundManager.PlaySound(SoundType.GoldIn);
            // animation gold
            _coinPerDelay = _coinTotalDelay / _coinAmount;
            for (int i = 0; i < _coinAmount; i++)
            {
               float targetDelay = i * _coinPerDelay;
               GoldCoinAnimation(targetDelay);
            }
         });
      }
      
      
   }
}
