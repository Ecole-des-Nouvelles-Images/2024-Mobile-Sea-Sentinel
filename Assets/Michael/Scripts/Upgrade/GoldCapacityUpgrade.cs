using Michael.Scripts.Controller;

namespace Michael.Scripts.Upgrade
{
    public class GoldCapacityUpgrade : Upgrade
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
            PlayerData.Instance.MaxGoldCapacity += (int)IncrementValue;
        }
    }
    
}
