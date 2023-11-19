using Platformer._Project.EventSystem;
using UnityEngine;

namespace Platformer._Project.Scripts.SpawnSystem
{
    public class Collectible : Entity
    {
        [SerializeField] private int score = 10;
        [SerializeField] private IntEventChannel scoreChannel;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            scoreChannel.Invoke(score);
            Destroy(gameObject);
        }
    }
}