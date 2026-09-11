using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Ultrarogue.Characters
{
    public class Gutterman : BaseCharacter
    {
        public override string Name => "Gutterman";
        public override string Description => "Start with the Attractor Nail Gun and Spiky nails. Nailgun has infinite ammo.";
        public override string Detail => "Start with Attractor Nail Gun and Spiky Nails. The Nailgun has no limit on how many nails you can fire, and nailbomb kills heal you" +
            " (clearing rooms also heal you). Have a slight damage reduction of 20% towards any non explosion damage but increase explosion damage by 20%.";

        public override List<Passive> Passives => new List<Passive>() { Passive.InfiniteAmmo };
        public override List<string> StartingItems => new List<string>() { "Spiky Nails" };
        public override List<AWeapon> StartingWeapons => new List<AWeapon>() { new AWeapon(Plugin.Weapon.Nailgun, Plugin.Variant.Blue) };
        Change SpeedChange = new Change(percentage: -0.35f);
        Change DamageModifier = new Change(percentage: -0.35f);
        

        PlayerChange change;
        public override void Update(bool selected)
        {
            if (change == null)
                change = new PlayerChange(moveSpeed: SpeedChange, damageReduction: DamageModifier);
            if (selected)
            {
                SpeedChange.percentage = -0.35f;
                DamageModifier.percentage = -0.20f;
            }
            else
            {
                SpeedChange.percentage = 0;
                DamageModifier.percentage = 0;
            }

            
        }
    }

    [HarmonyPatch(typeof(Explosion), nameof(Explosion.Start))]
    public class IncreaseDamage
    {
        public static void Prefix(Explosion __instance)
        {
            if(__instance.enemy && Plugin.SelectedChar.GetType() == typeof(Gutterman))
            {
                __instance.damage = Mathf.RoundToInt((float)__instance.damage * 1.40f);
            }
        }
    }
    [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.AddPoints))]
    public class HealNailBombs
    {
        public static void Prefix(string pointID)
        {
            if (Plugin.SelectedChar.GetType() == typeof(Gutterman))
            {
                if (pointID == "ultrakill.nailbombed")
                {
                    NewMovement.Instance.GetHealth(25, false);
                }
            }
        }
    }
}
