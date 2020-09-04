using Quests.Location;

namespace Quests
{
    public interface IQuest
    {
        void Complete();
        ILocation Destination { get; }
        bool Completed { get; }
        ILocation Source { get; }
    }

    public class Quest : IQuest
    {
        public ILocation Source { get; }
        public ILocation Destination { get; }
        private IReward Reward { get; }

        public bool Completed { get; private set; }

        public Quest(ILocation source, ILocation destination, IReward reward)
        {
            Source = source;
            Destination = destination;
            Reward = reward;
        }

        public void Complete()
        {
            if (!Completed)
            {
                Reward.Grant();
                Completed = true;
            }
        }

        public override string ToString() => $"{(Completed ? "COMPLETED: " : "")} Delivery from {Source} to {Destination} for {Reward}!";
    }
}
