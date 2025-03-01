﻿using OWML.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace Finis {
    public class ControllerForEye {
        Coroutine _cloneShipBodyCoroutine;
        Coroutine _spawnShipBodyCoroutine;
        Coroutine _creditDestroyCoroutine;
        Coroutine _creditBigbangCoroutine;
        Coroutine _fixEyeWhenBringingItemCoroutine;
        public GameObject ClonedShip { get; private set; }

        public void DestroyResources() {
            if(_cloneShipBodyCoroutine != null) {
                Finis.Instance.StopCoroutine(_cloneShipBodyCoroutine);
                _cloneShipBodyCoroutine = null;
            }
            if(_spawnShipBodyCoroutine != null) {
                Finis.Instance.StopCoroutine(_spawnShipBodyCoroutine);
                _spawnShipBodyCoroutine = null;
            }
            if(_creditDestroyCoroutine != null) {
                Finis.Instance.StopCoroutine(_creditDestroyCoroutine);
                _creditDestroyCoroutine = null;
            }
            if(_creditBigbangCoroutine != null) {
                Finis.Instance.StopCoroutine(_creditBigbangCoroutine);
                _creditBigbangCoroutine = null;
            }
            if(_fixEyeWhenBringingItemCoroutine != null) {
                Finis.Instance.StopCoroutine(_fixEyeWhenBringingItemCoroutine);
                _fixEyeWhenBringingItemCoroutine = null;
            }
            GameObject.Destroy(ClonedShip);
        }

        public void CloneShip() {
            _cloneShipBodyCoroutine = Finis.Instance.StartCoroutine(CloneShipBody());
        }

        IEnumerator CloneShipBody() {
            GameObject originalShip;
            while (true) {
                yield return null;
                originalShip = GameObject.FindWithTag("Ship");
                if(originalShip) {
                    break;
                }
            }

            var lights = new GameObject[] {
                originalShip.transform.Find("Module_Cockpit/Lights_Cockpit/Pointlight_HEA_ShipCockpit").gameObject,
                originalShip.transform.Find("Module_Cabin/Lights_Cabin/Pointlight_HEA_ShipCabin").gameObject,
                originalShip.transform.Find("Module_Supplies/Lights_Supplies/Pointlight_HEA_ShipSupplies_Top").gameObject,
            };
            foreach (var light in lights) {
                light.gameObject.SetActive(false);
            }

            ClonedShip = GameObject.Instantiate(originalShip);
            ClonedShip.SetActive(false);
            ClonedShip.name = originalShip.name;
            GameObject.DontDestroyOnLoad(ClonedShip);

            foreach (var light in lights) {
                light.gameObject.SetActive(true);
            }
            //yield return null;
            //originalShip.transform.Find("Module_Cockpit/Lights_Cockpit/Pointlight_HEA_ShipCockpit").GetComponent<LightmapController>().Awake();
            //originalShip.transform.Find("Module_Cabin/Lights_Cabin/Pointlight_HEA_ShipCabin").GetComponent<LightmapController>().Awake();
            //originalShip.transform.Find("Module_Supplies/Lights_Supplies/Pointlight_HEA_ShipSupplies_Top").GetComponent<LightmapController>().Awake();
        }

        public void SpawnShip() {
            _spawnShipBodyCoroutine = Finis.Instance.StartCoroutine(SpawnShipBody());
            _creditDestroyCoroutine = Finis.Instance.StartCoroutine(DestroyCreditBody());
            _creditBigbangCoroutine = Finis.Instance.StartCoroutine(BigbangCreditBody());
            _fixEyeWhenBringingItemCoroutine = Finis.Instance.StartCoroutine(FixEyeWhenBringingItemBody());
        }

        IEnumerator FixEyeWhenBringingItemBody() {
            GameObject eye;
            while (true) {
                eye = GameObject.Find("EyeOfTheUniverse_Body");
                if(eye) {
                    break;
                }
                yield return null;
            }
            while(true) {
                yield return null;
                if(!eye.activeSelf) {
                    eye.SetActive(true);
                    break;
                }
            }

            GameObject star;
            while(true) {
                var shelling = GameObject.Find("Shelling_Body");
                if(shelling) {
                    star = shelling.transform.Find("Sector/Star").gameObject;
                    break;
                }
                yield return null;
            }
            while(true) {
                yield return null;
                if(!star.activeSelf) {
                    star.SetActive(true);
                    break;
                }
            }
        }

        IEnumerator DestroyCreditBody() {
            GameObject destroyCreditVolume;
            while (true) {
                destroyCreditVolume = GameObject.Find("destroy_credit_volume");
                if(destroyCreditVolume) {
                    destroyCreditVolume.SetActive(false);
                    break;
                }
                yield return null;
            }
            yield return new WaitForSeconds(120);
            destroyCreditVolume.SetActive(true);
        }

        IEnumerator BigbangCreditBody() {
            GameObject bigbangCreditVolume;
            while (true) {
                bigbangCreditVolume = GameObject.Find("bigbang_credit_volume");
                if(bigbangCreditVolume) {
                    bigbangCreditVolume.SetActive(false);
                    break;
                }
                yield return null;
            }
            GameObject observatory;
            while (true) {
                yield return null;
                var eyeSector = GameObject.Find("EyeOfTheUniverse_Body/Sector_EyeOfTheUniverse");
                if(eyeSector) {
                    observatory = eyeSector.transform.Find("Sector_Observatory").gameObject;
                    break;
                }
            }
            while (true) {
                yield return null;
                if(observatory.activeSelf) {
                    PlayerData.SaveEyeCompletion();
                    bigbangCreditVolume.SetActive(true);
                    break;
                }
            }
        }

        IEnumerator SpawnShipBody() {
            GameObject playerSpawnPoint;
            Finis.Log("start: finding playerSpawnPoint");
            while (true) {
                yield return null;
                var dummyBody = GameObject.Find("Dummy_Body");
                if(dummyBody) {
                    playerSpawnPoint = dummyBody.transform.Find("PlayerSpawnPoint").gameObject;
                    if(playerSpawnPoint) {
                        break;
                    }
                }
            }
            Finis.Log("end: finding playerSpawnPoint");

            var spawnPos = playerSpawnPoint.transform.position + new Vector3(-11.1367f - 0.5f, 213.3824f - 199.3824f, 54.2352f - 35.606f);
            var spawnedShip = GameObject.Instantiate(ClonedShip);
            spawnedShip.transform.position = spawnPos;
            spawnedShip.name = ClonedShip.name;
            spawnedShip.SetActive(true);

            Locator._shipBody = spawnedShip.GetAttachedOWRigidbody();
            Locator._shipDetector = spawnedShip.transform.Find("ShipDetector").gameObject;
            Locator._shipTransform = spawnedShip.transform;

            yield return null;

            var cockpitAttachPoint = spawnedShip.transform.Find("Module_Cockpit/Systems_Cockpit/CockpitAttachPoint");
            cockpitAttachPoint.GetComponent<PlayerAttachPoint>().Start();

            while(true) {
                yield return null;
                if(!spawnedShip.activeSelf) {
                    spawnedShip.SetActive(true);
                    break;
                }
            }

            Transform tractorBeamFluid;
            while(true) {
                yield return null;
                tractorBeamFluid = spawnedShip.transform.Find("Module_Cabin/Systems_Cabin/Hatch/TractorBeam/BeamVolume");
                if(tractorBeamFluid) {
                    tractorBeamFluid.GetComponent<TractorBeamFluid>().SetVolumeActivation(false);
                    yield return null;
                    tractorBeamFluid.GetComponent<TractorBeamFluid>().SetVolumeActivation(true);
                    break;
                }
            }

            var consoleDisplay = spawnedShip.transform.Find("Module_Cockpit/Systems_Cockpit/ShipCockpitUI/CockpitCanvases/ShipWorldSpaceUI/ConsoleDisplay");
            try {
                consoleDisplay.GetComponent<ShipNotificationDisplay>().Start();
            }
            catch (Exception ex) { }

            var vessel = GameObject.Find("Vessel_Body");
            if(vessel) {
                vessel.SetActive(false);
            }
        }
    }
}
