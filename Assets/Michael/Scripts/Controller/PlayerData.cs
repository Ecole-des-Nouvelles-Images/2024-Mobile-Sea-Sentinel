using System;
using Michael.Scripts.Manager;
using TMPro;
using UnityEngine;

namespace Michael.Scripts.Controller
{
    public class PlayerData : MonoBehaviourSingleton<PlayerData>
    {
        [Header ("Player Data")]
        public int CurrentGold;
        public  int MaxGoldCapacity = 150;
        public   float FireRate;
        public  int BulletDamage;
        public int ExplosifBarrelNumber = 0;
        public int CurrentExplosifBarrel;
        public int BarrelDamage;
        
        public TextMeshProUGUI goldText;
        public TextMeshProUGUI ExplosiveBarrelText;
        private void Start() 
        {
            CurrentGold = MaxGoldCapacity; 
            UpdateExplosiveBarrelText();
            UpdatePlayerGold();
        }
        
        private void Update()
        {
          
        }
        
        public void UpdateExplosiveBarrelText()
        {
            ExplosiveBarrelText.text = PlayerData.Instance.CurrentExplosifBarrel.ToString();
        }

        public void UpdatePlayerGold()
        {
            goldText.text =  goldText.text = CurrentGold + " / " + MaxGoldCapacity;
            if (CurrentGold <= 0)
            {
                CurrentGold = 0;
                GameManager.Instance.GameOver();
                
            }
            if (CurrentGold >= MaxGoldCapacity)
            {
                CurrentGold = MaxGoldCapacity;
            }
        }

      
        
        
        
        
        
        
    }
}
