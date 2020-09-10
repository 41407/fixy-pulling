using TMPro;
using UnityEngine;
using Zenject;

namespace Quests
{
    public class QuestJournalView : MonoBehaviour
    {
        [Inject] private IQuestJournal questJournal;

        private TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            InvokeRepeating("Refresh", 2, 2);
        }

        private void Refresh()
        {
            text.text = questJournal.ToString();
        }
    }
}
