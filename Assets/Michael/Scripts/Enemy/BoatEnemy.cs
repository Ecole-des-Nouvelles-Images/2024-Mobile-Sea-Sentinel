using DG.Tweening;
using Michael.Scripts.Controller;
using Michael.Scripts.Manager;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Slider = UnityEngine.UI.Slider;
using Sequence = DG.Tweening.Sequence;

namespace Michael.Scripts.Enemy
{
  
    
    public class BoatEnemy : MonoBehaviour
    {
        [Header("Boat Data")]
        public BoatType BoatType;
        public int CurrentBoatGold = 0;
        public int BoatGoldMax; 
        private int _maxHealth;
        private int _currentHealth;
        
        [SerializeField] private Transform _boatModel;
        [SerializeField] private TextMeshProUGUI _boatGoldText;
        [SerializeField] private ParticleSystem _explosionParticles;
        [SerializeField] private TextMeshProUGUI _damageNumberText;
        [SerializeField] Slider _boatHealthBar;
        [SerializeField] Slider _boatEaseHealthBar;
        [SerializeField] private GameObject _boatUi;
        [SerializeField] private AnimationCurve _bounceFeedback;
        [SerializeField] private float _lerpSpeed = 0.05f;
        [SerializeField] CanvasGroup _damageImageCanvasGroup;
        [SerializeField] private GameObject _chest;
        private GameObject _playerTarget;
        private NavMeshAgent _navMeshAgent;
        private bool _hasThief ;
        private Vector3 _initialPosition;
        private Sequence _damageNumberSequence;
        private Vector3 _originalPosition;
        private GameObject[] _playerChests;
        

        void Start() {
            
            _initialPosition = transform.position;
           // UpgradeStats(WaveManager.Instance.SpeedIncrement,WaveManager.Instance.GoldIncrement,WaveManager.Instance.HealthIncrement);
            _boatGoldText.text = CurrentBoatGold + "/" + BoatGoldMax;
            GetNearestTarget();
            FollowTarget(_playerTarget.transform.position);
            _boatHealthBar.maxValue = _maxHealth;
            _boatEaseHealthBar.maxValue = _maxHealth;
            _originalPosition = _damageNumberText.transform.position;
            
            
        }

        public void InitializeBoatStats()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = BoatType.Speed;
            BoatGoldMax = BoatType.GoldCapacity;
            _maxHealth = BoatType.MaxHealth;
            
          /*  if (Random.value <= WaveManager.Instance._currentWaveData.BoatWithGoldPourcent)
            {
                CurrentBoatGold = Mathf.FloorToInt(BoatGoldMax * 0.2f);
                Debug.Log("as gold");
            }
            else {
                CurrentBoatGold = 0;
            }*/
        }

        public void SetGoldOnBoat(int gold)
        {
            CurrentBoatGold = gold;
        }
        
        public void UpgradeStats()
        {
            _navMeshAgent.speed = WaveManager.Instance.SpeedIncrement * WaveManager.Instance._upgradeNumber;
            BoatGoldMax = WaveManager.Instance.GoldIncrement * WaveManager.Instance._upgradeNumber;
            _maxHealth = WaveManager.Instance.HealthIncrement * WaveManager.Instance._upgradeNumber;
            
            Debug.Log("stats upgraded");
        }

    
        void Update() {
            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(PlayerData.Instance.BulletDamage);
            }
            
            if (!_hasThief) return;
            
