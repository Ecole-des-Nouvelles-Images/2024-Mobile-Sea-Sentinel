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
        ValueText.text = PlayerData.Instance.FireRate.ToString();
    }

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        PlayerData.Instance.FireRate += IncrementValue;
        ValueText.text = PlayerData.Instance.FireRate.ToString();
    }
}
