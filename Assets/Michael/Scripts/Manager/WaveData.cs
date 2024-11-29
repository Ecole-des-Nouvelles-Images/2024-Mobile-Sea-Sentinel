using System;
using System.Collections.Generic;
using UnityEngine;

namespace Michael.Scripts.Manager
{
    [CreateAssetMenu(fileName = "WaveData",menuName = "ScriptableObjects/WaveData")]
    public class WaveData : ScriptableObject
    {
        [field : SerializeField]
        public List<BoatType> Boats { get; private set; }
    }
}