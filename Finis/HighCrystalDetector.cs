using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Finis {
    public class HighCrystalDetector : MonoBehaviour {
        //public RodItem.State Color;

        void OnCollisionEnter(Collision collision) {
            Finis.Log($"collided with {collision.collider.name}");
            if(collision.collider.transform.parent && collision.collider.transform.parent.name.Contains("highred_crystal")) {
                StateController.Instance.CollisionRed();
            }
            else if(collision.collider.transform.parent && collision.collider.transform.parent.name.Contains("highblue_crystal")) {
                StateController.Instance.CollisionBlue();
            }
            //if(collision.collider.name == "Player_Body") {
            //    if(Color == RodItem.State.RED) {
            //        StateController.Instance.CollisionRed();
            //    }
            //    else {
            //        StateController.Instance.CollisionBlue();
            //    }
            //}
        }
    }
}
