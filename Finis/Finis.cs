using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;

namespace Finis {
    public class Finis : ModBehaviour {
        public static Finis Instance;
        public static INewHorizons newHorizons;
        public static bool _bandTogether;

        public StateController _stateController;
        public FixOWObj _fixOWObj;
        //public ControllerForEye _controllerForEye;
        public ControllerForEyeOnJam3System _controllerForEye;

        public static void Log(string text, MessageType messageType = MessageType.Message) {
            Instance.ModHelper.Console.WriteLine(text, messageType);
        }

        public static bool ModExist(string mod) {
            return Instance.ModHelper.Interaction.ModExists(mod);
        }

        private void Awake() {
            Instance = this;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }

        private void Start() {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"{nameof(Finis)} is loaded!", MessageType.Success);

            //PlayerData.SaveEyeCompletion(); // Start should be Jam3 not the Eye.

            // Get the New Horizons API and load configs
            newHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            newHorizons.LoadConfigs(this);

            bool initial = true;
            ModHelper.Events.Unity.RunWhen(() => PlayerData._currentGameSave != null, () => {
                Log("Start in Jam3 not in the Eye");
                PlayerData._currentGameSave.warpedToTheEye = false;
            });

            _bandTogether = ModHelper.Interaction.ModExists("pikpik_carrot.BandTogether");
            if(_bandTogether ) {
                ModHelper.Events.Unity.FireOnNextUpdate(() => {
                    BandTogetherController.FixBandTogether();
                });
            }

            // Example of accessing game code.
            LoadManager.OnCompleteSceneLoad += (scene, loadScene) => {
                //if(loadScene != OWScene.SolarSystem || newHorizons.GetCurrentStarSystem() != "Jam3") {
                //    if(loadScene == OWScene.EyeOfTheUniverse) {
                //        if(_controllerForEye != null && _controllerForEye.ClonedShip) {
                //            initial = false;
                //            _controllerForEye.SpawnShip();
                //        }
                //        if(initial) {
                //            Log("initial fix is now worked");
                //            PlayerData._currentGameSave.warpedToTheEye = false;
                //            LoadManager.LoadScene(OWScene.SolarSystem, LoadManager.FadeType.ToBlack, 0.5f);
                //        }
                //    }
                //    return;
                //}
                ModHelper.Console.WriteLine("Loaded into Jam3 system!", MessageType.Success);

                if (_stateController != null) {
                    _stateController.DestroyResources();
                }
                _stateController = new StateController();
                _stateController.Initialize();

                if(_fixOWObj != null) {
                    _fixOWObj.DestroyResources();
                }
                _fixOWObj = new FixOWObj();

                if(_controllerForEye != null) {
                    //if(_controllerForEye.ClonedShip) {
                    //    return;
                    //}
                    _controllerForEye.DestroyResources();
                }
                _controllerForEye = new ControllerForEyeOnJam3System();
                _controllerForEye.Initialize();
                //_controllerForEye = new ControllerForEye();
                //_controllerForEye.CloneShip();
            };
        }
    }

}
