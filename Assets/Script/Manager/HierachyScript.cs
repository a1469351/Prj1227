using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ns
{
	[InitializeOnLoad]
	public class HierachyScript
	{
		static int toggleWidth = 20;
		static HierachyScript()
        {
			EditorApplication.hierarchyWindowItemOnGUI += HierachyCB;
        }

		private static void HierachyCB(int instanceid, Rect selectionRect)
        {
			Rect r = new Rect(selectionRect);
			r.x += selectionRect.width - toggleWidth;
			r.width = toggleWidth;
			GameObject go = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
			if (go != null)go.SetActive(GUI.Toggle(r, go.activeSelf, string.Empty));
        }
	}
}