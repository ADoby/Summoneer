using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

public class EditorHelper
{
	public static void FourDirectionRange(Vector3 position, ref float minX, ref float maxX, ref float minY, ref float maxY)
	{
		Vector3 pos = position + Vector3.right * maxX;
		Vector3 newPos = Handles.Slider(pos, Vector3.right, HandleUtility.GetHandleSize(pos) * 0.05f, Handles.DotCap, 0.1f);
		maxX = Mathf.Max(maxX + (newPos - pos).x, 0f);

		pos = position + Vector3.left * minX;
		newPos = Handles.Slider(pos, Vector3.left, HandleUtility.GetHandleSize(pos) * 0.05f, Handles.DotCap, 0.1f);
		minX = Mathf.Max(minX - (newPos - pos).x, 0f);

		pos = position + Vector3.up * maxY;
		newPos = Handles.Slider(pos, Vector3.up, HandleUtility.GetHandleSize(pos) * 0.05f, Handles.DotCap, 0.1f);
		maxY = Mathf.Max(maxY + (newPos - pos).y, 0f);

		pos = position + Vector3.down * minY;
		newPos = Handles.Slider(pos, Vector3.down, HandleUtility.GetHandleSize(pos) * 0.05f, Handles.DotCap, 0.1f);
		minY = Mathf.Max(minY - (newPos - pos).y, 0f);
	}
}
#endif