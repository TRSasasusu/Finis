using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Finis {
    public static class BandTogetherController {
        public static void FixBandTogether() {
            Finis.Log("Start resolving to conflict with BandTogether");

            // See https://stackoverflow.com/a/28503102 because most methods of event cannot be used in outside the class it is defined on
            FieldInfo subscribersReflect = typeof(LoadManager).GetField("OnCompleteSceneLoad", BindingFlags.Static | BindingFlags.NonPublic);
            MulticastDelegate editableOnCompleteSceneLoad = subscribersReflect.GetValue(null) as MulticastDelegate;
            Delegate[] subscribers = editableOnCompleteSceneLoad.GetInvocationList();

            foreach(var subscriber in subscribers) {
                Finis.Log($"subscriber: {subscriber.Target}");
                if(subscriber.Target.ToString() != "pikpik_carrot.BandTogether (BandTogether.ModMain)") {
                    continue;
                }

                var btOnCompleteSceneLoad = (LoadManager.SceneLoadEvent)subscriber;
                Finis.newHorizons.GetStarSystemLoadedEvent().AddListener(loadScene => {
                    OWScene oWScene;
                    if (loadScene == "Jam3") {
                        oWScene = OWScene.SolarSystem;
                    }
                    else {
                        oWScene = OWScene.EyeOfTheUniverse;
                    }
                    btOnCompleteSceneLoad(OWScene.None, oWScene);
                });
                LoadManager.OnCompleteSceneLoad -= btOnCompleteSceneLoad;
                break;
            }
            Finis.Log("End moving BT's function from OnCompleteSceneLoad to NH's GetStarSystemLoadedEvent");

            subscribersReflect = typeof(LoadManager).GetField("OnStartSceneLoad", BindingFlags.Static | BindingFlags.NonPublic);
            MulticastDelegate editableOnStartSceneLoad = subscribersReflect.GetValue(null) as MulticastDelegate;
            subscribers = editableOnStartSceneLoad.GetInvocationList();

            foreach(var subscriber in subscribers) {
                Finis.Log($"subscriber: {subscriber.Target}");
                if(subscriber.Target.ToString() != "pikpik_carrot.BandTogether (BandTogether.ModMain)") {
                    continue;
                }

                var btOnStartSceneLoad = (LoadManager.SceneLoadEvent)subscriber;
                Finis.newHorizons.GetChangeStarSystemEvent().AddListener(loadScene => {
                    OWScene oWScene;
                    if (loadScene == "Jam3") {
                        oWScene = OWScene.SolarSystem;
                    }
                    else {
                        oWScene = OWScene.EyeOfTheUniverse;
                    }
                    btOnStartSceneLoad(oWScene, OWScene.None);
                });
                LoadManager.OnStartSceneLoad -= btOnStartSceneLoad;
                break;
            }
            Finis.Log("End moving BT's function from OnStartSceneLoad to NH's GetChangeStarSystemEvent");
        }

        //event EventHandler hoge;
        //event Piyo piyo;
        //public void FixBandTogether() {
        //    hoge.GetInvocationList();
        //    piyo.GetInvocationList();
        //    piyo += (x, y) => { };
        //    LoadManager.OnCompleteSceneLoad.GetInvocationList();
        //}

        //delegate void Piyo(string a, string b);
    }
}
