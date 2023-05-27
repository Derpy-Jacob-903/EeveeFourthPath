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
using MelonLoader.TinyJSON;
using BTD_Mod_Helper.Api;
using System.Collections.Generic;

namespace Eevee.Upgrades.Paragon
{

    public class EeveeModVanillaParagon : ModVanillaParagon
    {
        public override string BaseTower => "Eevee-Eevee";
        public override string Name => "Eevee-Eevee";
    }
    public class EeveeParagon : ModParagonUpgrade<EeveeModVanillaParagon>
    {
        public override int Cost => 000000;
        public override string Description => "Sometimes the hand of fate must be forced...";
        public override string DisplayName => "Eevee Paragon";
        public override string DisplayNamePlural => base.DisplayNamePlural;

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var Vee = GetTowerModel<Eevee>(0, 0, 0).Duplicate();
            var Glac = GetTowerModel<Eevee>(2, 0, 5).Duplicate();
            //var Glac2 = GetTowerModel<Eevee>(2, 0, 3).Duplicate();

            Glac.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            //Glac2.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

            var glaceProjectileModel = towerModel.GetAttackModel().GetDescendant<ProjectileModel>();

            var Jolt = GetTowerModel<Eevee>(5, 2, 0).Duplicate();

            var Flare = GetTowerModel<Eevee>(0, 5, 0).Duplicate();
            var Mage = Game.instance.model.GetTowerFromId(TowerType.WizardMonkey + "-402").Duplicate();

            towerModel.GetAttackModel().SetWeapon(Flare.GetAttackModel().weapons[0]);
            towerModel.GetAttackModel().weapons[0].Rate *= 10; // 'Master Of Fire' fires 10 times as fast as 'Inflamed Flareon', which is laggy with all the 'CreateProjectileOnContact' stuff.
            towerModel.GetAttackModel().weapons[0].Rate *= 0.6f;

            //towerModel.GetAttackModel().AddWeapon(Jolt.GetAttackModel().weapons[1]);

            towerModel.GetAttackModel().AddWeapon(Game.instance.model.GetTowerFromId("Druid-500").GetAttackModel().weapons[2].Duplicate()); //weapons[1]  // Superstorm Ball Lightning
            //towerModel.GetAttackModel().AddWeapon(Glac.GetAttackModel().weapons[0]); //Icicle Impale

            towerModel.GetAttackModel().AddBehavior(Game.instance.model.GetTowerFromId("IceMonkey-205").GetAttackModel().weapons[0].Duplicate());


            TowerModel druid = Game.instance.model.GetTowerFromId(TowerType.Druid + "-200");

            // Create lightning weapon and increase attack speed
            //WeaponModel lightningWeapon = druid.GetAttackModel().weapons[1].Duplicate();

            towerModel.GetAttackModel().weapons[0].projectile.display = Mage.GetAttackModel().weapons[0].projectile.display;

            foreach (WeaponModel weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
            {
                var balls = Glac.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                balls.projectile = Jolt.GetAttackModel().weapons[0].projectile;
                balls.emission = druid.GetAttackModel().weapons[1].emission;

                //var IceMonke = Glac.GetAttackModel().weapons[0].projectile.GetBehavior<FreezeModel>().projectile.;
                //AddBehaviorToBloonModel laserShock = Game.instance.model.GetTowerFromId("Gwendolin 20").GetDescendant<AddBehaviorToBloonModel>().Duplicate();
                weaponModel.projectile.AddBehavior(balls);
            }

            foreach (ProjectileModel projectileModel in towerModel.GetDescendants<ProjectileModel>().ToArray())
            {
                if (!projectileModel.HasBehavior<FreezeModel>())
                {
                    projectileModel.AddBehavior(Glac.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<FreezeModel>().Duplicate());
                }
            }
            //foreach (WeaponModel weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
            //{
            //    foreach (AddBehaviorToBloonModel addBehaviorToBloonModel in Glac.GetDescendants<AddBehaviorToBloonModel>().ToArray())
            //    {
                    //AddBehaviorToBloonModel laserShock = Glac.GetDescendant<AddBehaviorToBloonModel>().Duplicate();
            //        FreezeModel_Ice
            //        weaponModel.projectile.AddBehavior(addBehaviorToBloonModel);
            //    }
                //AddBehaviorToBloonModel laserShock = Flare.GetDescendant<AddBehaviorToBloonModel>().Duplicate();
                //weaponModel.projectile.AddBehavior(laserShock);
            //}

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

            towerModel.ApplyDisplay<EeveeParagonDisplay>();

            foreach (var damageModel in towerModel.GetDescendants<ParagonAssetSwapModel>().ToArray())
            {
                towerModel.RemoveBehavior<ParagonAssetSwapModel>(damageModel);
            }
            //foreach (var damageModel in towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ToArray())
            //{
            //    damageModel.assetPath.guidRef = GetDisplayGUID<EeveeParagonDisplay>();
            //}
            //towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<ParagonMagicProj>();
        }
    }
    public class EeveeParagonDisplay : ModTowerDisplay<EeveeModVanillaParagon>
        {
        public override float Scale => 30f; //+ ParagonDisplayIndex * .025f;  // Higher degree Paragon displays will be bigger

        //public override string BaseDisplay =>  // The floating monkey part of the True Sun God
        public override string BaseDisplay => GetDisplayGUID<EeveeDisplay>();

        public override bool UseForTower(int[] tiers) => IsParagon(tiers);

        /// <summary>
        /// All classes that derive from ModContent MUST have a zero argument constructor to work
        /// </summary>
        public EeveeParagonDisplay()
        {
        }

        public EeveeParagonDisplay(int i)
        {
            ParagonDisplayIndex = i;
        }

        public override int ParagonDisplayIndex { get; }  // Overriding in this way lets us set it in the constructor

        /// <summary>
        /// Create a display for each possible ParagonDisplayIndex
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ModContent> Load()
        {
            for (var i = 0; i < TotalParagonDisplays; i++)
            {
                yield return new EeveeParagonDisplay(i);
            }
        }


        public override string Name => nameof(EeveeParagonDisplay) + ParagonDisplayIndex;  // make sure each instance has its own name

        /// <summary>
        /// Could use the ParagonDisplayIndex property to use different effects based on the paragon strength
        /// </summary>
        /// <param name="node"></param>
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            node.PrintInfo();
            node.SaveMeshTexture();
            SetMeshTexture(node, "pm0133_00_Body1");
            //SetMeshTexture(node, "pm0133_00_Eye1", 1);
            //SetMeshTexture(node, "pm0133_00_Mouth1", 2);
        }
    }
    public class ParagonMagicProj : ModDisplay
    {

        public override string BaseDisplay => "7367b996679da4f4a900902bf8d2a76f";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            node.PrintInfo();
            node.SaveMeshTexture();
            //SetMeshTexture(node, "ArcaneShot");
        }
    }
}