using System.Globalization;
using Fixy;
using TMPro;
using UnityEngine;
using Zenject;

namespace HUD
{
    public class Speedometer : MonoBehaviour
    {
        [Inject] private IFixyController fixyController;

        private TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            text.text = fixyController.GetSpeed().ToString("0.0 m/s", CultureInfo.InvariantCulture);
        }
    }
}
