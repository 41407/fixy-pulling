using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Fixy
{
    public class DrivetrainNoise : MonoBehaviour
    {
        [Inject] private AudioSource AudioSource { get; }
        [Inject] private IDrivetrain Drivetrain { get; }

        [SerializeField] private List<AudioClip> sounds;

        private int currentSound = 0;

        private void Update()
        {
            AudioSource.volume = Mathf.Lerp(AudioSource.volume, Mathf.Clamp(Drivetrain.MashingCoefficient, 0.05f, 1f), 0.4f);
            if (Drivetrain.IsBraking)
            {
                AudioSource.volume += 0.5f;
            }
            var normalizedCrankAngle = Drivetrain.GetCrankAngle() / 180f;
            var newSound = Mathf.FloorToInt((sounds.Count - 1) * normalizedCrankAngle);
            if (newSound != currentSound)
            {
                currentSound = newSound;
                AudioSource.clip = sounds[currentSound % sounds.Count];
                AudioSource.pitch = Mathf.Lerp(AudioSource.pitch, Random.Range(0.9f, 1.1f), 0.5f);
                AudioSource.Play();
            }
        }
    }
}
