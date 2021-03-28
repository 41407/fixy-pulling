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
            text.text = ToKilometersPerHour(GetSpeed()).ToString("0.0 km/h", CultureInfo.InvariantCulture);
        }

        private float GetSpeed()
        {
            return fixyController.GetSpeed();
        }

        private static float ToKilometersPerHour(float metersPerSecond) => metersPerSecond * 3.6f;
    }
}
