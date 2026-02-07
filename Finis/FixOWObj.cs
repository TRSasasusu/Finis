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
        const string TABLE_PATH = "FinisPlateau_Body/Sector/Props_NOM_Table_Geo";
        const string SHELF_PATH = "FinisPlateau_Body/Sector/Strucutre_NOM_Shelf";
        readonly string[] VASE_THICK_PATHS = new string[] {
            "FinisPlateau_Body/Sector/Prefab_NOM_VaseThick_0",
            "FinisPlateau_Body/Sector/Prefab_NOM_VaseThick_1",
            "FinisPlateau_Body/Sector/Prefab_NOM_VaseThick_2",
        };
        readonly string[] CHAIR_PATHS = new string[] {
            "FinisPlateau_Body/Sector/Props_NOM_SimpleChair_Geo 1_0",
            "FinisPlateau_Body/Sector/Props_NOM_SimpleChair_Geo 1_1",
            "FinisPlateau_Body/Sector/Props_NOM_SimpleChair_Geo 1_2",
            "FinisPlateau_Body/Sector/Props_NOM_SimpleChair_Geo 1_3",
        };
        const string ESCAPE_POD_COMPUTER_PATH = "FinisPlateau_Body/Sector/EscapePod_Socket/Interactibles_EscapePod/Prefab_NOM_Vessel_Computer";
        const string MOON_ESCALL_WALL_PATH = "RedMoon_Body/Sector/finis_red_moon/weakred_crystal_moon/finis_moon_escall_wall";
        const string MOON_INFINITE_WALL_PATH = "RedMoon_Body/Sector/finis_red_moon/weakred_crystal_moon/finis_moon_infinite_wall";
        const string GREEN_MAT_OBJ_PATH = "FinisPlateau_Body/Sector/finis_plateau/green_crystal_topcave/Pillar";
        const string ERNESTO_SCROLL = "StarshipCommunity_Body/Sector/finis_ernesto_scroll/Props_NOM_Scroll/Props_NOM_Scroll_Geo";

        Material _grayMat;
        Material _greenMat;

        public void DestroyResources() {
            GameObject.Destroy(_grayMat);
            GameObject.Destroy(_greenMat);
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
                planetSphere = GameObject.Find(PLANET_SPHERE_PATH);
                if(planetSphere) {
                    _grayMat = planetSphere.GetComponent<MeshRenderer>().materials[0];
                    break;
                }
                yield return null;
            }
            Finis.Log("end: finding planetSphere");

            Finis.Log("start: finding whiteboard");
            while(true) {
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
                yield return null;
            }
            Finis.Log("end: finding whiteboard");

            Finis.Log("start: finding table");
            while(true) {
                var table = GameObject.Find(TABLE_PATH);
                if(table) {
                    var colliderObj = new GameObject("collider");
                    colliderObj.transform.parent = table.transform;
                    colliderObj.transform.localPosition = new Vector3(0, 0.5f, 0);
                    colliderObj.transform.localEulerAngles = Vector3.zero;
                    colliderObj.transform.localScale = new Vector3(2.75f, 1.1f, 1.5f);
                    colliderObj.AddComponent<BoxCollider>();


                    var tableRenderer = table.GetComponent<MeshRenderer>();
                    var tableMats = tableRenderer.materials;
                    tableMats[0] = _grayMat;
                    tableMats[2] = _grayMat;
                    tableRenderer.materials = tableMats;
                    break;
                }
                yield return null;
            }
            Finis.Log("end: finding table");

            Finis.Log("start: finding shelf");
            while(true) {
                var shelf = GameObject.Find(SHELF_PATH);
                if(shelf) {
                    var colliderObj = new GameObject("collider");
                    colliderObj.transform.parent = shelf.transform;
                    colliderObj.transform.localPosition = new Vector3(2, 0, 0);
                    colliderObj.transform.localEulerAngles = Vector3.zero;
                    colliderObj.transform.localScale = new Vector3(4, 0.5f, 1);
                    colliderObj.AddComponent<BoxCollider>();

                    var shelfRenderer = shelf.GetComponent<MeshRenderer>();
                    var shelfMats = shelfRenderer.materials;
                    shelfMats[0] = _grayMat;
                    shelfMats[1] = _grayMat;
                    shelfRenderer.materials = shelfMats;
                    break;
                }
                yield return null;
            }
            Finis.Log("end: finding shelf");

            Finis.Log("start: finding vase");
            foreach(var path in VASE_THICK_PATHS) {
                while(true) {
                    var vase = GameObject.Find(path);
                    if(vase) {
                        var colliderObj = new GameObject("collider");
                        colliderObj.transform.parent = vase.transform;
                        colliderObj.transform.localPosition = new Vector3(0, 0.3f, 0);
                        colliderObj.transform.localEulerAngles = Vector3.zero;
                        colliderObj.transform.localScale = new Vector3(0.5f, 0.6f, 0.5f);
                        colliderObj.AddComponent<BoxCollider>();

                        var vaseRenderer = vase.GetComponent<MeshRenderer>();
                        var vaseMats = vaseRenderer.materials;
                        vaseMats[0] = _grayMat;
                        vaseRenderer.materials = vaseMats;
                        break;
                    }
                    yield return null;
                }
            }
            Finis.Log("end: finding vase");

            Finis.Log("start: finding chair");
            foreach(var path in CHAIR_PATHS) {
                while(true) {
                    var chair = GameObject.Find(path);
                    if (chair) {
                        var colliderObj = new GameObject("collider");
                        colliderObj.transform.parent = chair.transform;
                        colliderObj.transform.localPosition = new Vector3(0, 0.35f, 0);
                        colliderObj.transform.localEulerAngles = new Vector3(0, 10, 0);
                        colliderObj.transform.localScale = new Vector3(1.25f, 0.65f, 0.5f);
                        colliderObj.AddComponent<BoxCollider>();

                        var chairRenderer = chair.GetComponent<MeshRenderer>();
                        var chairMats = chairRenderer.materials;
                        chairMats[0] = _grayMat;
                        chairRenderer.materials = chairMats;
                        break;
                    }
                    yield return null;
                }
            }
            Finis.Log("end: finding chair");

            Finis.Log("start: finding escape pod computer");
            while(true) {
                var escape_pod_computer = GameObject.Find(ESCAPE_POD_COMPUTER_PATH);
                if(escape_pod_computer) {
                    escape_pod_computer.GetComponent<CapsuleCollider>().enabled = false;
                    var pillar = escape_pod_computer.transform.Find("Props_NOM_Vessel_Computer 1");
                    pillar.transform.localPosition = new Vector3(0, 3, 0);
                    break;
                }
                yield return null;
            }
            Finis.Log("end: finding escape pod computer");

            Finis.Log("start: finding escall wall");
            while (true) {
                var moonEscallWall = GameObject.Find(MOON_ESCALL_WALL_PATH);
                if(moonEscallWall) {
                    moonEscallWall.transform.localScale = new Vector3(0.78f, 0.78f, 0.78f);
                    break;
                }
                yield return null;
            }
            Finis.Log("end: finding escall wall");

            Finis.Log("start: finding infinite wall");
            while(true) {
                var moonInfiniteWall = GameObject.Find(MOON_INFINITE_WALL_PATH);
                if(moonInfiniteWall) {
                    moonInfiniteWall.transform.localScale = new Vector3(0.88f, 0.88f, 0.88f);
                    break;
                }
                yield return null;
            }
            Finis.Log("end: finding infinite wall");

            //Finis.Log("start: fixing ship light");
            //var ship = Locator._shipBody;
            //var camera = Locator.GetPlayerCamera();
            ////ship.transform.Find("Module_Cockpit/Lights_Cockpit/Pointlight_HEA_ShipCockpit").GetComponent<LightmapController>().Awake();
            ////ship.transform.Find("Module_Cabin/Lights_Cabin/Pointlight_HEA_ShipCabin").GetComponent<LightmapController>().Awake();
            ////ship.transform.Find("Module_Supplies/Lights_Supplies/Pointlight_HEA_ShipSupplies_Top").GetComponent<LightmapController>().Awake();
            //ship.transform.Find("Module_Cockpit/Lights_Cockpit/Pointlight_HEA_ShipCockpit").GetComponent<LightmapController>().UpdateLightmapSettings(camera.mainCamera);
            //ship.transform.Find("Module_Cabin/Lights_Cabin/Pointlight_HEA_ShipCabin").GetComponent<LightmapController>().UpdateLightmapSettings(camera.mainCamera);
            //ship.transform.Find("Module_Supplies/Lights_Supplies/Pointlight_HEA_ShipSupplies_Top").GetComponent<LightmapController>().UpdateLightmapSettings(camera.mainCamera);
            //Finis.Log("end: fixing ship light");

            Finis.Log("start: finding green obj for mat");
            while (true) {
                var greenObj = GameObject.Find(GREEN_MAT_OBJ_PATH);
                if (greenObj) {
                    _greenMat = greenObj.GetComponent<MeshRenderer>().material;
                    break;
                }
                yield return null;
            }
            Finis.Log("end: finding green obj for mat");

            Finis.Log("start: finding that scroll");
            while (true) {
                var scroll = GameObject.Find(ERNESTO_SCROLL);
                if (scroll) {
                    scroll.GetComponent<MeshRenderer>().material = _greenMat;
                    break;
                }
                yield return null;
            }
            Finis.Log("end: finding that scroll");
        }
    }
}
