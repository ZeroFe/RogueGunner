using UnityEngine;

[CreateAssetMenu(fileName = "New Buff Effect", menuName = "UseEffect/Buff Effect")]
public class BuffEffect : PersistEffect
{
    public enum EBuffAbility
    {
        Attack,
        Defence,
        MoveSpeed,
        AttackSpeed,
    }

    public EBuffAbility Ability;
    public bool IsBuff = true;
    public float increaseBase = 0;
    public float increasePerStack = 0;

    public override string Explain()
    {
        return "BuffEffect!!";
    }

    public override void TakeUseEffect()
    {
        throw new System.NotImplementedException();
    }
}
