using Platformer._Project.Scripts.Utils.Timer;
using UnityEngine;

namespace Platformer._Project.Scripts.SpawnSystem
{
    public class CollectibleSpawnManager : EntitySpawnManager {
        [SerializeField] private CollectibleData[] collectibleData;
        [SerializeField] private float spawnInterval = 1f;
        
        private EntitySpawner<Collectible> _spawner;
        
        private CountdownTimer _spawnTimer;
        private int _counter;

        protected override void Awake() {
            base.Awake();

            _spawner = new EntitySpawner<Collectible>(
                new EntityFactory<Collectible>(collectibleData), SpawnPointStrategy);
            
            _spawnTimer = new CountdownTimer(spawnInterval);
            _spawnTimer.OnTimerStop += () => {
                if (_counter++ >= spawnPoints.Length) {
                    _spawnTimer.Stop();
                    return;
                }
                Spawn();
                _spawnTimer.Start();
            };
        }
        
        void Start() => _spawnTimer.Start();
        
        void Update() => _spawnTimer.Tick(Time.deltaTime);

        public override void Spawn() => _spawner.Spawn();
    }
}