using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Finis {
    public class RodItem : OWItem {
        public enum State {
            BLUE,
            RED,
        }
        public State Color { get; private set; } = State.BLUE;
        public bool PickedUp { get; private set; }

        public override void Awake() {
            base.Awake();
        }

        public override string GetDisplayName() {
            return "Rod";
        }

        public override void PickUpItem(Transform holdTranform) {
            base.PickUpItem(holdTranform);
            PickedUp = true;
        }

        public override void DropItem(Vector3 position, Vector3 normal, Transform parent, Sector sector, IItemDropTarget customDropTarget) {
            base.DropItem(position, normal, parent, sector, customDropTarget);
            PickedUp = false;
        }
    }
}
