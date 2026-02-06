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
        const string DESTROY_CREADIT_VOLUME_PATH = "EyeOfTheUniverse_Body/Sector/destroy_credit_volume";
        const string PLAYER_SPAWN_POINT_PATH = "EyeOfTheUniverse_Body/PlayerSpawnPoint";
        const string EYE_PATH = "EyeOfTheUniverse_Body";
        const string SHELLING_PATH = "Shelling_Body";

        GameObject _destroyCreditVolume;
        GameObject _eye;
        GameObject _shelling;
        GameObject _playerSpawnPoint;

        Coroutine _initializeBody;
        Coroutine _creditDestroyCoroutine;

        public void Initialize() {
            _initializeBody = Finis.Instance.StartCoroutine(InitializeBody());
        }

        IEnumerator InitializeBody() {
            while(true) {
                _destroyCreditVolume = GameObject.Find(DESTROY_CREADIT_VOLUME_PATH);
                if(_destroyCreditVolume) {
                    _destroyCreditVolume.SetActive(false);
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

            while (true) {
                _playerSpawnPoint = GameObject.Find(PLAYER_SPAWN_POINT_PATH);
                if(_playerSpawnPoint) {
                    break;
                }
                yield return null;
            }
        }

        public void StartEye() {
            _eye.SetActive(true);
            _shelling.SetActive(true);
            _creditDestroyCoroutine = Finis.Instance.StartCoroutine(DestroyCreditBody());

            if(PlayerState.IsInsideShip()) {
                EscapeFromShip();
            }

            var playerBody = Locator.GetPlayerBody();
            playerBody.WarpToPositionRotation(_playerSpawnPoint.transform.position, _playerSpawnPoint.transform.rotation);
            playerBody.SetVelocity(PointVelocity(_playerSpawnPoint.transform));
            var shipSpawnPos = _playerSpawnPoint.transform.position + new Vector3(-11.1367f - 0.5f, 213.3824f - 199.3824f, 54.2352f - 35.606f);
            var shipBody = Locator.GetShipBody();
            if(shipBody) {
                shipBody.transform.position = shipSpawnPos;
            }
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

        public void DestroyResources() {
            if (_initializeBody != null) {
                Finis.Instance.StopCoroutine(_initializeBody);
                _initializeBody = null;
            }
            if(_creditDestroyCoroutine != null) {
                Finis.Instance.StopCoroutine(_creditDestroyCoroutine);
                _creditDestroyCoroutine = null;
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

        static Vector3 PointVelocity(Transform point) {
            var parentOWRigidbody = point.GetComponentInParent<OWRigidbody>();
            return parentOWRigidbody.GetVelocity() + parentOWRigidbody.GetPointTangentialVelocity(point.position);
        }
    }
}
