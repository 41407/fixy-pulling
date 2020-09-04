using UnityEngine;
using Zenject;

namespace Quests.Location
{
    public interface ILocation
    {
        IQuest AcceptQuest();
        bool HasActiveQuest { get; }
        string Name { get; }
        void SetQuest(IQuest quest);
    }

    public class Location : MonoBehaviour, ILocation
    {
        [SerializeField] private string locationName;
        [Inject] private IAvailableQuestView questView;

        private IQuest Quest { get; set; }
        public bool HasActiveQuest => Quest != null;
        public string Name => locationName;

        public void SetQuest(IQuest quest)
        {
            Quest = quest;
            questView.Show(Quest.ToString());
        }

        public IQuest AcceptQuest()
        {
            var quest = Quest;
            Quest = null;
            questView.Hide();
            return quest;
        }

        public override string ToString() => Name;
    }
}
