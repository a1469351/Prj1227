using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ns
{
	public class UIPanel : MonoBehaviour
    {
        public GameObject go;
		virtual public void OnShow()
        {

        }

		virtual public void OnHide()
        {

        }
	}
	public class UIManager : SingletonBase<UIManager>
	{
        Transform uiroot;
		Dictionary<string, UIPanel> UIDict = new Dictionary<string, UIPanel>();

        override protected void Awake()
        {
            base.Awake();
            uiroot = GameObject.Find("Canvas/UIRoot").GetComponent<Transform>();
        }

        public void OpenUI(string name)
        {
            if (UIDict.ContainsKey(name))
            {
                UIDict[name].OnShow();
                UIDict[name].go.transform.SetAsLastSibling();
            }
            else
            {
                GameObject go = ResourceManager.Instance.GetPrefab(name);
                if (go != null)
                {
                    UIPanel uip = go.GetComponent<UIPanel>();
                    if (uip != null)
                    {
                        GameObject instance = Instantiate(go, uiroot);
                        instance.name = instance.name.Replace("(Clone)", "");
                        uip.go = instance;
                        UIDict.Add(name, uip);
                        uip.OnShow();
                        instance.transform.SetAsLastSibling();
                    }
                }
            }
        }

        public void CloseUI(string name)
        {
            if (UIDict.ContainsKey(name))
            {
                UIDict[name].OnHide();
                Destroy(UIDict[name].go);
                UIDict.Remove(name);
            }
        }

        public void CloseAllUI()
        {
            foreach(var ui in UIDict)
            {
                ui.Value.OnHide();
                Destroy(ui.Value.go);
            }
            UIDict.Clear();
        }
    }
}