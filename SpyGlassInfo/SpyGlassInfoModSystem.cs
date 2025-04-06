using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using HarmonyLib;
using System.Reflection;
using System;
using Vintagestory.Server;
using System.Runtime.CompilerServices;
using spyglass.src;
namespace SpyGlassInfo
{
    [HarmonyPatch]
    public class SpyGlassInfoModSystem : ModSystem
    {
        public static ICoreAPI api;
        public Harmony harmony;
        public override void Start(ICoreAPI api)
        {
            SpyGlassInfoModSystem.api = api;
            if (!Harmony.HasAnyPatches("SpyGlassInfo"))
            {
                harmony = new Harmony("SpyGlassInfo");
                harmony.PatchAll();
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("spyglass.src.ItemSpyglass", "OnHeldInteractStart")]
        public static void IncreaseRange(EntityAgent byEntity)
        {
            if (byEntity is not EntityPlayer playerEntity) return;
            playerEntity.Player.WorldData.PickingRange = 1000;

        }

        [HarmonyPrefix]
        [HarmonyPatch("spyglass.src.ItemSpyglass", "OnHeldInteractStop")]
        public static void ResetRange(EntityAgent byEntity)
        {
            if (byEntity is not EntityPlayer playerEntity) return;
            
            if(playerEntity.Player.WorldData.CurrentGameMode == EnumGameMode.Survival)
            {
                playerEntity.Player.WorldData.PickingRange = GlobalConstants.DefaultPickingRange;
            }
            else if (playerEntity.Player.WorldData.CurrentGameMode == EnumGameMode.Creative)
            {
                playerEntity.Player.WorldData.PickingRange = 1000;

            }


        }
        [HarmonyPrefix]
        [HarmonyPatch("spyglass.src.ItemSpyglass", "GetHeldInteractionHelp")]
        public static void CheckMouseButtons()
        {
            api.Logger.Notification("debug mousebutton");
            
        }
    }
}