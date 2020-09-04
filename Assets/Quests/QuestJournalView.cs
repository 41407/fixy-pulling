using System;
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

        private void Update()
        {
            text.text = questJournal.ToString();
        }
    }
}
