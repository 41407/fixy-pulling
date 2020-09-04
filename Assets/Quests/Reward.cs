using UnityEngine;

namespace Quests
{
    public interface IReward
    {
        void Grant();
    }

    public class DebugLogReward : IReward
    {
        public void Grant() => Debug.Log("Hooray! Reward granted!");

        public override string ToString() => "the joy of a job well done";
    }
}
