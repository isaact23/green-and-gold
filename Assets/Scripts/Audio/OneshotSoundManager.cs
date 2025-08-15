using System;
using UnityEngine;
using UnityEngine.Audio;

namespace GnG.Audio {
    // Factory class for OneshotSounds - instantiates the prefab and runs the OneshotSound script for each sound
    public class OneshotSoundManager : MonoBehaviour {
        [SerializeField] private GameObject oneshotSoundPrefab;
        [SerializeField] private AudioMixerGroup masterAudioMixerGroup;
        [SerializeField] private AudioMixerGroup blockAudioMixerGroup;

        public void PlayAtPoint(AudioResource audioResource, Vector3 point) {
            Debug.LogWarning("Played sound with unspecified sound type - using master channel");
            PlayAtPoint(audioResource, point, SoundType.Master);
        }
        public void PlayAtPoint(AudioResource audioResource, Vector3 point, SoundType soundType) {
            AudioMixerGroup mixer;
            switch (soundType) {
                case SoundType.Master: {
                    mixer = masterAudioMixerGroup; break;
                }
                case SoundType.Block: {
                    mixer = blockAudioMixerGroup; break;
                }
                default: {
                    mixer = masterAudioMixerGroup; break;
                }
            }
            playAtPoint(audioResource, mixer, point);
        }

        private void playAtPoint(AudioResource audioResource, AudioMixerGroup audioMixerGroup, Vector3 point) {

            if (audioResource == null || point == null)
                throw new ArgumentException("AudioResource and Point must not be null");

            GameObject oneshotSoundObj = GameObject.Instantiate(oneshotSoundPrefab, point, Quaternion.identity);
            oneshotSoundObj.GetComponent<OneshotSound>().Play(audioResource, audioMixerGroup);

        }
    }
}