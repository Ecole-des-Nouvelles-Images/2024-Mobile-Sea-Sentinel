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
        base.ApplyUpgrade();
        PlayerData.Instance.FireRate -= IncrementValue;
        IncrementValueText.text = "- " + IncrementValue;
    }
}


