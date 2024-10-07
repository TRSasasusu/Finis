using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;

namespace Finis {
    public class Finis : ModBehaviour {
        public static Finis Instance;

        public FixOWObj _fixOWObj;

        public static void Log(string text, MessageType messageType = MessageType.Message) {
            Instance.ModHelper.Console.WriteLine(text, messageType);
        }

        private void Awake() {
            Instance = this;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }

        private void Start() {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"{nameof(Finis)} is loaded!", MessageType.Success);

            // Get the New Horizons API and load configs
            var newHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            newHorizons.LoadConfigs(this);

            // Example of accessing game code.
            LoadManager.OnCompleteSceneLoad += (scene, loadScene) => {
                if (loadScene != OWScene.SolarSystem) return;
                ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);
                var stateController = new StateController();
                stateController.Initialize();

                if(_fixOWObj != null) {
                    _fixOWObj.DestroyResources();
                }
                _fixOWObj = new FixOWObj();
            };
        }
    }

}