            if (!(_navMeshAgent.remainingDistance < 1)) return;
            WaveManager.Instance._spawnedBoats.Remove(gameObject);
            Destroy(gameObject);
        }

        private void FollowTarget(Vector3 destination) {
            _navMeshAgent.SetDestination(destination);
        }


        private void OnTriggerEnter(Collider other) {
        
            if (other.CompareTag("Bullet")) {
               
               TakeDamage(PlayerData.Instance.BulletDamage);
                Debug.Log("bateau touché");
            }

            if (other.CompareTag("Player")) {

                if (!_hasThief)
                {
                    StealGold(other.gameObject);
                }
                _hasThief = true;
            }
        }
        
        [ContextMenu("Take Damage")]
        public void TakeDamage(int damage)
        {
            HealthBarFeedback(_boatUi);
            _currentHealth -= damage;
            _boatHealthBar.value = _currentHealth; 
           _boatModel.DOShakePosition( 1f,new Vector3(0.2f,0,0.2f),4).SetEase(Ease.InBounce);
            Sequence feedBackSequence = DOTween.Sequence();
            feedBackSequence.Append( _damageImageCanvasGroup.DOFade(0.2f, 0.1f).SetEase(Ease.Linear));
            feedBackSequence.Append( _damageImageCanvasGroup.DOFade(0f, 0.1f).SetEase(Ease.Linear));
            feedBackSequence.Play();
            _boatEaseHealthBar.DOValue( _currentHealth, _lerpSpeed);
            ShowDamageNumber(damage);
            if (_currentHealth <= 0)
            {
                WaveManager.Instance._spawnedBoats.Remove(gameObject);
                Instantiate(_explosionParticles, transform.position, Quaternion.identity);
                GameManager.Instance.ShakeCamera();
                DestroyBoat();
            }
           
        }

        private void StealGold(GameObject target)
        {
            Debug.Log("StealGold");
            int goldtoSteal = BoatGoldMax - CurrentBoatGold;
            PlayerData.Instance.CurrentGold -= goldtoSteal;
            CurrentBoatGold = BoatGoldMax;
            HealthBarFeedback( PlayerData.Instance.goldText.gameObject);
            _boatGoldText.text = CurrentBoatGold + "/" + BoatGoldMax;
            target.transform.DOShakePosition(1, Vector3.one).SetEase(Ease.InBounce);
            FollowTarget(_initialPosition);
        }
        
        

        private void DestroyBoat()
        {
            // activer version en plusieurs morceaux
            // opacité shader
            Destroy(gameObject);
            if (CurrentBoatGold >= 1 )
            { 
                GameObject chest = Instantiate(_chest, transform.position,transform.rotation);
                chest.GetComponent<Chest>().ChestGold = CurrentBoatGold;
            }
        }

        private void HealthBarFeedback(GameObject ui)
        {
            ui.transform.DOScale(1.2f, 0.5f).SetEase(_bounceFeedback);
        }
        
        private void ShowDamageNumber(int damage)
        {

            _damageNumberText.text = "-" + damage;
            _damageNumberText.gameObject.transform.localPosition = new Vector3(0,5,0);
            _damageNumberSequence.Kill();
            _damageNumberSequence = DOTween.Sequence();
            _damageNumberText.gameObject.SetActive(true);
            _damageNumberSequence.Join(_damageNumberText.gameObject.transform.DOMoveY(_originalPosition.y, 0.3f).SetEase(Ease.OutQuad));
            _damageNumberSequence.Join((_damageNumberText.DOFade(1f, 0.25f)));
            _damageNumberSequence.Append((_damageNumberText.DOFade(0f, 0.25f)));
            _damageNumberSequence.Play();
            _damageNumberSequence.OnComplete(() => { _damageNumberText.gameObject.SetActive(false);
            });
        }

        public void GetNearestTarget()
        {
            _playerChests = GameObject.FindGameObjectsWithTag("Player");
            GameObject nearestChest = _playerChests[0];
            float nearestDistance = Vector3.Distance(gameObject.transform.position, nearestChest.transform.position);

            for (int i = 1; i < _playerChests.Length; i++)
            {
                float distance = Vector3.Distance(gameObject.transform.position, _playerChests[i].transform.position);
                if (distance < nearestDistance)
                {
                    nearestChest = _playerChests[i];
                    distance = nearestDistance;
                }
            }
            _playerTarget = nearestChest;
        }
        
    }
}
