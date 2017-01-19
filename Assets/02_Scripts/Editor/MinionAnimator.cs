using System.Collections;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class MinionAnimator
{
	[MenuItem("Assets/Create/Minion Animator")]
	public static void CreateMinionAnimator()
	{
		string path = "Assets";
		foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
		{
			path = AssetDatabase.GetAssetPath(obj);
			if (File.Exists(path))
			{
				path = Path.GetDirectoryName(path);
			}
			break;
		}
		//AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(path + "/New MyAsset.asset"));
	}

	private static AnimatorController GenerateSelectableAnimatorContoller(AnimationTriggers animationTriggers, Selectable target)
	{
		if (target == null)
			return null;

		// Where should we create the controller?
		var path = GetSaveControllerPath(target);
		if (string.IsNullOrEmpty(path))
			return null;

		// figure out clip names
		var normalName = string.IsNullOrEmpty(animationTriggers.normalTrigger) ? "Normal" : animationTriggers.normalTrigger;
		var highlightedName = string.IsNullOrEmpty(animationTriggers.highlightedTrigger) ? "Highlighted" : animationTriggers.highlightedTrigger;
		var pressedName = string.IsNullOrEmpty(animationTriggers.pressedTrigger) ? "Pressed" : animationTriggers.pressedTrigger;
		var disabledName = string.IsNullOrEmpty(animationTriggers.disabledTrigger) ? "Disabled" : animationTriggers.disabledTrigger;

		// Create controller and hook up transitions.
		var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
		GenerateTriggerableTransition(normalName, controller);
		GenerateTriggerableTransition(highlightedName, controller);
		GenerateTriggerableTransition(pressedName, controller);
		GenerateTriggerableTransition(disabledName, controller);

		AssetDatabase.ImportAsset(path);

		return controller;
	}

	private static AnimationClip GenerateTriggerableTransition(string name, AnimatorController controller)
	{
		// Create the clip
		var clip = AnimatorController.AllocateAnimatorClip(name);
		AssetDatabase.AddObjectToAsset(clip, controller);

		// Create a state in the animatior controller for this clip
		var state = controller.AddMotion(clip);

		// Add a transition property
		controller.AddParameter(name, AnimatorControllerParameterType.Trigger);

		// Add an any state transition
		var stateMachine = controller.layers[0].stateMachine;

		var transition = stateMachine.AddAnyStateTransition(state);
		transition.AddCondition(AnimatorConditionMode.If, 0, name);
		return clip;
	}

	private static string GetSaveControllerPath(Selectable target)
	{
		var defaultName = target.gameObject.name;
		var message = string.Format("Create a new animator for the game object '{0}':", defaultName);
		return EditorUtility.SaveFilePanelInProject("New Animation Contoller", defaultName, "controller", message);
	}
}