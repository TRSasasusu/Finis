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
        }
    }
}
