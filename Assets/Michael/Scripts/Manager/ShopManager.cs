using System;
using DG.Tweening;
using Michael.Scripts.Controller;
using Michael.Scripts.UI;
using Michael.Scripts.Upgrade;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



    
    public class ShopManager : MonoBehaviour
    {
       

        [SerializeField] private TextMeshProUGUI currentGoldText;
        private void Update()
        {
            currentGoldText.text = "golds actuels : " + PlayerData.Instance.CurrentGold;
        }
        
        public void PurchaseUpgrade(Button button)
        {
            Upgrade upgrade = button.GetComponent<Upgrade>();
            if (PlayerData.Instance.CurrentGold > upgrade.CurrentCost)
            {
                PlayerData.Instance.CurrentGold -= upgrade.CurrentCost;
                upgrade.ApplyUpgrade();
                UpdateButtons(button.gameObject);
            }
            else
            {
                upgrade.CostText.color = Color.red;
            }
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

    
 
