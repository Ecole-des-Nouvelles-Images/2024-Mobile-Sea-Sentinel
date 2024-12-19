using Michael.Scripts.Controller;
using Unity.VisualScripting;

namespace Michael.Scripts.Upgrade
{
    public class DamageUpgrade : Upgrade
    {
        public override void Start()
        {
            base.Start();
            IncrementValueText.text = "+ " + IncrementValue;
           
        }


        public override void ApplyUpgrade()
        {
            
            base.ApplyUpgrade();
            IncrementValueText.text = "+ " + IncrementValue;
            PlayerData.Instance.BulletDamage += (int)IncrementValue;
        }
    }
}
