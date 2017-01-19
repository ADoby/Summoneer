using SimpleLibrary;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class ContainerSpawner : Spawner
{
	private ContainerDifficultySetting setting;
	private Vector3 position;
	private bool noSpace = true;
	private int tries = 0;
	private float currentDifficulty;

	protected override void Do()
	{
		position = Utils.PositionInLevel();

		noSpace = true;
		tries = 0;
		while (noSpace)
		{
			noSpace = Physics2D.OverlapCircle(position, 2f);
			tries++;
			if (tries > 20)
			{
				Debug.Log("Didnt find space");
				return;
			}
		}

		currentDifficulty = GameManager.Instance.CurrentDifficulty;
		for (int i = 0; i < GameManager.Instance.ContainerDifficultySettings.Count; i++)
		{
			setting = GameManager.Instance.ContainerDifficultySettings[i];
			if (setting.MinDifficulty > currentDifficulty)
				continue;
			if (setting.MaxDifficulty < currentDifficulty)
				continue;

			if (Random.value < setting.Chance)
			{
				ContainerManager.SpawnContainer(position, setting.ContainerID);
				return;
			}
		}
	}
}