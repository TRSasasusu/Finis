using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Finis {
    [HarmonyPatch]
    public static class Patches {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(OWItem), nameof(OWItem.MoveAndChildToTransform))]
        public static void OWItem_MoveAndChildToTransform_Prefix(ref Transform socketTransform, OWItem __instance) {
            if (socketTransform.parent.name != "ItemCarryTool") {
                return;
            }
            if(__instance.name == "Rod") {
                Finis.Log("Set Rod in RodSocket");
                socketTransform = StateController.Instance.RodSocket;
            }
        }
    }
}
