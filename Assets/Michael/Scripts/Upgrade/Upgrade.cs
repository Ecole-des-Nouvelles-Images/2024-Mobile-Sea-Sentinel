using Michael.Scripts.Controller;
using TMPro;
using UnityEngine;

namespace Michael.Scripts.Upgrade
{
    public abstract class Upgrade : MonoBehaviour
    {
        public int BaseCost;
        public float CostIncrement; 
        public float IncrementValue;
        public int CurrentCost ;
        public TextMeshProUGUI CostText;
        public TextMeshProUGUI IncrementValueText;
     //   public TextMeshProUGUI ValueText;
        public virtual void Start() 
        {
            CurrentCost = BaseCost;  
            CostText.text = BaseCost.ToString();
        }

        public virtual void ApplyUpgrade()
        {
            CurrentCost = (int)(CurrentCost * CostIncrement);
            CostText.text = CurrentCost.ToString();
        }
    }
}