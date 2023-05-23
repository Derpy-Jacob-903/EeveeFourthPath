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
using BTD_Mod_Helper.Api.Towers;
using Eevee;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.Towers.Mods;
using Il2CppAssets.Scripts.Utils;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using System.Linq;
using Il2CppNinjaKiwi.NKMulti.Transfer;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using UnityEngine;
using Eevee.Upgrades.MiddlePath;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using System;

namespace Eevee
{
    public class AltEevee : ModTower
    {
        public override TowerSet TowerSet => TowerSet.Magic;
        public override string Name => "Eevee";
        public override string BaseTower => TowerType.Druid;
        public override int Cost => 400;

        public override int TopPathUpgrades => 5;
        public override int MiddlePathUpgrades => 5;
        public override int BottomPathUpgrades => 5;
        public override string Description => "A Pokemon-tower with several Evolutions";
        public override ParagonMode ParagonMode => ParagonMode.Base000;
        public override string DisplayName => "Alternate Eevee";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            //towerModel.range += 10;
            //var attackModel = towerModel.GetAttackModel();
            //attackModel.range += 10;

            //var projectile = attackModel.weapons[0].projectile;
            //projectile.pierce += 2;
            //towerModel.ApplyDisplay<EeveeDisplay>();
        }

    }
}
namespace Eevee.Upgrades.TopPath
{
    public class SharpPins : ModUpgrade<AltEevee>
    {
        public override int Path => TOP;
        public override int Tier => 1;
        public override int Cost => 400;
        public override int Priority => -1;
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("DartMonkey-100").GetUpgrade(TOP, 1).icon;
        public override string Description => "Can pop 1 extra Bloon per shot.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
            {
                weaponModel.projectile.pierce += 1;
            }
        }
    }
    public class PinRicochet : ModUpgrade<AltEevee>
    {
        public override int Path => TOP;
        public override int Tier => 2;
        public override int Cost => 400;
        public override int Priority => -1;
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("DartMonkey-100").GetUpgrade(TOP, 1).icon;
        public override string Description => "Pins ricochet from Bloon to Bloon.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
            {
                weaponModel.projectile.AddBehavior(Game.instance.model.GetTowerFromId("BoomerangMonkey-300").GetAttackModel().weapons[0].projectile.GetBehavior<RetargetOnContactModel>().Duplicate());
                weaponModel.projectile.pierce += 1;
            }
        }
    }
    public class AltJolteon : ModUpgrade<AltEevee>
    {
        public override int Path => TOP;
        public override int Cost => 1300;
        public override int Tier => 3;
        public override string Portrait => "BlitzaPortrait";
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("Druid").GetUpgrade(TOP, 2).icon;
        public override string DisplayName => "Jolteon";

        public override string Description => "Discharges electricity in a small area. Long and Super Range Tacks give more splitting.";
        public override int Priority => 1;

        public override void ApplyUpgrade(TowerModel towerModel)
        {



            TowerModel druid = Game.instance.model.GetTowerFromId(TowerType.Druid + "-200");

            // Create lightning weapon and increase attack speed
            WeaponModel lightningWeapon = druid.GetAttackModel().weapons[1].Duplicate();
            lightningWeapon.Rate = towerModel.GetAttackModel().weapons[0].Rate / 1.7f;
            lightningWeapon.animation = 1;
            lightningWeapon.name = "TeslaCoil_LightningWeapon";

            // Edit lightning projectile to split less
            ProjectileModel lightningProjectile = lightningWeapon.projectile;
            lightningProjectile.pierce = towerModel.GetAttackModel().weapons[0].projectile.pierce + 47f;
            lightningProjectile.GetBehavior<LightningModel>().splitRange = towerModel.range / 2.5f;
            //int splits = 1;
            //if (towerModel.appliedUpgrades.Contains(UpgradeType.LongRangeTacks)) splits++;
            //if (towerModel.appliedUpgrades.Contains(UpgradeType.SuperRangeTacks)) splits++;
            //lightningProjectile.GetBehavior<LightningModel>().splits = splits;
            lightningProjectile.GetBehavior<LightningModel>().splits = 2;

            // Add laser shock to lightning
            //AddBehaviorToBloonModel laserShock = towerModel.GetDescendant<AddBehaviorToBloonModel>().Duplicate();
            //lightningProjectile.AddBehavior(laserShock);
            //lightningProjectile.collisionPasses = new[] { 0, 1 };

            // Add first lightning weapon
            towerModel.GetAttackModel().name = "TeslaCoil_LightningAttack";
            towerModel.GetAttackModel().SetWeapon(lightningWeapon, 0);
            towerModel.GetAttackModel().range += 10;
            //towerModel.GetAttackModel().RemoveBehavior<TargetCloseModel>();
            //towerModel.GetAttackModel().AddBehavior(new RandomTargetModel("RandomTargetModel", true, false));
            //towerModel.GetAttackModel().GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

            foreach (WeaponModel weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
            {
                if (weaponModel.name == "TeslaCoil_LightningWeapon")
                {
                    if (towerModel.appliedUpgrades.Contains("Heavy Pins")) weaponModel.projectile.GetDamageModel().damage++;
                }
            }
            // Add remaining lightning weapons
            int count = 5;
            if (towerModel.appliedUpgrades.Contains("More Pins")) count += 3;
            //if (towerModel.appliedUpgrades.Contains(UpgradeType.EvenMoreTacks)) count += 2;

            for (int i = 1; i < count; i++)
            {
                towerModel.AddBehavior(towerModel.GetAttackModel("TeslaCoil_LightningAttack").Duplicate());
            }

            //towerModel.ApplyDisplay<JolteonDisplay>();
            // Buff laser shock ticks
            //foreach (AddBehaviorToBloonModel addBehavior in towerModel.GetDescendants<AddBehaviorToBloonModel>().ToArray())
            //{
            //    addBehavior.lifespan = 1.55f;
            //    addBehavior.GetBehavior<DamageOverTimeModel>().Interval = 0.5f;
            //}

            towerModel.range += 10;
        }
    }
    public class AltElectroBall : ModUpgrade<AltEevee>
    {
        public override int Path => TOP;
        public override int Tier => 4;
        public override int Cost => 19000;
        public override string Portrait => "BlitzaPortrait";
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("Druid-400").GetUpgrade(TOP, 4).icon;
        public override string Description => "Jolteon attacks with an electro ball attack and increased damage";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectileModel = attackModel.GetDescendant<ProjectileModel>();
            attackModel.weapons[0].projectile.GetDamageModel().damage += 1;
            attackModel.AddWeapon(Game.instance.model.GetTowerFromId("Druid-400").GetAttackModel().weapons[2].Duplicate());
            projectileModel.pierce += 50;
        }
    }
    public class AltMasterOfElectricity : ModUpgrade<AltEevee>
    {
        public override int Path => TOP;
        public override int Tier => 5;
        public override int Cost => 35000;
        public override string Portrait => "BlitzaPortrait2";
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("EngineerMonkey-050").GetUpgrade(MIDDLE, 5).icon;
        public override string Description => "More damage, pierce and range";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var projectileModel = towerModel.GetAttackModel().GetDescendant<ProjectileModel>();
            var attackModel = towerModel.GetAttackModel();
            towerModel.range += 20;
            attackModel.range += 20;
            attackModel.weapons[0].projectile.GetDamageModel().damage += 8;
            //ttackModel.weapons[0].projectile.GetDamageModel().damage += 4;
            //projectileModel.pierce += 8;
        }
    }
}
namespace Eevee.Upgrades.MiddlePath
{
    public class EeveeSeesEverything : ModUpgrade<AltEevee>
    {
        public override int Path => MIDDLE;
        public override int Tier => 1;
        public override int Cost => 250;
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("SniperMonkey-010").GetUpgrade(MIDDLE, 1).icon;
        public override string Description => "Eevee can target Camo Bloons. Attacks do +1 damage to Camo Bloons.";
        public override int Priority => -2;

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
    }
    public class StrongEevee : ModUpgrade<AltEevee>
    {
        public override int Path => MIDDLE;
        public override int Tier => 2;
        public override int Cost => 500;
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("Druid-100").GetUpgrade(TOP, 1).icon;
        public override string Description => "Attacks pop 2 layers of Bloon, plus 1 to Ceramics and Fortified Bloons.";
        public override int Priority => -1;

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var projectileModel = towerModel.GetAttackModel().GetDescendant<ProjectileModel>();
            projectileModel.GetDamageModel().damage += 1;
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_", "Ceramic,Fortified", 1, 1, false, false));

        }
    }
    public class AltFlareon : ModUpgrade<AltEevee>
    {
        public override int Path => MIDDLE;
        public override int Tier => 3;
        public override int Cost => 2500;
        public override string Portrait => "FlareonPortrait";
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("WizardMonkey-010").GetUpgrade(MIDDLE, 1).icon;
        public override string Description => "Evolving Eevee to Flareon and increases the attack speed and pierce";
        public override int Priority => 3;

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectileModel = attackModel.GetDescendant<ProjectileModel>();
            //projectileModel.pierce += 2;
            attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("Gwendolin 6").GetAttackModel().weapons[0].projectile.Duplicate();
            //towerModel.GetWeapon().rate *= 0.5f;
            attackModel.weapons[0].projectile.SetHitCamo(true);

            towerModel.range += 10;
            attackModel.range += 10;
            towerModel.ApplyDisplay<FlareonDisplay>();
        }
    }
    public class OverheatFlareon : ModUpgrade<AltEevee>
    {
        public override int Path => MIDDLE;
        public override int Tier => 4;
        public override int Cost => 25000;
        public override string Portrait => "FlareonPortrait2";
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("WizardMonkey-030").GetUpgrade(MIDDLE, 3).icon;
        public override string Description => "Overheat Ability";
        public override int Priority => 2;

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectileModel = attackModel.GetDescendant<ProjectileModel>();
            //projectileModel.GetDamageModel().damage += 8;
            //projectileModel.pierce += 5;
            attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("Gwendolin 15").GetAttackModel().weapons[0].projectile.Duplicate(); //
            attackModel.weapons[0].projectile.SetHitCamo(true);
            projectileModel.GetDamageModel().damage = 4;

            var ability = Game.instance.model.GetTowerFromId("BoomerangMonkey-040").Duplicate<TowerModel>().GetBehavior<AbilityModel>();
            var ability2 = Game.instance.model.GetTowerFromId("Benjamin 3").Duplicate<TowerModel>().GetBehavior<AbilityModel>();
            ability2.cooldown = 60;
            ability2.name = "AbilityModel_FlareonBiohackAbilityOld";
            ability2.GetBehavior<BiohackModel>().affectedCount = 1;
            ability2.GetBehavior<BiohackModel>().damageIncrease = 4;
            ability2.GetBehavior<BiohackModel>().lifespan *= 3;
            foreach (DelayedShutoffModel delayedShutoffModel in ability2.GetBehavior<BiohackModel>().GetDescendants<DelayedShutoffModel>().ToArray())
            {
                delayedShutoffModel.delay *= 3;
                delayedShutoffModel.shutoffTime *= 4;
            }
            ability.GetBehavior<TurboModel>().lifespan = ability2.GetBehavior<BiohackModel>().GetDescendants<DelayedShutoffModel>().ToArray().First().delay;
            ability.GetBehavior<TurboModel>().extraDamage = 8;
            ability.GetBehavior<TurboModel>().multiplier = 0.6f;
            //ability.AddBehavior<DelayedShutoffModel>(ability2.GetBehavior<BiohackModel>().GetDescendants<DelayedShutoffModel>().ToArray().First());

            ability.name = "AbilityModel_FlareonBiohackAbility";
            //TurboModel
            towerModel.AddBehavior(ability);


        }
    }
    public class AltMasterOfFire : ModUpgrade<AltEevee>
    {
        public override int Path => MIDDLE;
        public override int Tier => 5;
        public override int Cost => 42000;
        public override string Portrait => "FlareonPortrait3";
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("TackShooter-500").GetUpgrade(TOP, 5).icon;
        public override string Description => "Increased range, pierce, damage and attack speed";
        public override int Priority => 1;

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectileModel = attackModel.GetDescendant<ProjectileModel>();
            attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("Gwendolin 20").GetAttackModel().weapons[0].projectile.Duplicate(); //
            attackModel.weapons[0].projectile.SetHitCamo(true);
            projectileModel.GetDamageModel().damage = 12;
            towerModel.GetWeapon().rate *= 0.6f;

            towerModel.GetBehavior<AbilityModel>().GetBehavior<TurboModel>().extraDamage = 24;
        }
    }
}
namespace Eevee.Upgrades.BottomPath
{
    public class FasterEeveeAlt : ModUpgrade<AltEevee>
    {
        public override int Path => BOTTOM;
        public override int Tier => 1;
        public override int Cost => 320;
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("DartMonkey-010").GetUpgrade(MIDDLE, 1).icon;
        public override string Description => "Eevee throws pins 30% faster!";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetWeapon().rate *= 0.70f;
        }
    }
    public class FasterPins : ModUpgrade<AltEevee>
    {
        public override int Path => BOTTOM;
        public override int Tier => 2;
        public override int Cost => 500;
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("DartMonkey-020").GetUpgrade(MIDDLE, 2).icon;
        public override string Description => "Pins move much faster through the air. Eevee throws pins a bit faster as well.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetWeapon().rate *= 0.7f;

            var attackModel = towerModel.GetAttackModel();
            var projectileModel = attackModel.GetDescendant<ProjectileModel>();
            //projectileModel.GetDescendant<TravelStraitModel>().Speed *= 1.5f;
            //projectileModel.GetDescendant<TravelStraitModel>().speed *= 1.5f;

        }
    }
    public class AltGlaceon : ModUpgrade<AltEevee>
    {
        public override int Path => BOTTOM;
        public override int Tier => 3;
        public override int Cost => 2500;
        public override string Portrait => "GlaceonPortrait";
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("IceMonkey-004").GetUpgrade(BOTTOM, 3).icon;
        public override string Description => "Evolving Eevee to Glaceon and faster attack speed";
        public override int Priority => 3;

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("SentryCold").GetAttackModel().weapons[0].projectile.Duplicate();
            towerModel.GetWeapon().rate *= 0.8f;
            attackModel.weapons[0].projectile.SetHitCamo(true);
            towerModel.ApplyDisplay<GlaceonDisplay>();
        }
    }
    public class Snowscape : ModUpgrade<AltEevee>
    {
        public override int Path => BOTTOM;
        public override int Tier => 4;
        public override int Cost => 2500;
        public override string Portrait => "GlaceonPortrait";
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("IceMonkey-004").GetUpgrade(MIDDLE, 3).icon;
        public override string Description => "Evolving Eevee to Glaceon and faster attack speed";
        public override int Priority => 2;

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile = Game.instance.model.GetTowerFromId("IceMonkey-103").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();

            towerModel.GetWeapon().rate *= 0.8f;
            attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 1;
            attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce += 20;
            //attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("IceMonkey-103").GetAttackModel().weapons[0].projectile.Duplicate();
            attackModel.weapons[0].projectile.SetHitCamo(true);

            towerModel.AddBehavior<FreezeNearbyWaterModel>(Game.instance.model.GetTowerFromId("IceMonkey-130").GetDescendant<FreezeNearbyWaterModel>());
            towerModel.AddBehavior<SlowBloonsZoneModel>(Game.instance.model.GetTowerFromId("IceMonkey-130").GetDescendant<SlowBloonsZoneModel>());
        }
    }
    public class AltMasterOfIce : ModUpgrade<AltEevee>
    {
        public override int Path => BOTTOM;
        public override int Tier => 5;
        public override int Cost => 50000;
        public override string Portrait => "GlaceonPortrait";
        public override SpriteReference IconReference => Game.instance.model.GetTowerFromId("IceMonkey-005").GetUpgrade(BOTTOM, 5).icon;
        public override string Description => "Increased range, damage, attack speed and pierce";
        public override int Priority => 1;

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var projectileModel = attackModel.GetDescendant<ProjectileModel>();
            projectileModel.display = Game.instance.model.GetTowerFromId("IceMonkey-105").GetAttackModel().weapons[0].projectile.display;
            attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile = Game.instance.model.GetTowerFromId("IceMonkey-105").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
            //attackModel.AddBehavior(Game.instance.model.GetTowerFromId("IceMonkey-105").GetAttackModel().weapons[0].Duplicate());
            towerModel.range += 20;
            attackModel.range += 20;
            projectileModel.GetDamageModel().damage += 10;
            projectileModel.pierce += 3;
            towerModel.GetWeapon().rate *= 0.75f;
        }
    }
}

