using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Finis {
    [HarmonyPatch]
    public static class PatchForJP {
        static Font _japanese_dynamic_font;
        static Font _japanese_font;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TextTranslation), nameof(TextTranslation.SetLanguage))]
        public static void TextTranslation_SetLanguage_Postfix(ref TextTranslation.Language lang, TextTranslation __instance) {
            _japanese_dynamic_font = null;
            _japanese_font = null;

            if (lang == TextTranslation.Language.JAPANESE) {
                _japanese_dynamic_font = TextTranslation.GetFont(true);
                if (_japanese_dynamic_font == null) {
                    Finis.Log($"failed to get japanese dynamic font.");
                }
                else {
                    Finis.Log($"get japanese dynamic font.");
                }
                _japanese_font = TextTranslation.GetFont(false);
                if (_japanese_font == null) {
                    Finis.Log($"failed to get japanese font.");
                }
                else {
                    Finis.Log($"get japanese font.");
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShipLogFactListItem), nameof(ShipLogFactListItem.UpdateTextReveal))]
        public static void ShipLogFactListItem_UpdateTextReveal_Postfix(ShipLogFactListItem __instance) {
            if (_japanese_dynamic_font == null || _japanese_font == null) {
                return;
            }

            //Finis.Log($"Check text: {__instance._text.text}");
            if (__instance._text.text.Contains("<i>")) {
                __instance._text.font = _japanese_dynamic_font;
            }
            else {
                __instance._text.font = _japanese_font;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FontAndLanguageController), nameof(FontAndLanguageController.InitializeFont))]
        public static void FontAndLanguageController_InitializeFont_Postfix(FontAndLanguageController __instance) {
            if (__instance.name != "ShipLogFontAndLanguageController") {
                return;
            }
            if (_japanese_dynamic_font == null) {
                return;
            }

            for(var i = 0; i < __instance._textContainerList.Count; i++) {
                var container = __instance._textContainerList[i];
                Finis.Log($"Check container text: {container.textElement.text}");
                if(container.textElement.text.Contains("<i>")) {
                    container.originalFont = _japanese_dynamic_font;
                    container.textElement.font = _japanese_dynamic_font;
                    __instance._textContainerList[i] = container;
                }
            }
        }
    }
}
