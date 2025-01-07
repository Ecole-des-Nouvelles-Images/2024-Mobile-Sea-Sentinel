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
        public int MaxExplosifBarrel = 0;
        public int CurrentExplosifBarrel;
        public int BarrelDamage;
        
        public TextMeshProUGUI goldText;
        public TextMeshProUGUI ExplosiveBarrelText;
        private void Start()
        {
            CurrentExplosifBarrel = MaxExplosifBarrel;
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
            if (CurrentGold <= 0)
            {
                CurrentGold = 0;
                Debug.Log("gold a zero");
                goldText.color = Color.red;
              //  GameManager.Instance.GameOver();
                
            }
            else
            {
                goldText.color = Color.white;
            }
            if (CurrentGold >= MaxGoldCapacity)
            {
                CurrentGold = MaxGoldCapacity;
            }
            
            goldText.text =  goldText.text = CurrentGold + " / " + MaxGoldCapacity;
        }

      
        
        
        
        
        
        
    }
}
