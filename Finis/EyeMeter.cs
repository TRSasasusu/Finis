using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Finis {
    public class EyeMeter : MonoBehaviour {
        const float SPEED = 1;

        Transform _hatchlingMeter;

        void Awake() {
            _hatchlingMeter = transform.Find("Cone");
        }

        void Update() {
            var angles = transform.localEulerAngles;
            angles += new Vector3(1, Mathf.Sqrt(3), 0) * SPEED * Time.deltaTime;
            transform.localEulerAngles = angles;

            if(StateController.Instance != null && StateController.Instance.PlayerBody) {
                var distance = Vector3.Distance(StateController.Instance.PlayerBody.transform.position, transform.position);
                if(distance > 29) {
                    _hatchlingMeter.transform.localScale = Vector3.one;
                    return;
                }

                _hatchlingMeter.transform.LookAt(StateController.Instance.PlayerBody.transform);
                _hatchlingMeter.transform.localScale = new Vector3(1, 1, distance * 0.5f);
            }
        }
    }
}
