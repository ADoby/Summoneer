using SimpleLibrary;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class KISpawner : Spawner
{
	private Vector3 position;
	private MinionSpawnInfos infos;
	private float currentDifficulty;
	private EnemyDifficultySetting setting;
	private KI_Owner owner;

	protected override void Do()
	{
		position = Utils.PositionOnLevelBorder();

		infos = new MinionSpawnInfos();
		currentDifficulty = GameManager.Instance.CurrentDifficulty;
		for (int i = 0; i < GameManager.Instance.MinionDifficultySettings.Count; i++)
		{
			setting = GameManager.Instance.MinionDifficultySettings[i];
			if (setting.MinDifficulty > currentDifficulty)
				continue;
			if (setting.MaxDifficulty < currentDifficulty)
				continue;
			int count = Random.Range(setting.MinCount, setting.MaxCount + 1);

			if (count <= 0)
				continue;

			infos.Infos.Add(new MinionSpawnInfo(setting.MinionID, count));
		}
		if (infos.Infos.Count > 0)
		{
			owner = new GameObject("KI").AddComponent<KI_Owner>();
			owner.Type = Owner.Types.AGGRESSIVE;
			owner.transform.position = position;
			owner.StartInfo = infos;
		}
	}
}