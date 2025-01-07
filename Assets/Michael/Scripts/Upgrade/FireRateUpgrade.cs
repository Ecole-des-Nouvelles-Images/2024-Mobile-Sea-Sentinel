using System.Collections;
using System.Collections.Generic;
using Michael.Scripts.Controller;
using Michael.Scripts.Upgrade;
using UnityEngine;

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
        if (PlayerData.Instance.FireRate > 0 )
        {
            base.ApplyUpgrade();
            PlayerData.Instance.FireRate -= IncrementValue;
        }


    }
}


