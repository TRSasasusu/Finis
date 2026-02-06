using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Finis {
    public class EndBHTrigger : MonoBehaviour {
        public ControllerForEyeOnJam3System _controller;

        bool _done = false;

        void OnTriggerEnter(Collider other) {
            if(_done) {
                return;
            }

            if (other.attachedRigidbody != null) {
                if(other.attachedRigidbody.CompareTag("Player") || (PlayerState.IsInsideShip() && other.attachedRigidbody.CompareTag("Ship"))) {
                    _controller.StartEye();
                    _done = true;
                }
            }
        }
    }
}
