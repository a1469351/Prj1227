using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ns
{
	public class EventManager : SingletonBase<EventManager>
	{
		private Dictionary<EventEnum, Action> dic = new Dictionary<EventEnum, Action>();
		public enum EventEnum
        {
			Test1,
			Test2,
			ScoreChange
		}

		public void RegisterEvent(EventEnum e, Action ac)
        {
			if (dic.ContainsKey(e))
            {
				dic[e] += ac;
            }
			else
            {
				dic.Add(e, ac);
            }
        }

		public void UnRegisterEvent(EventEnum e, Action ac)
        {
			if (dic.ContainsKey(e))
            {
				dic[e] -= ac;
            }
        }

		public void RemoveEvent(EventEnum e, Action ac)
        {
			if (dic.ContainsKey(e))
			{
				dic.Remove(e);
			}
		}

		public void SendEvent(EventEnum e)
        {
			if (dic.ContainsKey(e))
            {
				dic[e]();
            }
        }
	}
}