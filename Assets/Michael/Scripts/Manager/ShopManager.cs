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
        
        [SerializeField] private TextMeshProUGUI currentGoldText;
        [SerializeField] private List<Button> buyButtons;
        private void Update()
        {
          
        }

        private void Start()
        {
            currentGoldText.text = "" + PlayerData.Instance.CurrentGold;
            foreach (var button in buyButtons) 
            {
                Upgrade upgrade = button.GetComponent<Upgrade>();
                if (PlayerData.Instance.CurrentGold > upgrade.CurrentCost)
                {
                    upgrade.CostText.color = Color.red;
                }
            }
            
            
        }

      
        public void PurchaseUpgrade(Button button)
        {
            Upgrade upgrade = button.GetComponent<Upgrade>();
            if (PlayerData.Instance.CurrentGold > upgrade.CurrentCost)
            {
                PlayerData.Instance.CurrentGold -= upgrade.CurrentCost;
                upgrade.ApplyUpgrade();
                UpdateButtons(button.gameObject);
                
                if (PlayerData.Instance.CurrentGold < upgrade.CurrentCost)
                {
                    upgrade.CostText.color = Color.red;
                }
            }
            else
            {
                upgrade.CostText.color = Color.red;
            }
            currentGoldText.text = "" + PlayerData.Instance.CurrentGold;
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

    
 
