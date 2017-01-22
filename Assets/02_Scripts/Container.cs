using System.Collections;
using UnityEngine;

public class Container : Attackable
{
    [Range(0, 10)]
    public int Souls = 1;

    public MinionSpawnInfos MinionInfo;

    protected override IEnumerator Die(Owner attacker)
    {
        if (MinionInfo.Infos.Count > 0)
        {
            Minion[] minions = MinionInfo.Spawn(transform.position);
            KI_Owner owner = new GameObject().AddComponent<KI_Owner>();
            owner.CurrentTargetPosition = BodyCenter;
            for (int i = 0; i < minions.Length; i++)
            {
                owner.AddMinion(minions[i], true);
            }
            owner.SetCurrentAttacker(attacker);
        }

        for (int i = 0; i < Souls; i++)
        {
            ReleaseSouls(attacker);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
        Despawn();
    }
}