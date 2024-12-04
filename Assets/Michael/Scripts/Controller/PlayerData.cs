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
        public  int AlliesBoats; 
        
        
        public TextMeshProUGUI goldText;

        private void Start() 
        {
            CurrentGold = MaxGoldCapacity; 
        }
        
        private void Update()
        {
            goldText.text =  goldText.text = CurrentGold + " / " + MaxGoldCapacity;
            if (CurrentGold <= 0)
            {
                CurrentGold = 0;
                GameManager.Instance.GameOver();
            }
        }


      
        
        
        
        
        
        
    }
}
