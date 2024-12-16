using UnityEngine;

namespace Intégration.Scripts
{
    [CreateAssetMenu(fileName = "GameData",menuName = "ScriptableObjects/GameData")]
    public class GameData : ScriptableObject
    {
        public float MusicVolume;
        public float SfxVolume;
        public int Highscore;
        public bool _sfxSliderEnable;
        public bool _musicSliderEnable;
    }
}
