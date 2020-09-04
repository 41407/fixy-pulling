using System.Collections.Generic;
using Quests.Location;
using UnityEngine;

namespace Quests
{
    public interface IQuestJournal
    {
        void Enter(ILocation location);
        void Leave();
        void AcceptCurrentQuest();
    }

    public class QuestJournal : IQuestJournal
    {
        private List<IQuest> Quests { get; } = new List<IQuest>();
        private ILocation CurrentLocation { get; set; }

        public void Enter(ILocation location)
        {
            Debug.Log($"Entering {location}");
            CurrentLocation = location;
            CompleteQuestsWithDestination(location);
        }

        public void Leave()
        {
            CurrentLocation = null;
        }

        public void AcceptCurrentQuest()
        {
            Add(CurrentLocation?.AcceptQuest());
        }

        private void Add(IQuest quest)
        {
            if (quest != null)
            {
                Debug.Log($"Accepted quest {quest}");
                Quests.Add(quest);
            }
        }

        private void CompleteQuestsWithDestination(ILocation location)
        {
            Quests.FindAll(q => !q.Completed && q.Destination == location).ForEach(q => q.Complete());
        }

        public override string ToString()
        {
            var output = "";
            Quests.ForEach(q => output += q.ToString() + '\n');
            return output;
        }
    }
}