namespace Eevee.Upgrades
{
    public class AltEeveeParagon : ModParagonUpgrade<AltEevee>
    {
        public override int Cost => 200000;
        public override string Description => "Sometimes the hand of fate must be forced...";
        public override string DisplayName => "AltEevee Paragon";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var Vee = GetTowerModel<AltEevee>(0, 0, 0).Duplicate();
            var Glac = GetTowerModel<AltEevee>(2, 0, 5).Duplicate();

            Glac.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

            var glaceProjectileModel = towerModel.GetAttackModel().GetDescendant<ProjectileModel>();
            glaceProjectileModel.GetDamageModel().damage += 1;
            Glac.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_", "Ceramic,Fortified", 1, 1, false, false));

            var Jolt = GetTowerModel<AltEevee>(5, 2, 0).Duplicate();
            Jolt.GetWeapon().rate *= 0.70f;
            Jolt.GetWeapon().rate *= 0.70f;

            var Flare = GetTowerModel<AltEevee>(0, 5, 0).Duplicate();

            towerModel.GetAttackModel().SetWeapon(Glac.GetAttackModel().weapons[0]);

            TowerModel druid = Game.instance.model.GetTowerFromId(TowerType.Druid + "-200");

            // Create lightning weapon and increase attack speed
            WeaponModel lightningWeapon = druid.GetAttackModel().weapons[1].Duplicate();
            //lightningWeapon.Rate = towerModel.GetAttackModel().weapons[0].Rate / 1.7f;
            //lightningWeapon.animation = 1;
            //lightningWeapon.name = "TeslaCoil_LightningWeapon";

