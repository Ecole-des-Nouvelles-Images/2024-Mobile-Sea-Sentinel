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
        private void Update()
        {
          
        }

        private void Start()
        {
           UpdateStatsText();
        }
        
        
        

        private void UpdateStatsText()
        {
            _currentGoldText.text = "" + PlayerData.Instance.CurrentGold;
            _currentDamageText.text = "Degats: " + PlayerData.Instance.BulletDamage;
            _currentFireRateText.text = "Cadence de tir: " + PlayerData.Instance.FireRate;
            _currentBarrelText.text = "Tonneaux explo: " + PlayerData.Instance.CurrentExplosifBarrel;
            _currentMaxGoldText.text = "" + PlayerData.Instance.MaxGoldCapacity;
        }
        

      
        public void PurchaseUpgrade(Button button)
        {
            Upgrade upgrade = button.GetComponent<Upgrade>();
            if (PlayerData.Instance.CurrentGold > upgrade.CurrentCost)
            {
                PlayerData.Instance.CurrentGold -= upgrade.CurrentCost;
                upgrade.ApplyUpgrade();
               // UpdateButtons(button.gameObject);
                
                if (PlayerData.Instance.CurrentGold < upgrade.CurrentCost)
                {
                    upgrade.CostText.color = Color.red;
                }
            }
            else
            {
                upgrade.CostText.color = Color.red;
            }
           
            UpdateStatsText();
        }
        
        private void UpdateButtons(GameObject buttons)
        {
          UiFeedback(buttons);
        }

        private void UiFeedback(GameObject ui)
        {
            ui.transform.DOScale(1.2f, 0.5f).SetEase(Ease.InBounce);
        }

     
    }

    

    
 
