using System.Collections;
using System.Collections.Generic;
using Michael.Scripts.Controller;
using Michael.Scripts.Upgrade;
using UnityEngine;

public class BarrelUpgrade : Upgrade
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        IncrementValueText.text = "+ " + IncrementValue;

    }

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        PlayerData.Instance.ExplosifBarrelNumber += (int)IncrementValue;
        IncrementValueText.text = "+ " + IncrementValue;
    }
}
