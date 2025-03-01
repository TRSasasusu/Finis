﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Finis {
    // Copied from https://github.com/xen-42/outer-wilds-eye-warp/blob/master/EyeShipWarp/Patches.cs
    [HarmonyPatch]
    public static class EyePatch {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipThrusterController), nameof(ShipThrusterController.ReadTranslationalInput))]
        public static bool ShipThrusterController_ReadTranslationalInput(ShipThrusterController __instance, ref Vector3 __result) {
            if (Finis.newHorizons.GetCurrentStarSystem() != "EyeOfTheUniverse") return true;

            float value = OWInput.GetValue(InputLibrary.thrustX, InputMode.All);
            float value2 = OWInput.GetValue(InputLibrary.thrustZ, InputMode.All);
            float value3 = OWInput.GetValue(InputLibrary.thrustUp, InputMode.All);
            float value4 = OWInput.GetValue(InputLibrary.thrustDown, InputMode.All);
            if (!OWInput.IsInputMode(InputMode.ShipCockpit | InputMode.LandingCam)) {
                __result = Vector3.zero;
                return false;
            }
            if (!__instance._shipResources.AreThrustersUsable()) {
                __result = Vector3.zero;
                return false;
            }
            if (__instance._autopilot.IsFlyingToDestination()) {
                __result = Vector3.zero;
                return false;
            }
            Vector3 vector = new Vector3(value, 0f, value2);
            if (vector.sqrMagnitude > 1f) {
                vector.Normalize();
            }
            vector.y = value3 - value4;
            if (__instance._requireIgnition && __instance._landingManager.IsLanded()) {
                vector.x = 0f;
                vector.z = 0f;
                vector.y = Mathf.Clamp01(vector.y);
                if (!__instance._isIgniting && __instance._lastTranslationalInput.y <= 0f && vector.y > 0f) {
                    __instance._isIgniting = true;
                    __instance._ignitionTime = Time.time;
                    GlobalMessenger.FireEvent("StartShipIgnition");
                }
                if (__instance._isIgniting) {
                    if (vector.y <= 0f) {
                        __instance._isIgniting = false;
                        GlobalMessenger.FireEvent("CancelShipIgnition");
                    }
                    if (Time.time < __instance._ignitionTime + __instance._ignitionDuration) {
                        vector.y = 0f;
                    }
                    else {
                        __instance._isIgniting = false;
                        __instance._requireIgnition = false;
                        GlobalMessenger.FireEvent("CompleteShipIgnition");
                        RumbleManager.PlayShipIgnition();
                    }
                }
            }
            float d = __instance._thrusterModel.GetMaxTranslationalThrust() / __instance._thrusterModel.GetMaxTranslationalThrust();
            Vector3 vector2 = vector * d;
            if (__instance._limitOrbitSpeed && vector2.magnitude > 0f) {
                Vector3 vector3 = __instance._landingRF.GetOWRigidBody().GetWorldCenterOfMass() - __instance._shipBody.GetWorldCenterOfMass();
                Vector3 vector4 = __instance._shipBody.GetVelocity() - __instance._landingRF.GetVelocity();
                Vector3 vector5 = vector4 - Vector3.Project(vector4, vector3);
                Vector3 vector6 = Quaternion.FromToRotation(-__instance._shipBody.transform.up, vector3) * __instance._shipBody.transform.TransformDirection(vector2 * __instance._thrusterModel.GetMaxTranslationalThrust());
                Vector3 vector7 = Vector3.Project(vector6, vector3);
                Vector3 vector8 = vector6 - vector7;
                Vector3 a = vector5 + vector8 * Time.deltaTime;
                float magnitude = a.magnitude;
                float orbitSpeed = __instance._landingRF.GetOrbitSpeed(vector3.magnitude);
                if (magnitude > orbitSpeed) {
                    a = a.normalized * orbitSpeed;
                    vector8 = (a - vector5) / Time.deltaTime;
                    vector6 = vector7 + vector8;
                    vector2 = __instance._shipBody.transform.InverseTransformDirection(vector6 / __instance._thrusterModel.GetMaxTranslationalThrust());
                }
            }
            __instance._lastTranslationalInput = vector;
            __result = vector2;

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipGroanAudioController), nameof(ShipGroanAudioController.Update))]
        [HarmonyPatch(typeof(NotificationDisplayTextLayout), nameof(NotificationDisplayTextLayout.Update))]
        [HarmonyPatch(typeof(ShipLight), nameof(ShipLight.SetIntensityScale))] // added by me
        [HarmonyPatch(typeof(ShipCockpitUI), nameof(ShipCockpitUI.OnEnterFlightConsole))] // added by me
        [HarmonyPatch(typeof(ShipLight), nameof(ShipLight.SetIntensityScale))] // added by me
        private static bool SkipAtEye() => SceneManager.GetActiveScene().name != "EyeOfTheUniverse";

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipNotificationDisplay), nameof(ShipNotificationDisplay.Awake))]
        [HarmonyPatch(typeof(RepairReceiver), nameof(RepairReceiver.Awake))]
        private static bool DisableAtEye(MonoBehaviour __instance) {
            if(SceneManager.GetActiveScene().name != "EyeOfTheUniverse") {
                return true;
            }
            __instance.enabled = false;
            return false;
        }

        // added by me
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipCockpitUI), nameof(ShipCockpitUI.Start))]
        private static bool StartAtEye(ShipCockpitUI __instance) {
            if(SceneManager.GetActiveScene().name != "EyeOfTheUniverse") {
                return true;
            }
            __instance._playerCam = Locator.GetPlayerCamera();
            //__instance._signalscopeTool = __instance._playerCam.GetComponentInChildren<Signalscope>();
            __instance._shipAudioController = Locator.GetShipTransform().GetComponentInChildren<ShipAudioController>();
            return false;
            //__instance._cockpitLockOnGui = 
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipHUDMarker), nameof(ShipHUDMarker.OnEnterShip))]
        private static bool ShipHUDMarker_OnEnterShipAtEye(ShipHUDMarker __instance) {
            if(SceneManager.GetActiveScene().name != "EyeOfTheUniverse") {
                return true;
            }
            __instance._playerInShip = true;
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipHUDMarker), nameof(ShipHUDMarker.OnExitShip))]
        private static bool ShipHUDMarker_OnExitShipAtEye(ShipHUDMarker __instance) {
            if(SceneManager.GetActiveScene().name != "EyeOfTheUniverse") {
                return true;
            }
            __instance._playerInShip = false;
            return false;
        }
    }
}
