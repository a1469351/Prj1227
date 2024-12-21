using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ns
{
	public class AudioManager : SingletonBase<AudioManager>
	{
        private AudioSource BGMAudioSource;
        private int oneShotAudioNum = 5;
        private int oneShotAudioIndex = 0;
        private List<AudioSource> oneShotAudioList = new List<AudioSource>();
        private float totalVolume = 1;
        private Dictionary<string, AudioSource> singleAudioList = new Dictionary<string, AudioSource>();
        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            BGMAudioSource = GameObject.Find("Bg").GetComponent<AudioSource>();
            PlayBGM();
            for (int i = 0; i < oneShotAudioNum; ++i)
            {
                GameObject go = new GameObject();
                AudioSource ads = go.AddComponent<AudioSource>();
                oneShotAudioList.Add(ads);
            }
        }

        private void PlayBGM(string name = "")
        {
            if (string.IsNullOrEmpty(name)) name = "Temporal_Displacement";
            AudioClip adc = ResourceManager.Instance.GetAudioClip(name);
            BGMAudioSource.clip = adc;
            BGMAudioSource.Play();
        }

        public void PlayOneShotAudio(string name)
        {
            AudioClip adc = ResourceManager.Instance.GetAudioClip(name);
            if (adc != null) PlayOneShotAudio(adc);
        }

        public void PlayOneShotAudio(AudioClip adc)
        {
            AudioSource ads = oneShotAudioList[oneShotAudioIndex];
            ads.Stop();
            ads.clip = adc;
            ads.Play();
            oneShotAudioIndex = (oneShotAudioIndex + 1) % oneShotAudioNum;
        }

        public void PlaySingleAudio(string name, string audioClipName, bool loop = true)
        {
            AudioClip adc = ResourceManager.Instance.GetAudioClip(audioClipName);
            if (adc == null) return;
            if (!singleAudioList.ContainsKey(name))
            {
                GameObject go = new GameObject();
                AudioSource ads = go.AddComponent<AudioSource>();
                singleAudioList.Add(name, ads);
            }
            singleAudioList[name].loop = loop;
            if (singleAudioList[name].clip == adc && singleAudioList[name].isPlaying) return;
            singleAudioList[name].Stop();
            singleAudioList[name].clip = adc;
            singleAudioList[name].Play();
        }

        public void StopSingleAudio(string name)
        {
            if (singleAudioList.ContainsKey(name))
            {
                singleAudioList[name].Stop();
            }
        }

        public void ReleaseSingleAudio(string name)
        {
            if (singleAudioList.ContainsKey(name))
            {
                singleAudioList[name].Stop();
                Destroy(singleAudioList[name].gameObject);
                singleAudioList.Remove(name);
            }
        }

        public void SetAudioSourceVolume(float f)
        {
            foreach (AudioSource ads in oneShotAudioList)
            {
                ads.volume = f * totalVolume;
            }
        }

        public void UpdateAudioSourceVolume()
        {
            foreach (AudioSource ads in oneShotAudioList)
            {
                ads.volume *= totalVolume;
            }
            foreach (var p in singleAudioList)
            {
                p.Value.volume *= totalVolume;
            }
        }

        public void ResetAudioSourceVolume()
        {
            foreach (AudioSource ads in oneShotAudioList)
            {
                ads.volume /= totalVolume;
            }
            foreach (var p in singleAudioList)
            {
                p.Value.volume /= totalVolume;
            }
        }

        public float GetTotalVolume()
        {
            return totalVolume;
        }

        public void SetTotalVolume(float f)
        {
            ResetAudioSourceVolume();
            totalVolume = f;
            UpdateAudioSourceVolume();
            BGMAudioSource.volume = f;
        }
    }
}