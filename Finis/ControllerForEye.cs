using OWML.Common;
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
        public GameObject ClonedShip {  get; private set; }

        public void DestroyResources() {
            if(_cloneShipBodyCoroutine != null) {
                Finis.Instance.StopCoroutine(_cloneShipBodyCoroutine);
                _cloneShipBodyCoroutine = null;
            }
            if(_spawnShipBodyCoroutine != null) {
                Finis.Instance.StopCoroutine(_spawnShipBodyCoroutine);
                _spawnShipBodyCoroutine = null;
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

            ClonedShip = GameObject.Instantiate(originalShip);
            ClonedShip.SetActive(false);
            ClonedShip.name = originalShip.name;
            GameObject.DontDestroyOnLoad(ClonedShip);
        }

        public void SpawnShip() {
            _spawnShipBodyCoroutine = Finis.Instance.StartCoroutine(SpawnShipBody());
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

            var spawnPos = playerSpawnPoint.transform.position + new Vector3(-11.1367f - 0.5f, 203.3824f - 199.3824f, 54.2352f - 35.606f);
            var spawnedShip = GameObject.Instantiate(ClonedShip);
            spawnedShip.transform.position = spawnPos;
            spawnedShip.name = ClonedShip.name;
            spawnedShip.SetActive(true);

            Locator._shipBody = spawnedShip.GetAttachedOWRigidbody();
            Locator._shipDetector = spawnedShip.transform.Find("ShipDetector").gameObject;
            Locator._shipTransform = spawnedShip.transform;
        }
    }
}
