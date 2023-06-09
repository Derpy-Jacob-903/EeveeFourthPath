using MelonLoader;
using BTD_Mod_Helper;
using PathsPlusPlus;
using BTD_Mod_Helper.Api.Enums;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppSystem.IO;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using BTD_Mod_Helper.Api.ModOptions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;

[assembly: MelonInfo(typeof(EeveeFourthPath.EeveeFourthPath), EeveeFourthPath.ModHelperData.Name, EeveeFourthPath.ModHelperData.Version, EeveeFourthPath.ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace EeveeFourthPath;

public class EeveeFourthPath : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<EeveeFourthPath>("EeveeFourthPath loaded!");
    }
    public static readonly ModSettingCategory VeePatches = new("Patches")
    {
        collapsed = true
    };
    public static readonly ModSettingBool baseEeveeNerf = false;
    public static readonly ModSettingBool flareonNerf = false;
}
public class SylveonPath : PathPlusPlus
{
    public override string Tower => "Eevee-Eevee";

    public override int UpgradeCount => 5; // Increase this up to 5 as you create your Upgrades
}
public class LongRangePins : UpgradePlusPlus<SylveonPath>
{
    public override int Cost => 250;
    public override int Tier => 1;
    public override string Icon => VanillaSprites.LongRangeDartsUpgradeIcon;

    public override string Description => "Eevee throws pins further than normal.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.range += 10;
        foreach (var attackModel in towerModel.GetDescendants<AttackModel>().ToArray())
        {
            attackModel.range += 10;
        }
        foreach (var travelStraitModel in towerModel.GetDescendants<TravelStraitModel>().ToArray())
        {
            travelStraitModel.Lifespan *= 1.25f;
            travelStraitModel.lifespan = travelStraitModel.Lifespan;
            //travelStraitModel.lifespanFrames = (int)(travelStraitModel.Lifespan * 60);
        }

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}
public class HardPins : UpgradePlusPlus<SylveonPath>
{
    public override int Cost => 500;
    public override int Tier => 2;
    public override string Icon => VanillaSprites.HeatTippedDartUpgradeIcon;

    public override string Description => "Attacks can pop Lead and Frozen bloons. Jolteon's attacks can also pop Purple Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            damageModel.immuneBloonProperties &= ~BloonProperties.Frozen;
            damageModel.immuneBloonProperties &= ~BloonProperties.Lead;
            if(towerModel.GetUpgradeLevel(0) >= 3) //Is Jolteon?
            {
                damageModel.immuneBloonProperties &= ~BloonProperties.Purple;
            }
        }

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}
public class Sylveon : UpgradePlusPlus<SylveonPath>
{
    public override int Cost => 3350; //3000+350+
    public override int Tier => 3;
    public override string Icon => VanillaSprites.ArmorPiercingDartsUpgradeIcon;

    public override string Description => "Evolving Eevee to Sylveon and increases pierce and damage, plus extra damage to MOAB-class Bloons.";
    public override string Portrait => "700Sylveon_PSMD";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            damageModel.damage += 1;
        }
        foreach (var projectileModel in towerModel.GetDescendants<ProjectileModel>().ToArray())
        {
            projectileModel.pierce += 3;
            projectileModel.AddBehavior<DamageModifierForTagModel>(new DamageModifierForTagModel("DamageModifierForTagModel_Projectile", "Moabs", 1.0f, 3.0f, false, false));
        }


        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}

public class PlayRough : UpgradePlusPlus<SylveonPath>
{
    public override int Cost => 4900; //3000+350+
    public override int Tier => 4;
    public override string Icon => VanillaSprites.MonkeyPiratesUpgradeIconAA;

    public override string Description => "More damage, pierce, and extra MOAB damage. MOAB Takedown Ability: harpoons a MOAB class Bloon and brings it down.";
    public override string Portrait => "700Sylveon_PSMD";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            damageModel.damage += 1;
        }
        foreach (var projectileModel in towerModel.GetDescendants<ProjectileModel>().ToArray())
        {
            projectileModel.pierce += 3;
            projectileModel.GetBehavior<DamageModifierForTagModel>().damageAddative += 3;
        }

        var ability = Game.instance.model.GetTowerFromId("MonkeyBuccaneer-040").Duplicate().GetBehavior<AbilityModel>();
        towerModel.AddBehavior(ability);

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}

public class MasterofFairy : UpgradePlusPlus<SylveonPath>
{
    public override int Cost => 49000; //3000+350+
    public override int Tier => 5;
    public override string Icon => VanillaSprites.NavarchOfTheSeasUpgradeIcon;

    public override string Description => "Increased range, damage, attack speed and pierce. Ability has much shorter cooldown and can takedown ZOMGs";
    public override string Portrait => "700Sylveon_PSMD";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            damageModel.damage += 1;
        }
        foreach (var projectileModel in towerModel.GetDescendants<ProjectileModel>().ToArray())
        {
            projectileModel.pierce += 3;
            if (projectileModel.GetBehavior<DamageModifierForTagModel>() is not null) projectileModel.GetBehavior<DamageModifierForTagModel>().damageAddative += 10;
        }
        towerModel.RemoveBehaviors<AbilityModel>();


        var ability = Game.instance.model.GetTowerFromId("MonkeyBuccaneer-050").Duplicate().GetBehavior<AbilityModel>();
        if (ability is not null)
        {
            foreach (TargetGrapplableModel targetGrapplableModel in ability.GetDescendants<TargetGrapplableModel>().ToArray())
            {
                targetGrapplableModel.hooks = 1;
                targetGrapplableModel.zomgHooksRequired = 1;
            }
        }
        towerModel.AddBehavior(ability);

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}