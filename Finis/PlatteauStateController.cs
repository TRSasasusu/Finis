using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace Finis {
    public class PlatteauStateController {
        public static PlatteauStateController Instance;

        public RodItem PickedUpRod { get; private set; }
        public Transform RodSocket { get; private set; }

        public GameObject FinisPlateauSector { get; private set; }
        public GameObject EndVolume { get; private set; }
        public GameObject EndBH { get; private set; }

        List<Renderer> _greenRenderers;
        List<GameObject> _weakredObjs;
        List<GameObject> _weakblueObjs;
        GameObject _finisPalaceFactReveal;
        End _end;

        public PlatteauStateController() {
            Instance = this;
        }

        public void PickUpRod(RodItem rod) {
            PickedUpRod = rod;
            if(rod.Color == RodItem.State.BLUE) {
                rod.ChangeToBlue();
                EnableRed(false);
                EnableBlue(true);
            }
            else {
                rod.ChangeToRed();
                EnableRed(true);
                EnableBlue(false);
            }
            EnableGreen(true);
        }

        public void DropRod() {
            PickedUpRod = null;
            EnableGreen(false);
            EnableRed(false);
            EnableBlue(true);
        }

        public void CollisionRed() {
            if(PickedUpRod == null || PickedUpRod.Color == RodItem.State.RED) {
                return;
            }
            PickedUpRod.ChangeToRed();
            EnableRed(true);
            EnableBlue(false);
        }

        public void CollisionBlue() {
            if(PickedUpRod == null || PickedUpRod.Color == RodItem.State.BLUE) {
                return;
            }
            PickedUpRod.ChangeToBlue();
            EnableRed(false);
            EnableBlue(true);
        }

        public void EnableGreen(bool enabled) {
            foreach(var renderer in  _greenRenderers) {
                renderer.enabled = enabled;
            }
            _finisPalaceFactReveal.SetActive(enabled);
        }

        public void EnableRed(bool enabled) {
            foreach(var obj in _weakredObjs) {
                obj.SetActive(enabled);
            }
        }

        public void EnableBlue(bool enabled) {
            foreach(var obj in _weakblueObjs) {
                obj.SetActive(enabled);
            }
        }

        public void StartEnd() {
            _end.StartEnd();
        }

        public void Initialize() {
            Finis.Instance.StartCoroutine(InitializeBody());
        }

        IEnumerator InitializeBody() {
            Finis.Log("Start InitializeBody");

            GameObject defaultItemSocket;
            while (true) {
                yield return null;
                defaultItemSocket = GameObject.Find("Player_Body/PlayerCamera/ItemCarryTool/ItemSocket");
                if (defaultItemSocket) {
                    break;
                }
            }
            RodSocket = GameObject.Instantiate(defaultItemSocket).transform;
            RodSocket.gameObject.name = "RodSocket";
            RodSocket.parent = defaultItemSocket.transform.parent;
            RodSocket.localPosition = new Vector3(0.405f, -0.33f, 0.45f);
            RodSocket.localEulerAngles = new Vector3(0, 340, 25);
            Finis.Log("Set RodSocket");

            while(true) {
                yield return null;
                var playerBody = GameObject.Find("Player_Body");
                if(playerBody) {
                    playerBody.AddComponent<HighCrystalDetector>();
                    break;
                }
            }
            Finis.Log("Set HighCrystalDetector");

            while (true) {
                yield return null;
                FinisPlateauSector = GameObject.Find("FinisPlateau_Body/Sector");
                if(FinisPlateauSector) {
                    break;
                }
            }
            Finis.Log("Found our sector");

            while(true) {
                yield return null;
                EndVolume = GameObject.Find("FinisPlateau_Body/Sector/EndVolume");
                if(EndVolume) {
                    break;
                }
            }
            while(true) {
                yield return null;
                EndBH = GameObject.Find("FinisPlateau_Body/Sector/EndBH");
                if(EndBH) {
                    break;
                }
            }

            _greenRenderers = new List<Renderer>();
            _weakredObjs = new List<GameObject>();
            _weakblueObjs = new List<GameObject>();
            foreach(var child in FinisPlateauSector.GetComponentsInChildren<Transform>()) {
                if(child.name == "Rod") {
                    var rodItem = child.gameObject.AddComponent<RodItem>();
                    rodItem._localDropOffset = new Vector3(0, 1, 0);
                    child.gameObject.AddComponent<OWCollider>();
                    Finis.Log("Set RodItem");
                }
                //else if(child.name.Contains("highred_crystal")) {
                //    foreach(var collider in child.GetComponentsInChildren<Collider>()) {
                //        collider.gameObject.AddComponent<HighCrystalDetector>().Color = RodItem.State.RED;
                //    }
                //    //child.gameObject.AddComponent<HighCrystal>().Color = RodItem.State.RED;
                //}
                //else if(child.name.Contains("highblue_crystal")) {
                //    foreach(var collider in child.GetComponentsInChildren<Collider>()) {
                //        collider.gameObject.AddComponent<HighCrystalDetector>().Color = RodItem.State.BLUE;
                //    }
                //}
                else if(child.name.Contains("weakred_crystal")) {
                    _weakredObjs.Add(child.gameObject);
                }
                else if(child.name.Contains("weakblue_crystal")) {
                    _weakblueObjs.Add(child.gameObject);
                }
                else if(child.name.Contains("green_crystal")) {
                    _greenRenderers.AddRange(child.GetComponentsInChildren<Renderer>());
                }
                else if(child.name == "finis_palace_fact_reveal") {
                    _finisPalaceFactReveal = child.gameObject;
                }
            }

            EnableGreen(false);
            EnableRed(false);
            EnableBlue(true);

            _end = new End();
        }
    }
}
