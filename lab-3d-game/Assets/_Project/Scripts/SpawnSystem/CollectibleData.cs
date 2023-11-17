using UnityEngine;

namespace Platformer._Project.Scripts.SpawnSystem
{
    [CreateAssetMenu(fileName = "CollectibleData", menuName = "Platformer/Collectible Data")]
    public class CollectibleData : EntityData {
        public int score;
    }
}