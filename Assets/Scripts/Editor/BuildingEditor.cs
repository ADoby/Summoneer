using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Building))]
public class BuildingEditor : Editor
{
	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Generate", GUILayout.Height(30)))
		{
			Building build = (Building)target;
			build.GenerateBuilding();
			EditorUtility.SetDirty(target);
		}
		base.DrawDefaultInspector();
	}
}
#endif