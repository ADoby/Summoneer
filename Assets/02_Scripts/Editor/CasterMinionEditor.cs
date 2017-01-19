using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR

using UnityEditor;

#endif

[CustomEditor(typeof(CasterMinion))]
public class CasterMinionEditor : Editor
{
#if UNITY_EDITOR

    private Color bg = new Color(0, 0.1f, 1f, 0.1f);
    private Color border = new Color(0, 0.1f, 1f, 0.5f);

    private Color bgWantedDistance = new Color(0.5f, 0.5f, 0f, 0.1f);
    private Color borderDistance = new Color(0.5f, 0.5f, 0f, 0.5f);

    private void OnSceneGUI()
    {
        CasterMinion minion = (CasterMinion)target;

        if (minion.Body == null)
            return;

        List<Vector3> rectangle = new List<Vector3>();
        rectangle.Add(minion.Body.position + Vector3.up * minion.BodyCenterYDiff - Vector3.right * minion.AttackRangeX + Vector3.up * minion.AttackRangeY);
        rectangle.Add(rectangle[0] + Vector3.right * minion.AttackRangeX * 2f);
        rectangle.Add(rectangle[1] - Vector3.up * minion.AttackRangeY * 2f);
        rectangle.Add(rectangle[2] - Vector3.right * minion.AttackRangeX * 2f);
        Handles.DrawSolidRectangleWithOutline(rectangle.ToArray(), bg, border);

        rectangle.Clear();
        rectangle.Add(minion.Body.position + Vector3.up * minion.BodyCenterYDiff - Vector3.right * minion.WantedDistanceToOtherThingsX + Vector3.up * minion.WantedDistanceToOtherThingsY);
        rectangle.Add(rectangle[0] + Vector3.right * minion.WantedDistanceToOtherThingsX * 2f);
        rectangle.Add(rectangle[1] - Vector3.up * minion.WantedDistanceToOtherThingsY * 2f);
        rectangle.Add(rectangle[2] - Vector3.right * minion.WantedDistanceToOtherThingsX * 2f);
        Handles.DrawSolidRectangleWithOutline(rectangle.ToArray(), bgWantedDistance, borderDistance);

        Handles.color = Color.yellow;

        //Sight Range
        Handles.DrawWireDisc(minion.Body.position, Vector3.forward, minion.SightRange);

        float range = minion.SightRange;
        Sumoneer.EditorHelper.FourDirectionRange(minion.Body.position, ref range, ref range, ref range, ref range);
        minion.SightRange = range;

        range = minion.AttackRangeX;
        float rangey = minion.AttackRangeY;
        Sumoneer.EditorHelper.FourDirectionRange(minion.Body.position + Vector3.up * minion.BodyCenterYDiff, ref range, ref range, ref rangey, ref rangey);
        minion.AttackRangeX = range;
        minion.AttackRangeY = rangey;

        range = minion.WantedDistanceToOtherThingsX;
        rangey = minion.WantedDistanceToOtherThingsY;
        Sumoneer.EditorHelper.FourDirectionRange(minion.Body.position + Vector3.up * minion.BodyCenterYDiff, ref range, ref range, ref rangey, ref rangey);
        minion.WantedDistanceToOtherThingsX = range;
        minion.WantedDistanceToOtherThingsY = rangey;

        Handles.color = Color.white;
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }

    private void OnEnable()
    {
        Minion minion = (Minion)target;
        minion.ShowCurrentLevel();
    }

    public override void OnInspectorGUI()
    {
        Minion minion = (Minion)target;

        GUILayout.Label(string.Format("Level Setting: CurrentLevel: {0}/{1}", minion.Level + 1, minion.LevelInfos.Count));
        GUILayout.BeginHorizontal();

        GUI.enabled = minion.Level > 0;
        if (GUILayout.Button("Previous"))
        {
            minion.Level--;
            minion.ShowCurrentLevel();
        }

        GUI.enabled = minion.Level < minion.LevelInfos.Count - 1;
        if (GUILayout.Button("Next"))
        {
            minion.Level++;
            minion.ShowCurrentLevel();
        }
        GUILayout.EndHorizontal();
        GUI.enabled = true;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        // After we drawn our stuff, draw the default inspector
        DrawDefaultInspector();
    }

#endif
}