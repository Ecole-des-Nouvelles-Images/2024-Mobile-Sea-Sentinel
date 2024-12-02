using UnityEngine;

namespace Michael.Scripts
{
    [CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/Upgrade")]
    public class Upgrade : ScriptableObject
    {
        public string UpgradeName;
        public string Description;
        public int BaseCost;
        public int CostIncrement; 
        public UpgradeType Type; 
        public float Value;
    }
    
    public enum UpgradeType
    {
        Damage,
        FireRate,
        MaxGold,
        ExtraProjectile
    }
}