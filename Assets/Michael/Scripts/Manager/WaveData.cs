using System;
using System.Collections.Generic;
using UnityEngine;

namespace Michael.Scripts.Manager
{
    [CreateAssetMenu(fileName = "WaveData",menuName = "ScriptableObjects/WaveData")]
    public class WaveData : ScriptableObject
    {
        [field : SerializeField]
        public int LittleBoatCount { get; private set; }
        [field : SerializeField]
        public int BalancedBoatCount { get; private set; }
        [field : SerializeField]
        public int BigBoatCount { get; private set; }
        [field : SerializeField]
        public int BoatWithGoldPourcent { get; private set; }
    }
}