using System;
using Quests.Location;
using UnityEngine;
using Zenject;

namespace Quests
{
    public class QuestPlayer : MonoBehaviour
    {
        [Inject] private IQuestJournal questJournal;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                questJournal.AcceptCurrentQuest();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var location = other.GetComponentInParent<ILocation>();
            if (location != null)
            {
                questJournal.Enter(location);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var location = other.GetComponentInParent<ILocation>();
            if (location != null)
            {
                questJournal.Leave();
            }
        }
    }
}
