using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ns
{
	public static class CommonFunc
	{
		public static float DistancePoint2Line(Vector3 p, Vector3 linePoint1, Vector3 linePoint2)
        {
			Vector3 v1 = p - linePoint1;
			Vector3 v2 = linePoint2 - linePoint1;
			Vector3 proj = Vector3.Project(v1, v2);
			float dis = Mathf.Sqrt(Mathf.Pow(Vector3.Magnitude(v1), 2) - Mathf.Pow(Vector3.Magnitude(proj), 2));
			return dis;
        }
	}
}