using Michael.Scripts.Controller;
using Unity.VisualScripting;

namespace Michael.Scripts.Upgrade
{
    public class DamageUpgrade : Upgrade
    {
        public override void Start()
        {
            base.Start();
           // ValueText.text = PlayerData.Instance.BulletDamage.ToString();
        }


        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();
            IncrementValueText.text = " + " + IncrementValue;
            PlayerData.Instance.BulletDamage += (int)IncrementValue;
            ValueText.text = PlayerData.Instance.BulletDamage.ToString();
        }
    }
}
