using UnityEngine;
using UnityEngine.Audio;

namespace GnG.Audio {
    // Controller for the OneshotSound prefab (plays one sound and deletes itself)
    public class OneshotSound : MonoBehaviour {
        private AudioSource audioSource;
        private bool didStart = false;

        public void Play(AudioResource audioResource, AudioMixerGroup mixerGroup) {
            audioSource = GetComponent<AudioSource>();
            audioSource.resource = audioResource;
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.Play();
            didStart = true;
        }

        void Update() {
            if (didStart && !audioSource.isPlaying) {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}