namespace Platformer._Project.Scripts.Utils.Timer
{
    public class StopwatchTimer : Timer
    {
        public StopwatchTimer() : base(0)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (IsRunning) Time += deltaTime;
        }

        public void Reset()
        {
            Time = 0;
        }

        public float GetTime()
        {
            return Time;
        }
    }
}