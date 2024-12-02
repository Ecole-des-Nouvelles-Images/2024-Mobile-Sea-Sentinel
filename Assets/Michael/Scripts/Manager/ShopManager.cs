using System;
using System.Collections.Generic;
using Michael.Scripts.Controller;
using UnityEngine;

namespace Michael.Scripts.Manager
{

    public class ShopManager : MonoBehaviour
    {

        public static Action OnDamageUpgrade;
        public static Action OnGoldCapacityUpgrade;
        [SerializeField] PlayerData _playerData;
        public Upgrade[] allUpgrades; 
        
        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

    

    }
}
    
 
