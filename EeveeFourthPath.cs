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
using Il2CppTMPro;
using Il2CppAssets.Scripts.Models.SimulationBehaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;

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

    public override int UpgradeCount => 3; // Increase this up to 5 as you create your Upgrades
}
public class VaporeonPath : PathPlusPlus
{
    public override string Tower => "Eevee-Eevee";

    public override int UpgradeCount => 3; // Increase this up to 5 as you create your Upgrades
}
public class ExtraPins : UpgradePlusPlus<SylveonPath>
{
    public override int Cost => 250;
    public override int Tier => 1;
    public override string Icon => VanillaSprites.LotsMoreDartsUpgradeIcon;

    public override string Description => "Throws 8 Pins at a time.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var randomEmissionModel in towerModel.GetDescendants<RandomEmissionModel>().ToArray())
        {
            randomEmissionModel.count += 3;
        }

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}
public class HardPins : UpgradePlusPlus<SylveonPath>
{
    public override int Cost => 300;
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
public class LongRangePins : UpgradePlusPlus<VaporeonPath>
{
    public override int Cost => 250;
    public override int Tier => 1;
    public override string Icon => VanillaSprites.LotsMoreDartsUpgradeIcon;

    public override string Description => "Increased range.";

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
            travelStraitModel.lifespanFrames = (int)(travelStraitModel.Lifespan*60);
        }

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}
public class PinRicochet : UpgradePlusPlus<VaporeonPath>
{
    public override int Cost => 300;
    public override int Tier => 2;
    public override string Icon => VanillaSprites.HeatTippedDartUpgradeIcon;

    public override string Description => "Pins bounce off obstacles.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var projectileModel in towerModel.GetDescendants<ProjectileModel>().ToArray())
        {
            if (!(towerModel.GetUpgradeLevel(0) >= 3)) //Is NOT Jolteon
            {
                projectileModel.AddBehavior<ProjectileBlockerCollisionReboundModel>(Game.instance.model.GetTowerFromId("DartMonkey-300").GetAttackModel().weapons[0].projectile.GetDescendant<ProjectileBlockerCollisionReboundModel>().Duplicate());
            }
        }
        foreach (var travelStraitModel in towerModel.GetDescendants<TravelStraitModel>().ToArray())
        {
            travelStraitModel.Lifespan *= 1.25f;
            travelStraitModel.lifespan = travelStraitModel.Lifespan;
            travelStraitModel.lifespanFrames = (int)(travelStraitModel.Lifespan * 60);
        }

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}
public class Vaporeon : UpgradePlusPlus<VaporeonPath>
{
    public override int Cost => 5600; //3000+350+
    public override int Tier => 3;
    public override string Icon => VanillaSprites.ArmorPiercingDartsUpgradeIcon;

    public override string Description => "Evolving Eevee to Vaporeon and greatly increases pierce and damage. Gains Darkshift ability to shift to a nearby location, including water.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var attackModel = towerModel.GetAttackModel();
        var oldProjectile = attackModel.weapons[0].projectile;
        towerModel.areaTypes = Game.instance.model.GetTowerFromId("PatFusty").areaTypes;
        //attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("MonkeyAce-003").GetAttackModel().weapons[0].projectile.Duplicate();

        attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("MonkeySub").GetWeapons()[0].projectile.Duplicate();
        foreach (var attackModel2 in towerModel.GetDescendants<AttackModel>().ToArray())
        {
            attackModel2.AddBehavior(new TargetFirstSharedRangeModel("TargetFirstShared", false, true, false, true));
            attackModel2.AddBehavior(new TargetLastSharedRangeModel("TargetLastShared", false, true, false, true));
            attackModel2.AddBehavior(new TargetCloseSharedRangeModel("TargetCloseShared", false, true, false, true));
            attackModel2.AddBehavior(new TargetStrongSharedRangeModel("TargetStrongShared", false, true, false, true));
        }
        foreach (var projectileModel in towerModel.GetDescendants<ProjectileModel>().ToArray())
        {
            projectileModel.pierce = 16;
            //projectileModel.AddBehavior<DamageModifierForTagModel>(new DamageModifierForTagModel("DamageModifierForTagModel_Projectile", "Moabs", 1.0f, 3.0f, false, false));
            if (!(towerModel.GetUpgradeLevel(0) >= 3)) //Is NOT Jolteon
            {
                projectileModel.AddBehavior<ProjectileBlockerCollisionReboundModel>(Game.instance.model.GetTowerFromId("DartMonkey-300").GetAttackModel().weapons[0].projectile.GetDescendant<ProjectileBlockerCollisionReboundModel>().Duplicate());
            }
        }
        foreach (var travelStraitModel in towerModel.GetDescendants<TravelStraitModel>().ToArray())
        {
            travelStraitModel.Lifespan *= 4f;
            travelStraitModel.lifespan = travelStraitModel.Lifespan;
            travelStraitModel.lifespanFrames = (int)(travelStraitModel.Lifespan * 60);
        }
        towerModel.AddBehavior(Game.instance.model.GetTowerFromId("SuperMonkey-003").GetBehavior<AbilityModel>().Duplicate());

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}
public class AcidArmor : UpgradePlusPlus<VaporeonPath>
{
    public override int Cost => 5000;
    public override int Tier => 4;
    public override string Icon => VanillaSprites.LotsMoreDartsUpgradeIcon;

    public override string Description => "Adds Submerge targeting option that produce 3 lives per round and coats nearby Bloons in acid. Vaporeon does not use its main attack while submerged.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        //Shield
        //new ShieldPerRoundModel("ShieldPerRoundModel_", 3);

        towerModel.AddBehavior<SubmergeModel>(Game.instance.model.GetTowerFromId("MonkeySub-300").GetBehavior<SubmergeModel>().Duplicate());
        towerModel.GetBehavior<SubmergeModel>().submergeAttackModel.GetDescendant<ProjectileModel>().RemoveBehavior<RemoveBloonModifiersModel>();
        towerModel.GetBehavior<SubmergeModel>().submergeAttackModel.GetDescendant<ProjectileModel>().AddBehavior(Game.instance.model.GetTowerFromId("Alchemist").GetWeapons()[0].projectile.GetBehavior<AddBehaviorToBloonModel>());
        //BonusLivesPerRound RemoveBloonModifiersModel
        //    AddBehaviorToBloonModel

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}
