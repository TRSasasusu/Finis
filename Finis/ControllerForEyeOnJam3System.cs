using OWML.ModHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace Finis {
    public class ControllerForEyeOnJam3System {
        const string END_BH_PATH = "FinisPlateau_Body/Sector/EndBH";
        const string DESTROY_CREDIT_VOLUME_PATH = "EyeOfTheUniverse_Body/Sector/destroy_credit_volume";
        const string BIGBANG_CREDIT_VOLUME_PATH = "EyeOfTheUniverse_Body/Sector/bigbang_credit_volume";
        const string PLAYER_SPAWN_POINT_PATH = "EyeOfTheUniverse_Body/PlayerSpawnPoint";
        const string QUANTUM_FOAM_PATH = "EyeOfTheUniverse_Body/Sector/SixthPlanet_Root/Effects_SixthPlanet/Sector_QuantumFoamLayer";
        const string EYE_PATH = "EyeOfTheUniverse_Body";
        const string SHELLING_PATH = "Shelling_Body";

        static readonly (string, string)[] OTHER_BODIES = new (string, string)[] {
            ("Jam3Sun_Body", null),
            ("Jam 3 Sun_Proxy", null),
            ("StarshipCommunity_Body", null),
            ("FinisPlateau_Body", null),
            ("RedMoon_Body", null),
            ("FinisPlateauCore_Body", null),
            ("Gravelrock_Body", "xen.UnofficialJam3Entry"),
            ("GravelrockCommunicationSatellite_Body", "xen.UnofficialJam3Entry"),
            ("TheBoiledEgg_Body", "Echatsum.CallisThesis"),
            ("Aicale_Body", "MegaPiggy.Axiom"),
            ("Axiom_Body", "MegaPiggy.Axiom"),
            ("BrokenSatellite_Body", "MegaPiggy.Axiom"),
            ("ALTTH_Body", "CrypticBird.Jam3"),
            ("ModJamHub_Body", "coderCleric.JamHub"),
            ("EggStar_Body", "Hawkbar.SolarRangers"),
            ("EchoHike_Body", "Trifid.TrifidJam3"),
            ("ForlornProject_Body", "TeamErnesto.OWJam3ModProject"),
            ("FracturedHarmony_Body", "pikpik_carrot.BandTogether"),
            ("MAGISTARIUM_Body", "GameWyrm.HearthsNeighbor2"),
        };

        GameObject _destroyCreditVolume;
        GameObject _quantumForm;
        GameObject _eye;
        GameObject _shelling;
        GameObject _playerSpawnPoint;
        Dictionary<string, GameObject> _otherBodies;

        Coroutine _initializeBody;
        Coroutine _creditDestroyCoroutine;
        Coroutine _setPlayerPos;

        public void Initialize() {
            _initializeBody = Finis.Instance.StartCoroutine(InitializeBody());
        }

        IEnumerator InitializeBody() {
            while(true) {
                _destroyCreditVolume = GameObject.Find(DESTROY_CREDIT_VOLUME_PATH);
                if(_destroyCreditVolume) {
                    _destroyCreditVolume.SetActive(false);
                    break;
                }
                yield return null;
            }

            while (true) {
                _playerSpawnPoint = GameObject.Find(PLAYER_SPAWN_POINT_PATH);
                if(_playerSpawnPoint) {
                    break;
                }
                yield return null;
            }

            while (true) {
                _quantumForm = GameObject.Find(QUANTUM_FOAM_PATH);
                if (_quantumForm) {
                    _quantumForm.SetActive(false);
                    break;
                }
                yield return null;
            }

            while (true) {
                var bigbang = GameObject.Find(BIGBANG_CREDIT_VOLUME_PATH);
                if (bigbang) {
                    var enableQuantumFoam = new GameObject("EnableQuantumFoam");
                    enableQuantumFoam.transform.parent = bigbang.transform.parent;
                    enableQuantumFoam.transform.localPosition = bigbang.transform.localPosition + new Vector3(0, 200, 0);
                    enableQuantumFoam.transform.localRotation = bigbang.transform.localRotation;
                    enableQuantumFoam.transform.localScale = bigbang.transform.localScale;
                    var collider = enableQuantumFoam.AddComponent<BoxCollider>();
                    collider.isTrigger = true;
                    collider.size = new Vector3(8000, 400, 8000);
                    enableQuantumFoam.AddComponent<EnableQuantumFoam>()._controller = this;
                    break;
                }
                yield return null;
            }

            while (true) {
                _eye = GameObject.Find(EYE_PATH);
                if(_eye) {
                    _eye.SetActive(false);
                    break;
                }
                yield return null;
            }

            while (true) {
                _shelling = GameObject.Find(SHELLING_PATH);
                if (_shelling) {
                    _shelling.SetActive(false);
                    break;
                }
                yield return null;
            }

            while (true) {
                GameObject endBH = GameObject.Find(END_BH_PATH);
                if (endBH) {
                    var destructionVolume = endBH.transform.Find("DestructionVolume");
                    var endBHTrigger = new GameObject("EndBHTrigger");
                    endBHTrigger.transform.parent = endBH.transform;
                    endBHTrigger.transform.localPosition = Vector3.zero;
                    endBHTrigger.transform.localEulerAngles = Vector3.zero;
                    endBHTrigger.transform.localScale = Vector3.one;
                    endBHTrigger.layer = destructionVolume.gameObject.layer;
                    var collider = endBHTrigger.AddComponent<SphereCollider>();
                    collider.radius = destructionVolume.GetComponent<SphereCollider>().radius;
                    collider.isTrigger = true;
                    endBHTrigger.AddComponent<EndBHTrigger>()._controller = this;
                    destructionVolume.gameObject.SetActive(false);
                    break;
                }
                yield return null;
            }

            Finis.Log("most of final setup is done.");
            _otherBodies = new Dictionary<string, GameObject>();
            while(true) {
                var hasRemain = false;
                foreach((var path, var mod) in OTHER_BODIES) {
                    if(mod != null && !Finis.ModExist(mod)) {
                        continue;
                    }
                    if (_otherBodies.ContainsKey(path)) {
                        continue;
                    }
                    var obj = GameObject.Find(path);
                    if(obj) {
                        _otherBodies.Add(path, obj);
                        continue;
                    }
                    hasRemain = true;
                }
                if(!hasRemain) {
                    break;
                }
                yield return null;
            }
            Finis.Log("all final setup is done.");
        }

        public void StartEye() {
            Vector3 center = Vector3.zero;
            foreach ((var path, var obj) in _otherBodies) {
                if(obj) {
                    if(path == "Jam3Sun_Body") {
                        center = obj.transform.position;
                        foreach(Transform child in obj.transform) {
                            child.gameObject.SetActive(false);
                        }
                    }
                    else {
                        obj.SetActive(false);
                    }
                }
            }

            _eye.SetActive(true);
            _eye.transform.position = center;//Vector3.zero;
            _eye.transform.rotation = Quaternion.identity;
            _shelling.SetActive(true);
            _shelling.transform.position = center + new Vector3(0, 0, 1000);
            _shelling.transform.rotation = Quaternion .identity;
            _shelling.transform.Find("Sector/Star/PlanetDestructionVolume").GetComponent<DestructionVolume>()._onlyAffectsPlayerAndShip = true; // to avoid remove the Eye
            _creditDestroyCoroutine = Finis.Instance.StartCoroutine(DestroyCreditBody());

            if(PlayerState.IsInsideShip()) {
                EscapeFromShip();
            }

            _setPlayerPos = Finis.Instance.StartCoroutine(SetPlayerPos());

            Locator.GetGlobalMusicController().gameObject.SetActive(false);
        }

        void EscapeFromShip() {
            var shipBody = Locator.GetShipBody();
            shipBody.GetComponentInChildren<ShipDamageController>().ToggleInvincibility();
            foreach(var volume in shipBody.GetComponentsInChildren<OWTriggerVolume>()) {
                volume.RemoveObjectFromVolume(Locator.GetPlayerDetector());
                volume.RemoveObjectFromVolume(Locator.GetPlayerCamera().GetComponentInChildren<FluidDetector>().gameObject);
            }
            if(PlayerState.AtFlightConsole()) {
                var cockpit = shipBody.GetComponentInChildren<ShipCockpitController>();
                cockpit.ExitFlightConsole();
                cockpit.CompleteExitFlightConsole();
            }
            shipBody.GetComponentInChildren<HatchController>().OpenHatch();
        }

        public void EnableQuantumFoam() {
            _quantumForm.SetActive(true);
        }

        public void DestroyResources() {
            if (_initializeBody != null) {
                Finis.Instance.StopCoroutine(_initializeBody);
                _initializeBody = null;
            }
            if(_creditDestroyCoroutine != null) {
                Finis.Instance.StopCoroutine(_creditDestroyCoroutine);
                _creditDestroyCoroutine = null;
            }
            if (_setPlayerPos != null) {
                Finis.Instance.StopCoroutine(_setPlayerPos);
                _setPlayerPos = null;
            }
        }

        IEnumerator DestroyCreditBody() {
            Finis.Log("waiting destroy credit");
            yield return new WaitForSeconds(80);
            _destroyCreditVolume.SetActive(true);
            _destroyCreditVolume.GetComponent<SphereShape>().radius = 1000000;
            Finis.Log("destroy credit done");
        }

        IEnumerator SetPlayerPos() {
            var shipBody = Locator.GetShipBody();
            for(int i = 0; i < 5; ++i) {
                var playerBody = Locator.GetPlayerBody();
                playerBody.WarpToPositionRotation(_playerSpawnPoint.transform.position, _playerSpawnPoint.transform.rotation);
                playerBody.SetVelocity(PointVelocity(_playerSpawnPoint.transform));
                var shipSpawnPos = _playerSpawnPoint.transform.position + new Vector3(-11.1367f - 0.5f, 213.3824f - 199.3824f, 54.2352f - 35.606f);
                if(shipBody) {
                    shipBody.transform.position = shipSpawnPos;
                }
                yield return null;
                yield return new WaitForSeconds(0.2f);
            }
            if(shipBody) {
                shipBody.GetComponentInChildren<ShipTractorBeamSwitch>().ActivateTractorBeam();
            }
        }

        static Vector3 PointVelocity(Transform point) {
            var parentOWRigidbody = point.GetComponentInParent<OWRigidbody>();
            return parentOWRigidbody.GetVelocity() + parentOWRigidbody.GetPointTangentialVelocity(point.position);
        }
    }
}
