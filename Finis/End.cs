using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using IEnumerator = System.Collections.IEnumerator;

namespace Finis {
    public class End {
        GameObject _cannonBlack;
        GameObject _cannonYellow;
        GameObject _bhPos;
        GameObject _unbrokenGround;
        GameObject _brokenGround;
        List<Transform> _brokenObjs;
        GameObject _core;
        Light _pointLight;
        GameObject _endMusic;
        bool _startEnd;

        public End() {
            StateController.Instance.EndVolume.SetActive(false);
            StateController.Instance.EndBH.SetActive(false);

            _brokenObjs = new List<Transform>();
            foreach (var child in StateController.Instance.FinisPlateauSector.GetComponentsInChildren<Transform>(true)) {
                if(child.name == "green_crystal_cannon") {
                    _cannonBlack = child.transform.Find("black").gameObject;
                    _cannonYellow = child.transform.Find("yellow").gameObject;
                    _pointLight = _cannonYellow.GetComponentInChildren<Light>(true);
                }
                else if(child.name == "BHPosition") {
                    _bhPos = child.gameObject;
                }
                else if(child.name == "PrevBrokenGround") {
                    _unbrokenGround = child.gameObject;
                }
                else if(child.name == "BrokenGround") {
                    _brokenGround = child.gameObject;
                    _brokenObjs.AddRange(child.GetComponentsInChildren<Transform>().Skip(1));
                }
                else if(child.name == "wasteland") {
                    _brokenObjs.AddRange(child.GetComponentsInChildren<Transform>().Skip(1));
                }
                else if(child.name == "green_crystal_core") {
                    _core = child.gameObject;
                }
                else if(child.name == "finis_end_music") {
                    _endMusic = child.gameObject;
                }
            }
            _endMusic.SetActive(false);
        }

        public void StartEnd() {
            if(_startEnd) {
                return;
            }
            _startEnd = true;
            Finis.Instance.StartCoroutine(StartEndBody());
        }

        IEnumerator StartEndBody() {
            _cannonBlack.SetActive(false);
            _cannonYellow.SetActive(true);
            _endMusic.SetActive(true);

            foreach(var collider in _core.GetComponentsInChildren<Collider>()) {
                collider.enabled = false;
            }

            var t = 0f;
            var maxTime = 5;
            while(true) {
                yield return null;
                t += Time.deltaTime;
                _pointLight.range = Mathf.Lerp(_pointLight.range, 200, t > maxTime ? 1 : t / maxTime);
                _core.transform.localPosition = Utils.Lerp(_core.transform.localPosition, _bhPos.transform.localPosition, t > maxTime ? 1 : t / maxTime);
                if(t > maxTime) {
                    break;
                }
            }

            _unbrokenGround.SetActive(false);
            _brokenGround.SetActive(true);
            //foreach(var obj in _brokenGround.GetComponentsInChildren<Transform>().Skip(1)) {
            foreach(var obj in _brokenObjs) {
                obj.parent = _bhPos.transform.parent;
            }
            t = 0f;
            while(true) {
                yield return null;
                t += Time.deltaTime;
                foreach(var obj in  _brokenObjs) {
                    obj.localPosition = Utils.Lerp(obj.localPosition, _bhPos.transform.localPosition, t > maxTime ? 1 : t / maxTime);
                }
                if(t > maxTime) {
                    break;
                }
            }

            StateController.Instance.EndVolume.SetActive(true);
            StateController.Instance.EndBH.SetActive(true);
        }
    }
}
