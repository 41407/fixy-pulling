using Fixy;
using UnityEngine;
using Zenject;

namespace Quests.Location
{
    public interface IQuestView
    {
        void Show(string text);
        void Hide();
    }

    public class QuestView : MonoBehaviour, IQuestView
    {
        [Inject] private IPlayerPosition player { get; }

        private void Update()
        {
            transform.LookAt(player.Position);
        }

        public void Show(string text)
        {
            GetComponentInChildren<TextMesh>().text = text + "\n\nPress 'E' to accept :)";
        }

        public void Hide()
        {
            GetComponentInChildren<TextMesh>().text = "No quests today :) Nice to see you though :)";
        }
    }
}
