using SimpleLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public GameObject SoulPrefab;
    public float SoulSpeed = 5f;
    public List<ContainerSpawner> ContainerSpawner = new List<ContainerSpawner>();
    public List<KISpawner> KISpawner = new List<KISpawner>();

    public List<Player> RegisteredPlayers = new List<Player>();
    public List<Owner> RegisteredOwners = new List<Owner>();

    public ExperienceInfo ExperienceInfo;

    private float CalculateExp(float value, float mult, float strength, float otherStrength)
    {
        value = (value * mult);
        value = value + value * (otherStrength - strength) * ExperienceInfo.EnemyStrengthExperienceMultiplier;
        return Mathf.Max(value, 0f);
    }

    public float CalculateDamageDoneExp(float damage, float myStrength, float enemyStrength)
    {
        return CalculateExp(damage, ExperienceInfo.DamageDoneToExperience, myStrength, enemyStrength);
    }

    public float CalculateDamageSurvivedExp(float damage, float myStrength, float enemyStrength)
    {
        return CalculateExp(damage, ExperienceInfo.DamageSurvivedToExperience, myStrength, enemyStrength);
    }

    public float CalculateEnemyKilledExp(float myStrength, float enemyStrength)
    {
        return CalculateExp(ExperienceInfo.EnemeyKilledBaseExperience, ExperienceInfo.EnemyKilledExperience, myStrength, enemyStrength);
    }

    public bool CanAttack(Attacker attacker, Attackable other)
    {
        diff = other.BodyCenter - attacker.BodyCenter;
        diff.x -= other.SizeX * 0.5f - attacker.SizeX * 0.5f;
        diff.y -= other.SizeY * 0.5f - attacker.SizeY * 0.5f;
        return Mathf.Abs(diff.x) < attacker.AttackRangeX && Mathf.Abs(diff.y) < attacker.AttackRangeY;
    }

    private Vector3 bottomLeft = Vector3.zero;
    private Vector3 topLeft = Vector3.zero;
    private Vector3 topRight = Vector3.zero;
    private Vector3 bottomRight = Vector3.zero;

    private float delta = 0f;
    private int i;
    private Vector3 diff;
    public float CurrentDifficulty = 0f;
    public float DifficultyPerSecond = 0.1f;

    public Rect level = new Rect(-25, -25, 50, 50);

    public List<EnemyDifficultySetting> MinionDifficultySettings = new List<EnemyDifficultySetting>();
    public List<ContainerDifficultySetting> ContainerDifficultySettings = new List<ContainerDifficultySetting>();

    public Text Difficulty;
    public Text FPS;

    [Range(0.1f, 4f)]
    public float GameSpeed = 1f;

    public void RegisterPlayer(Player player)
    {
        RegisterOwner(player);
        if (RegisteredPlayers.Contains(player))
            return;
        RegisteredPlayers.Add(player);
    }

    public void RegisterOwner(Owner owner)
    {
        if (!RegisteredOwners.Contains(owner))
            RegisteredOwners.Add(owner);
    }

    public void UnregisterOwner(Owner owner)
    {
        if (RegisteredOwners.Contains(owner))
            RegisteredOwners.Remove(owner);
    }

    public Vector3[] GetAllPlayerPositions()
    {
        Vector3[] positions = new Vector3[RegisteredPlayers.Count];
        for (int i = 0; i < RegisteredPlayers.Count; i++)
        {
            positions[i] = RegisteredPlayers[i].transform.position;
        }
        return positions;
    }

    public Rect GetCurrentLevelRect()
    {
        return level;
    }

    public bool InRecruitRange(Owner owner, Minion minion)
    {
        diff = owner.MinionCenter - minion.transform.position;

        return diff.magnitude < 10f;
    }

    private void StartGame()
    {
        for (int i = 0; i < ContainerSpawner.Count; i++)
        {
            ContainerSpawner[i].Start();
        }
        for (int i = 0; i < KISpawner.Count; i++)
        {
            KISpawner[i].Start();
        }
    }

    private void UpdateGame()
    {
        if (Input.GetButtonDown("Escape"))
            Application.Quit();
        Time.timeScale = GameSpeed;

        delta = (delta + Time.deltaTime) / 2f;

        FPS.text = string.Format("FPS ({0:0.000}):{1:0000}", delta, 1f / delta);

        CurrentDifficulty += DifficultyPerSecond * Time.deltaTime;
        Difficulty.text = string.Format("Difficulty:{0:0.0}", CurrentDifficulty);
        for (i = 0; i < ContainerSpawner.Count; i++)
        {
            ContainerSpawner[i].Update(CurrentDifficulty);
        }
        for (i = 0; i < KISpawner.Count; i++)
        {
            KISpawner[i].Update(CurrentDifficulty);
        }

        bottomLeft.x = level.xMin;
        bottomLeft.y = level.yMin;
        topLeft.x = level.xMin;
        topLeft.y = level.yMax;
        topRight.x = level.xMax;
        topRight.y = level.yMax;
        bottomRight.x = level.xMax;
        bottomRight.y = level.yMin;
        //Debug.DrawLine(bottomLeft, topLeft);
        //Debug.DrawLine(topLeft, topRight);
        //Debug.DrawLine(topRight, bottomRight);
        //Debug.DrawLine(bottomRight, bottomLeft);
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        UpdateGame();
    }
}