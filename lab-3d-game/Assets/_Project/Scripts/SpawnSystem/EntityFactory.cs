using UnityEngine;

namespace Platformer._Project.Scripts.SpawnSystem
{
    public class EntityFactory<T> : IEntityFactory<T> where T : Entity {
        private readonly CollectibleData[] _data;
        
        public EntityFactory(CollectibleData[] data) {
            _data = data;
        }
        
        public T Create(Transform spawnPoint) {
            var entityData = _data[Random.Range(0, _data.Length)];
            var instance = Object.Instantiate(entityData.prefab, spawnPoint.position, spawnPoint.rotation);
            return instance.GetComponent<T>();
        }
    }
}