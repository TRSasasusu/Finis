using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace Finis {
    public class RodItem : OWItem {
        public enum State {
            BLUE,
            RED,
        }
        public State Color { get; private set; } = State.BLUE;
        //public bool PickedUp { get; private set; }

        GameObject _red;
        GameObject _redEffect;
        GameObject _blue;
        GameObject _blueEffect;
        Coroutine _expandEffectCoroutine;

        public override void Awake() {
            base.Awake();
            _red = transform.Find("rod/red").gameObject;
            _redEffect = _red.transform.Find("GetEffect").gameObject;
            _blue = transform.Find("rod/blue").gameObject;
            _blueEffect = _blue.transform.Find("GetEffect").gameObject;
        }

        public override string GetDisplayName() {
            return "Rod";
        }

        public override void PickUpItem(Transform holdTranform) {
            base.PickUpItem(holdTranform);
            //PickedUp = true;
            StateController.Instance.PickUpRod(this);
        }

        public override void DropItem(Vector3 position, Vector3 normal, Transform parent, Sector sector, IItemDropTarget customDropTarget) {
            base.DropItem(position, normal, parent, sector, customDropTarget);
            //PickedUp = false;
            StateController.Instance.DropRod();
        }

        public void ChangeToRed() {
            Color = State.RED;
            _blue.SetActive(false);
            _red.SetActive(true);

            if(_expandEffectCoroutine != null) {
                StopCoroutine(_expandEffectCoroutine);
                _expandEffectCoroutine = null;
            }
            _expandEffectCoroutine = StartCoroutine(ExpandEffect(_redEffect));
        }

        public void ChangeToBlue() {
            Color = State.BLUE;
            _red.SetActive(false);
            _blue.SetActive(true);

            if(_expandEffectCoroutine != null) {
                StopCoroutine(_expandEffectCoroutine);
                _expandEffectCoroutine = null;
            }
            _expandEffectCoroutine = StartCoroutine(ExpandEffect(_blueEffect));
        }

        IEnumerator ExpandEffect(GameObject effect) {
            effect.transform.localScale = Vector3.one;
            effect.SetActive(true);

            float t = 0;
            float acceleration = 20;
            float speed = 5;
            while(true) {
                yield return null;
                if(!effect) {
                    yield break;
                }
                speed += acceleration * Time.deltaTime;
                effect.transform.localScale += Vector3.one * Time.deltaTime * speed;
                t += Time.deltaTime;
                if(t > 2f) {
                    break;
                }
            }

            effect.SetActive(false);
        }
    }
}
