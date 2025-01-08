using System.Collections;
using System.Collections.Generic;
using Michael.Scripts.Controller;
using Michael.Scripts.Upgrade;
using UnityEngine;
using UnityEngine.UI;

public class FireRateUpgrade : Upgrade
{

    
    public override void Start()
    {
        base.Start(); 
        IncrementValueText.text = "- " + IncrementValue;
       
    }

    public override void ApplyUpgrade()
    {
        
        IncrementValueText.text = "- " + IncrementValue;
        if (PlayerData.Instance.FireRate > 0.4f )
        {
            base.ApplyUpgrade();
            PlayerData.Instance.FireRate -= IncrementValue;
        }
        else
        { 
            CostText.text = "MAX";
            CostText.color = Color.red;
            GetComponent<Button>().interactable = false;
        }


    }
}


