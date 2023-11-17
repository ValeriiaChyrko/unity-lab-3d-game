using UnityEngine;

namespace Platformer._Project.Scripts.SpawnSystem
{
    public abstract class EntitySpawnManager : MonoBehaviour {
        [SerializeField] protected SpawnPointStrategyType spawnPointStrategyType = SpawnPointStrategyType.Linear;
        [SerializeField] protected Transform[] spawnPoints;
        
        protected ISpawnPointStrategy SpawnPointStrategy;

        protected enum SpawnPointStrategyType {
            Linear,
            Random
        }

        protected virtual void Awake() {
            SpawnPointStrategy = spawnPointStrategyType switch {
                SpawnPointStrategyType.Linear => new LinearSpawnPointStrategy(spawnPoints),
                SpawnPointStrategyType.Random => new RandomSpawnPointStrategy(spawnPoints),
                _ => SpawnPointStrategy
            };
        }

        public abstract void Spawn();
    }
}