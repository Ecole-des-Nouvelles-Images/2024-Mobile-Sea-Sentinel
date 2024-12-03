using Michael.Scripts.Controller;
using Unity.VisualScripting;

namespace Michael.Scripts.Upgrade
{
    public class DamageUpgrade : Upgrade
    {
        public override void Start()
        {
            base.Start();
            ValueText.text = PlayerData.Instance.BulletDamage.ToString();
        }


        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();
            PlayerData.Instance.BulletDamage += IncrementValue;
            ValueText.text = PlayerData.Instance.BulletDamage.ToString();
        }
    }
}
