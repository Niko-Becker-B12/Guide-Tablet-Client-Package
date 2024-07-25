using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GuideTabletClient
{

    [RequireComponent(typeof(GuideTabletClient), typeof(AudioSource))]
    public class GuideTabletClientSoundManager : MonoBehaviour
    {

        GuideTabletClient client => GetComponent<GuideTabletClient>();

        AudioSource audioSource => GetComponent<AudioSource>();

        public List<EventSoundData> sounds = new List<EventSoundData>();


        private void OnEnable()
        {

            client.OnEventCallReceived += PlayEventSound;

            audioSource.volume = .5f;
            audioSource.playOnAwake = false;
            audioSource.clip = null;
            audioSource.loop = false;

        }

        private void OnDisable()
        {

            client.OnEventCallReceived -= PlayEventSound;

        }

        public void PlayEventSound(string trigger)
        {

            AudioClip clip = sounds.Find(x => x.trigger == trigger).audioClip;

            if (clip == null)
                return;

            audioSource.clip = clip;
            audioSource.Play();

        }

    }

    [System.Serializable]
    public class EventSoundData
    {

        public string trigger;
        public AudioClip audioClip;

    }

}