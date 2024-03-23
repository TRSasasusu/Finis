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

        public RodItem RodItem { get; private set; }
        public Transform RodSocket { get; private set; }

        GameObject _finisPlateauSector;

        public StateController() {
            Instance = this;
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
            RodSocket.localPosition = new Vector3(0.325f, -0.22f, 0.4f);
            RodSocket.localEulerAngles = new Vector3(0, 340, 25);
            Finis.Log("Set RodSocket");

            while (true) {
                yield return null;
                _finisPlateauSector = GameObject.Find("FinisPlateau_Body/Sector");
                if(_finisPlateauSector) {
                    break;
                }
            }
            Finis.Log("Found our sector");

            foreach(var child in _finisPlateauSector.GetComponentsInChildren<Transform>()) {
                if(child.name == "Rod") {
                    var rodItem = child.gameObject.AddComponent<RodItem>();
                    rodItem._localDropOffset = new Vector3(0, 1, 0);
                    child.gameObject.AddComponent<OWCollider>();
                    Finis.Log("Set RodItem");
                }
            }
        }
    }
}