            // Edit lightning projectile to split less
            //ProjectileModel lightningProjectile = lightningWeapon.projectile;
            //lightningProjectile.pierce = towerModel.GetAttackModel().weapons[0].projectile.pierce * 2 + 2f;
            //lightningProjectile.GetBehavior<LightningModel>().splitRange = towerModel.range / 2.5f;
            //int splits = 1;
            //if (towerModel.appliedUpgrades.Contains(UpgradeType.LongRangeTacks)) splits++;
            //if (towerModel.appliedUpgrades.Contains(UpgradeType.SuperRangeTacks)) splits++;
            //lightningProjectile.GetBehavior<LightningModel>().splits = splits;
            //lightningProjectile.GetBehavior<LightningModel>().splits = 8;



            // Add laser shock to lightning
            //AddBehaviorToBloonModel laserShock = towerModel.GetDescendant<AddBehaviorToBloonModel>().Duplicate();
            //lightningProjectile.AddBehavior(laserShock);
            //lightningProjectile.collisionPasses = new[] { 0, 1 };

            // Add first lightning weapon
            //towerModel.GetAttackModel().name = "TeslaCoil_LightningAttack";
            //towerModel.GetAttackModel().SetWeapon(lightningWeapon, 0);
            //towerModel.GetAttackModel().range += 10;
            //towerModel.GetAttackModel().RemoveBehavior<TargetCloseModel>();
            //towerModel.GetAttackModel().AddBehavior(new RandomTargetModel("RandomTargetModel", true, false));
            //towerModel.GetAttackModel().GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);


