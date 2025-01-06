using System;
using System.Collections.Generic;
using DG.Tweening;
using Michael.Scripts.Controller;
using Michael.Scripts.Manager;
using Michael.Scripts.UI;
using Michael.Scripts.Upgrade;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



    
    public class ShopManager : MonoBehaviour
    {
        
        [SerializeField] private TextMeshProUGUI _currentGoldText;
        [SerializeField] private TextMeshProUGUI _currentDamageText;
        [SerializeField] private TextMeshProUGUI _currentFireRateText;
        [SerializeField] private TextMeshProUGUI _currentBarrelText;
        [SerializeField] private TextMeshProUGUI _currentMaxGoldText;
        
        [SerializeField] private List<Button> _buyButtons;
        
        private void LateUpdate()
        {
            _currentGoldText.text = "" + PlayerData.Instance.CurrentGold;
            GameManager.Instance.CheckUpgradesCost();
        }

        private void Start()
        {
            UpdateStatsText();
        }
        
        
        

        private void UpdateStatsText()
        {
            _currentDamageText.text = "Degats: " + PlayerData.Instance.BulletDamage;
            _currentFireRateText.text = "Cadence de tir: " + PlayerData.Instance.FireRate + "s";
            _currentBarrelText.text = "Tonneaux explo: " + PlayerData.Instance.MaxExplosifBarrel;
            _currentMaxGoldText.text = "Capacite d'or: " + PlayerData.Instance.MaxGoldCapacity;
        }
        

      
        public void PurchaseUpgrade(Button button)
        {
            Upgrade upgrade = button.GetComponent<Upgrade>();
            if (PlayerData.Instance.CurrentGold > upgrade.CurrentCost)
            {
                SoundManager.PlaySound(SoundType.OpenChest);
                PlayerData.Instance.CurrentGold -= upgrade.CurrentCost;
                SoundManager.PlaySound(SoundType.OpenChest);
                upgrade.ApplyUpgrade();
               // UpdateButtons(button.gameObject);
            }
            UpdateStatsText();
            PlayerData.Instance.UpdatePlayerGold();

        }
        
    

     
    }

    

    
 
