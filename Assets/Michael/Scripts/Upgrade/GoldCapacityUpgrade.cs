using Michael.Scripts.Controller;

namespace Michael.Scripts.Upgrade
{
    public class GoldCapacityUpgrade : Upgrade
    {
        public override void Start()
        {
            base.Start();
          //  ValueText.text = PlayerData.Instance.MaxGoldCapacity.ToString();
        }


        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();
            PlayerData.Instance.MaxGoldCapacity += IncrementValue;
            ValueText.text = PlayerData.Instance.MaxGoldCapacity.ToString();
        }
    }
    
}
