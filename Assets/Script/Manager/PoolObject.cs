using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ns
{
	public class PoolObject : MonoBehaviour
	{
		public bool alive = true;
		public virtual void StartActive()
		{
			alive = true;
		}
		public virtual void StopActive()
        {
			alive = false;
		}
	}
}