using System;
using DG.Tweening;
using UnityEngine;

namespace Platformer._Project.Scripts.Utils.Platform
{
    public class PlatformMover : MonoBehaviour {
        [SerializeField] private Vector3 moveTo = Vector3.zero;
        [SerializeField] private float moveTime = 1f;
        [SerializeField] private Ease ease = Ease.InOutQuad;
        [SerializeField] private GameObject player;
        
        private Vector3 _startPosition;

        private void Start() {
            _startPosition = transform.position;
            Move();
        }

        private void Move() {
            transform.DOMove(_startPosition + moveTo, moveTime)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject == player)
            {
                player.transform.parent = transform;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject == player)
            {
                player.transform.parent = null;
            }
        }
    }
}