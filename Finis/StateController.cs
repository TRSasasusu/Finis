using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace Finis {
    public class StateController {
        public static StateController Instance;

        public RodItem PickedUpRod { get; private set; }
        public Transform RodSocket { get; private set; }

        public GameObject FinisPlateauSector { get; private set; }
        //public GameObject EndVolume { get; private set; }
        public GameObject EndBH { get; private set; }
        public GameObject FinisMoonSector { get; private set; }
        public GameObject PlayerBody { get; private set; }

        List<Renderer> _greenRenderers;
        List<GameObject> _weakredObjs;
        List<GameObject> _weakblueObjs;
        GameObject _finisPalaceFactReveal;
        End _end;
        Coroutine _runAfterExpandingRod;

        readonly string[] ADDITIONAL_RED_OBJECT_PATHS = new string[] {
            "RedMoon_Body/Sector/Quantum States - moonGMGroup",
            "RedMoon_Body/Sector/Quantum States - moonQuantumGroup",
            "RedMoon_Body/Sector/AudioVolume",
        };

        public StateController() {
            Instance = this;
        }

        public void PickUpRod(RodItem rod) {
            if(_runAfterExpandingRod != null) {
                Finis.Instance.StopCoroutine(_runAfterExpandingRod);
                _runAfterExpandingRod = null;
            }

            PickedUpRod = rod;
            if(rod.Color == RodItem.State.BLUE) {
                rod.ChangeToBlue();
                _runAfterExpandingRod = Finis.Instance.StartCoroutine(RunAfterExpandingRod(() => {
                    EnableRed(false);
                    EnableBlue(true);
                    EnableGreen(true);
                }));
            }
            else {
                rod.ChangeToRed();
                _runAfterExpandingRod = Finis.Instance.StartCoroutine(RunAfterExpandingRod(() => {
                    EnableRed(true);
                    EnableBlue(false);
                    EnableGreen(true);
                }));
            }
        }

        public void DropRod() {
            if(_runAfterExpandingRod != null) {
                Finis.Instance.StopCoroutine(_runAfterExpandingRod);
                _runAfterExpandingRod = null;
            }

            PickedUpRod = null;
            EnableGreen(false);
            EnableRed(false);
            EnableBlue(true);
        }

        public void CollisionRed() {
            if(PickedUpRod == null || PickedUpRod.Color == RodItem.State.RED) {
                return;
            }
            if(_runAfterExpandingRod != null) {
                Finis.Instance.StopCoroutine(_runAfterExpandingRod);
                _runAfterExpandingRod = null;
            }

            PickedUpRod.ChangeToRed();
            _runAfterExpandingRod = Finis.Instance.StartCoroutine(RunAfterExpandingRod(() => {
                EnableRed(true);
                EnableBlue(false);
            }));
        }

        public void CollisionBlue() {
            if(PickedUpRod == null || PickedUpRod.Color == RodItem.State.BLUE) {
                return;
            }
            if(_runAfterExpandingRod != null) {
                Finis.Instance.StopCoroutine(_runAfterExpandingRod);
                _runAfterExpandingRod = null;
            }

            PickedUpRod.ChangeToBlue();
            _runAfterExpandingRod = Finis.Instance.StartCoroutine(RunAfterExpandingRod(() => {
                EnableRed(false);
                EnableBlue(true);
            }));
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

        IEnumerator RunAfterExpandingRod(Action action) {
            yield return new WaitForSeconds(0.65f);
            action();
        }

        IEnumerator InitializeBody() {
            Finis.Log("Start InitializeBody");

            GameObject defaultItemSocket;
            while (true) {
                yield return null;
                //defaultItemSocket = GameObject.Find("Player_Body/PlayerCamera/ItemCarryTool/ItemSocket");
                defaultItemSocket = Locator.GetToolModeSwapper().GetItemCarryTool().transform.Find("ItemSocket").gameObject;
                if (defaultItemSocket) {
                    break;
                }
            }
            RodSocket = GameObject.Instantiate(defaultItemSocket).transform;
            RodSocket.gameObject.name = "RodSocket";
            RodSocket.parent = defaultItemSocket.transform.parent;
            RodSocket.localPosition = new Vector3(0.405f, -0.33f, 0.45f);
            RodSocket.localEulerAngles = new Vector3(26.1645f, 338.5462f, 328.3699f);
            Finis.Log("Set RodSocket");

            while(true) {
                yield return null;
                PlayerBody = GameObject.Find("Player_Body");
                if(PlayerBody) {
                    PlayerBody.AddComponent<HighCrystalDetector>();
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
                //if(Finis.newHorizons.GetCurrentStarSystem() != "Jam3") {
                //    Finis.Log("StateController is starting even in not Jam3", OWML.Common.MessageType.Warning);
                //    yield break;
                //}
            }
            while (true) {
                yield return null;
                FinisMoonSector = GameObject.Find("RedMoon_Body/Sector/");
                if(FinisMoonSector) {
                    break;
                }
            }
            Finis.Log("Found sectors");
            if(Finis.Instance._stateController != this) {
                Finis.Log("Multiple StateController is making now!", OWML.Common.MessageType.Warning);
                yield break;
            }

            //while(true) {
            //    yield return null;
            //    EndVolume = GameObject.Find("FinisPlateau_Body/Sector/EndVolume");
            //    if(EndVolume) {
            //        break;
            //    }
            //}
            while(true) {
                yield return null;
                EndBH = GameObject.Find("FinisPlateau_Body/Sector/EndBH");
                if(EndBH) {
                    break;
                }
            }
            Finis.Log("Found sectors");
            if(Finis.Instance._stateController != this) {
                Finis.Log("Multiple StateController is making now! (2)", OWML.Common.MessageType.Warning);
                yield break;
            }

            _greenRenderers = new List<Renderer>();
            _weakredObjs = new List<GameObject>();
            _weakblueObjs = new List<GameObject>();
            foreach(var child in FinisPlateauSector.GetComponentsInChildren<Transform>()) {
                if(child.name == "Rod") {
                    var rodItem = child.gameObject.AddComponent<RodItem>();
                    rodItem._localDropOffset = new Vector3(0, 0.73f, 0);
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
            foreach(var child in FinisMoonSector.GetComponentsInChildren<Transform>()) {
                if(child.name.Contains("weakred_crystal")) {
                    _weakredObjs.Add(child.gameObject);
                }
            }
            foreach(var path in ADDITIONAL_RED_OBJECT_PATHS) {
                while(true) {
                    var obj = GameObject.Find(path);
                    if(obj) {
                        _weakredObjs.Add(obj);
                        break;
                    }
                    yield return null;
                }
            }

            EnableGreen(false);
            EnableRed(false);
            EnableBlue(true);

            _end = new End();

            Finis.Log("Correctly set colors and end");

            while(true) {
                yield return null;
                var eyeMeter = GameObject.Find("RedMoon_Body/Sector/finis_red_moon/weakred_crystal_moon/eyemeter");
                if(eyeMeter) {
                    eyeMeter.AddComponent<EyeMeter>();
                    break;
                }
            }
            Finis.Log("Set EyeMeter");
        }
    }
}
