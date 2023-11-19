using Platformer._Project.Scripts.Utils.Timer;
using UnityEngine;

namespace Platformer._Project.Scripts.Utils.Enemy
{
    public interface IDetectionStrategy {
        bool Execute(Transform player, Transform detector, CountdownTimer timer);
    }
}