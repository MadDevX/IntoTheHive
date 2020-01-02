using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingRoundsModule : BaseModule
{
    public override int Priority => 1;

    public override short Id => 3;

    public override bool IsInheritable => true;

    private Modifier _mod = new Modifier(ModifierType.PercentageCumulative, -1.0f);

    public override void DecorateProjectile(IProjectile projectile)
    {
        projectile.DamageModifiers.Add(_mod);
    }

    public override void RemoveFromProjectile(IProjectile projectile)
    {
        projectile.DamageModifiers.Remove(_mod);
    }

    public override IModule Clone()
    {
        return new HealingRoundsModule();
    }
}
