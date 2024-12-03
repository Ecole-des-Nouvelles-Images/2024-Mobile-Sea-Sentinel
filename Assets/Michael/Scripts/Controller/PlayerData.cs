using System;
using Michael.Scripts.Manager;
using TMPro;
using UnityEngine;

namespace Michael.Scripts.Controller
{
    public class PlayerData : MonoBehaviour
    {
        public int MaxGoldCapacity = 150;
        public int CurrentGold;
        public float FireRate;
        public float BulletDamage;
        public int AlliesBoats; 
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
