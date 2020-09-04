using UnityEngine;

namespace Quests.Location
{
    public interface IAvailableQuestView
    {
        void Show(string text);
        void Hide();
    }

    public class AvailableQuestView : MonoBehaviour, IAvailableQuestView
    {
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
