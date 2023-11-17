﻿using UnityEngine;

namespace Platformer._Project.Scripts.SpawnSystem
{
    public class LinearSpawnPointStrategy : ISpawnPointStrategy {
        private int _index;
        private readonly Transform[] _spawnPoints;
        
        public LinearSpawnPointStrategy(Transform[] spawnPoints) {
            _spawnPoints = spawnPoints;
        }
        
        public Transform NextSpawnPoint() {
            var result = _spawnPoints[_index];
            _index = (_index + 1) % _spawnPoints.Length;
            return result;
        }
    }
}