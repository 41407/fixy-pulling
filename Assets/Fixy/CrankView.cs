using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Fixy
{
    public class CrankView : MonoBehaviour
    {
        [Inject] private IDrivetrain Drivetrain { get; }
        private Image Image => GetComponent<Image>();

        private RectTransform rect { get; set; }
        private RectTransform Rect => rect ? rect : rect = GetComponent<RectTransform>();

        private void Update()
        {
            Image.color = new Color(1f, 1f, 1f, Mathf.Clamp(Drivetrain.GetCrankAnglePedalingStrengthModifier(), 0.2f, 1f));
            Rect.rotation = Quaternion.AngleAxis(Drivetrain.GetCrankAngle(), Vector3.back);
            transform.localScale = Vector3.one * (Drivetrain.MashingCoefficient + 1f);
        }
    }
}
