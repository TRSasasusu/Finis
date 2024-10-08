using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace Finis {
    public class FixOWObj {
        //readonly Dictionary<string, string>[] NOMAI_TEXT_ON_CRYSTAL_PATHS = new Dictionary<string, string>[] {
        //    new Dictionary<string, string>{ {"path", "FinisPlateau_Body/Sector/finis_pod_cave_wall"}, {"color", "yellow"} },
        //};

        const string PLANET_SPHERE_PATH = "FinisPlateau_Body/Sector/finis_plateau/Sphere";
        const string WHITE_BOARD_PATH = "FinisPlateau_Body/Sector/Prefab_NOM_Whiteboard (1)/Props_NOM_Whiteboard (1)";

        Material _grayMat;

        public void DestroyResources() {
            GameObject.Destroy(_grayMat);
        }

        public FixOWObj() {
            Finis.Instance.StartCoroutine(Fix());
        }

        IEnumerator Fix() {
            //foreach(var pathDict in NOMAI_TEXT_ON_CRYSTAL_PATHS) {
            //    while(true) {
            //        yield return null;
            //        var nomai_text = GameObject.Find(pathDict["path"]);
            //        if(nomai_text != null) {
            //            var copy_nomai_text = Ins
            //        }
            //    }
            //}
            GameObject planetSphere;

            Finis.Log("start: finding planetSphere");
            while (true) {
                yield return null;
                planetSphere = GameObject.Find(PLANET_SPHERE_PATH);
                if(planetSphere) {
                    _grayMat = planetSphere.GetComponent<MeshRenderer>().materials[0];
                    break;
                }
            }
            Finis.Log("end: finding planetSphere");

            Finis.Log("start: finding whiteboard");
            while(true) {
                yield return null;
                var whiteBoard = GameObject.Find(WHITE_BOARD_PATH);
                if(whiteBoard) {
                    var stoneRenderer = whiteBoard.transform.Find("whiteboard_stone").GetComponent<MeshRenderer>();
                    var stoneMats = stoneRenderer.materials;
                    stoneMats[0] = _grayMat;
                    stoneRenderer.materials = stoneMats;
                    var brokenTilesRenderer = whiteBoard.transform.Find("whiteboard_brokenTiles").GetComponent<MeshRenderer>();
                    var brokenTilesMats = brokenTilesRenderer.materials;
                    brokenTilesMats[0] = _grayMat;
                    brokenTilesRenderer.materials = brokenTilesMats;
                    break;
                }
            }
            Finis.Log("end: finding whiteboard");
        }
    }
}
