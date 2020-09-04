using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Quests.Location
{
    public class QuestLocations : MonoBehaviour
    {
        [Inject] public List<ILocation> Locations;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                var availableLocations = Locations.FindAll(l => !l.HasActiveQuest);
                if (availableLocations.Count > 0)
                {
                    availableLocations.Sort((a, b) => Random.Range(-1000, 1000));
                    var location = availableLocations.First();
                    location.SetQuest(CreateQuestAt(location));
                }
            }
        }

        private IQuest CreateQuestAt(ILocation location)
        {
            var quest = new Quest(
                location,
                GetDestinationFor(location),
                new DebugLogReward()
            );
            Debug.Log($"New quest! {quest}");
            return quest;
        }

        private ILocation GetDestinationFor(ILocation location)
        {
            var availableLocations = Locations.FindAll(l => l != location);
            availableLocations.Sort((a, b) => Random.Range(-1000, 1000));
            return availableLocations.First();
        }
    }
}