            foreach (WeaponModel weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
            {

                var balls = weaponModel.projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                balls.projectile = lightningWeapon.projectile;
                balls.emission = Jolt.GetAttackModel().weapons[0].emission;
                //Game.instance.model.GetTowerFromId("IceMonkey-105").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();

                //AddBehaviorToBloonModel laserShock = Game.instance.model.GetTowerFromId("Gwendolin 20").GetDescendant<AddBehaviorToBloonModel>().Duplicate();
                weaponModel.projectile.AddBehavior(balls);
            }

            foreach (WeaponModel weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
            {
                //weaponModel.projectile.GetDamageModel().damage = 24;
                //damageModel.damage = 24 + (1 per degree over 20);
            }
            foreach (WeaponModel weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
            {
                AddBehaviorToBloonModel laserShock = Flare.GetDescendant<AddBehaviorToBloonModel>().Duplicate();
                weaponModel.projectile.AddBehavior(laserShock);
            }

            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

            towerModel.range += 60;
            foreach (AttackModel attackModel in towerModel.GetDescendants<AttackModel>().ToArray())
            {
                attackModel.range += 60;
            }
            foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
            {
                damageModel.immuneBloonProperties = BloonProperties.None;
            }
        }
    }
    public class EeveeModVanillaParagon : ModVanillaParagon
    {
        public override string BaseTower => "Eevee-Eevee-005";
        public override string Name => "Eevee-Eevee";
    }
    public class EeveeParagon : ModParagonUpgrade<EeveeModVanillaParagon>
    {
        public override int Cost => 200000;
        public override string Description => "Sometimes the hand of fate must be forced...";
        public override string DisplayName => "Eevee Paragon";
        public override string DisplayNamePlural => base.DisplayNamePlural;

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var Vee = GetTowerModel<Eevee>(0, 0, 0).Duplicate();
            var Glac = GetTowerModel<Eevee>(2, 0, 5).Duplicate();

            Glac.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

            var glaceProjectileModel = towerModel.GetAttackModel().GetDescendant<ProjectileModel>();
            //glaceProjectileModel.GetDamageModel().damage += 1;
            Glac.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_", "Ceramic,Fortified", 1, 1, false, false));

            var Jolt = GetTowerModel<Eevee>(5, 2, 0).Duplicate();
            Jolt.GetWeapon().rate *= 0.70f;
            Jolt.GetWeapon().rate *= 0.70f;

            var Flare = GetTowerModel<Eevee>(0, 5, 0).Duplicate();

            towerModel.GetAttackModel().SetWeapon(Glac.GetAttackModel().weapons[0]);

            TowerModel druid = Game.instance.model.GetTowerFromId(TowerType.Druid + "-200");

            // Create lightning weapon and increase attack speed
            WeaponModel lightningWeapon = druid.GetAttackModel().weapons[1].Duplicate();

            foreach (WeaponModel weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
            {

                var balls = weaponModel.projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                balls.projectile = lightningWeapon.projectile;
                balls.emission = Jolt.GetAttackModel().weapons[0].emission;
                //Game.instance.model.GetTowerFromId("IceMonkey-105").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();

                //AddBehaviorToBloonModel laserShock = Game.instance.model.GetTowerFromId("Gwendolin 20").GetDescendant<AddBehaviorToBloonModel>().Duplicate();
                weaponModel.projectile.AddBehavior(balls);
            }

            foreach (WeaponModel weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
            {
                //weaponModel.projectile.GetDamageModel().damage = 24;
                //damageModel.damage = 24 + (1 per degree over 20);
            }
            foreach (WeaponModel weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
            {
                AddBehaviorToBloonModel laserShock = Flare.GetDescendant<AddBehaviorToBloonModel>().Duplicate();
                weaponModel.projectile.AddBehavior(laserShock);
            }

            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

            towerModel.range += 60;
            foreach (AttackModel attackModel in towerModel.GetDescendants<AttackModel>().ToArray())
            {
                attackModel.range += 60;
            }
            foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
            {
                damageModel.immuneBloonProperties = BloonProperties.None;
            }

            var gwendolin = Game.instance.model.GetTowerFromId("Gwendolin 20").GetDescendant<PyrotechnicsSupportModel>().filters;
            var brickell = Game.instance.model.GetTowerFromId("AdmiralBrickell 3").GetDescendant<AbilityModel>().Duplicate();
            var boomerang = Game.instance.model.GetTowerFromId("BoomerangMonkey-040").GetDescendant<AbilityModel>().Duplicate();

            var ParagonOverheat = new AbilityModel("ParagonOverheat", "Paragon Overheat", "", -1, 0, brickell.icon, 40, null, false, false, "", 0, 0, -1, false, false);
            ActivateRateSupportZoneModel balls1 = new ActivateRateSupportZoneModel("ActivateRateSupportZoneModel_EeveeParagon", "EeveePargonRate", true, 0.6f, 999999, 999999, true, 10, brickell.GetBehavior<DisplayModel>(), brickell.GetDescendant<ActivateRateSupportZoneModel>().buffIconName, brickell.GetDescendant<ActivateRateSupportZoneModel>().buffIconName, gwendolin, false);
            //balls1.displayModel.display.guidRef = boomerang.GetBehavior<CreateEffectOnAbilityModel>().effectModel.assetId.guidRef;
            ActivateDamageModifierSupportZoneModel balls2 = new ActivateDamageModifierSupportZoneModel("ActivateDamageModifierSupportZoneModel_EeveeParagon", "EeveePargonRate", true, 999999, 999999, true, 10, new DamageModifierModel("DamageModifierModel_EeveeParagon"), filters: gwendolin);
            //var balls3 = ; 

            towerModel.AddBehavior(ParagonOverheat);
        }
    }
}