using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IEnumerator = System.Collections.IEnumerator;

namespace Finis {
    public class StateController {
        public RodItem RodItem { get; private set; }

        public void Initialize() {
            Finis.Instance.StartCoroutine(InitializeBody());
        }

        IEnumerator InitializeBody() {
            while (true) {
                yield return null;
            }
        }
    }
}
