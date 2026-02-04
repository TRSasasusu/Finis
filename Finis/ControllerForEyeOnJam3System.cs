using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace Finis {
    public class ControllerForEyeOnJam3System {
        const string DESTROY_CREADIT_VOLUME_PATH = "EyeOfTheUniverse_Body/Sector/destroy_credit_volume";
        const string EYE_PATH = "EyeOfTheUniverse_Body";
        const string SHELLING_PATH = "Shelling_Body";

        GameObject _destroyCreditVolume;
        GameObject _eye;
        GameObject _shelling;

        Coroutine _initializeBody;
        Coroutine _creditDestroyCoroutine;

        public void Initialize() {
            _initializeBody = Finis.Instance.StartCoroutine(InitializeBody());
        }

        IEnumerator InitializeBody() {
            while(true) {
                _destroyCreditVolume = GameObject.Find(DESTROY_CREADIT_VOLUME_PATH);
                if(_destroyCreditVolume) {
                    _destroyCreditVolume.SetActive(false);
                    break;
                }
                yield return null;
            }

            while (true) {
                _eye = GameObject.Find(EYE_PATH);
                if(_eye) {
                    _eye.SetActive(false);
                    break;
                }
                yield return null;
            }

            while (true) {
                _shelling = GameObject.Find(SHELLING_PATH);
                if (_shelling) {
                    _shelling.SetActive(false);
                    break;
                }
                yield return null;
            }
        }

        public void StartEye() {
            _creditDestroyCoroutine = Finis.Instance.StartCoroutine(DestroyCreditBody());
        }

        public void DestroyResources() {
            if (_initializeBody != null) {
                Finis.Instance.StopCoroutine(_initializeBody);
                _initializeBody = null;
            }
            if(_creditDestroyCoroutine != null) {
                Finis.Instance.StopCoroutine(_creditDestroyCoroutine);
                _creditDestroyCoroutine = null;
            }
        }

        IEnumerator DestroyCreditBody() {
            GameObject destroyCreditVolume;
            while (true) {
                destroyCreditVolume = GameObject.Find("destroy_credit_volume");
                if(destroyCreditVolume) {
                    destroyCreditVolume.SetActive(false);
                    break;
                }
                yield return null;
            }
            yield return new WaitForSeconds(120);
            destroyCreditVolume.SetActive(true);
        }
    }
}
