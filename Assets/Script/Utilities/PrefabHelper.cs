using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ns
{
	public class PrefabHelper : MonoBehaviour
	{
		static string txtPath = Application.dataPath + "/Resources/PrefabPath.txt";
		static string musicTxtPath = Application.dataPath + "/Resources/MusicPath.txt";
		static string prePath = Application.dataPath + "/Resources\\";
		static string preMusicPath = Application.dataPath + "/Resources/";
		[MenuItem("Tools/FindAllPrefabPath")]
		public static void FindAllPrefabPath()
        {
			string pathTxt = "";
			FindPath(ref pathTxt, Application.dataPath + "/Resources");
			if (!string.IsNullOrEmpty(pathTxt))
			{
				StreamWriter sw;
				FileInfo fi = new FileInfo(txtPath);
				sw = fi.CreateText();
				sw.Write(pathTxt);
				sw.Close();
				sw.Dispose();
				Debug.Log("FindAllPrefabPath done!");
			}
		}
		public static void FindPath(ref string pathTxt, string dirPath)
        {
			foreach (string path in Directory.GetFiles(dirPath))
            {
				if (Path.GetExtension(path) == ".prefab")
                {
					string line = path.Replace(prePath, "").Replace("\\", "/").Replace(".prefab", "") + "=" + Path.GetFileName(path).Replace(".prefab", "") + "\n";
					pathTxt += line;
                }
            }
			if (Directory.GetDirectories(dirPath).Length > 0)
            {
				foreach(string path in Directory.GetDirectories(dirPath))
                {
					FindPath(ref pathTxt, path);
                }
            }
		}

		[MenuItem("Tools/FindAllMusicPath")]
		public static void FindAllMusicPath()
		{
			string pathTxt = "";
			FindMusicPath(ref pathTxt, Application.dataPath + "/Resources/Music");
			if (!string.IsNullOrEmpty(pathTxt))
			{
				StreamWriter sw;
				FileInfo fi = new FileInfo(musicTxtPath);
				sw = fi.CreateText();
				sw.Write(pathTxt);
				sw.Close();
				sw.Dispose();
				Debug.Log("FindAllMusicPath done!");
			}
		}
		public static void FindMusicPath(ref string pathTxt, string dirPath)
		{
			foreach (string path in Directory.GetFiles(dirPath))
			{
				if (Path.GetExtension(path) != ".meta")
				{
					string line = path.Replace(preMusicPath, "").Replace("\\", "/").Replace(Path.GetExtension(path), "") + "=" + Path.GetFileName(path).Replace(Path.GetExtension(path), "") + "\n";
					pathTxt += line;
				}
			}
			if (Directory.GetDirectories(dirPath).Length > 0)
			{
				foreach (string path in Directory.GetDirectories(dirPath))
				{
					FindPath(ref pathTxt, path);
				}
			}
		}
	}
}