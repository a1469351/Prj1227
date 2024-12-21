using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ns
{
	public class ResourceManager : SingletonBase<ResourceManager>
	{
		Dictionary<string, string> pathList = new Dictionary<string, string>();
        Dictionary<string, GameObject> gameObjectList = new Dictionary<string, GameObject>();
        Dictionary<string, string> musicPathList = new Dictionary<string, string>();
        Dictionary<string, AudioClip> audioClipList = new Dictionary<string, AudioClip>();
        protected override void Awake()
        {
            base.Awake();
            ReadPrefabList();
            ReadMusicList();
        }

        void ReadPrefabList()
        {
            TextAsset textFile = Resources.Load<TextAsset>("PrefabPath") as TextAsset;
            if (textFile != null)
            {
                string[] str = textFile.text.Split('\n');
                foreach(string s in str)
                {
                    string[] ss = s.Split('=');
                    if (ss.Length == 2)
                    {
                        pathList.Add(ss[1], ss[0]);
                    }
                }
            }
            else
            {
                Debug.LogError("Read prefab list fail!");
            }
            foreach (var p in pathList)
            {
                GetPrefab(p.Key);
            }
        }

        void ReadMusicList()
        {
            TextAsset textFile = Resources.Load<TextAsset>("MusicPath") as TextAsset;
            if (textFile != null)
            {
                string[] str = textFile.text.Split('\n');
                foreach (string s in str)
                {
                    string[] ss = s.Split('=');
                    if (ss.Length == 2)
                    {
                        musicPathList.Add(ss[1], ss[0]);
                    }
                }
            }
            else
            {
                Debug.LogError("Read music list fail!");
            }
        }

        public GameObject GetPrefab(string name)
        {
            if (gameObjectList.ContainsKey(name))
            {
                return gameObjectList[name];
            }
            else if(pathList.ContainsKey(name))
            {
                GameObject go = (GameObject)Resources.Load(pathList[name]);
                gameObjectList[name] = go;
                if (go == null) Debug.LogError(name + " is null!");
                return go;
            }
            else
            {
                return null;
            }
        }

        public bool PrefabExist(string name)
        {
            return pathList.ContainsKey(name);
        }

        public AudioClip GetAudioClip(string name)
        {
            if (audioClipList.ContainsKey(name))
            {
                return audioClipList[name];
            }
            else if (musicPathList.ContainsKey(name))
            {
                AudioClip go = (AudioClip)Resources.Load(musicPathList[name]);
                audioClipList[name] = go;
                if (go == null) Debug.LogError(name + " is null!");
                return go;
            }
            else
            {
                return null;
            }
        }
    }
}