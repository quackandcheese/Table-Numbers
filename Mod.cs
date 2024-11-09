using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.Logging;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMods;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.Xml;
using TableNumbers.Components;
using TableNumbers.Views;
using TMPro;
using UnityEngine;
using KitchenLogger = KitchenLib.Logging.KitchenLogger;

namespace KitchenTableNumbers
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "com.quackandcheese.tablenumbers";
        public const string MOD_NAME = "Table Numbers";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "QuackAndCheese";
        public const string MOD_GAMEVERSION = ">=1.1.9";

        internal static KitchenLogger Logger;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        {
            Logger.LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        protected override void OnUpdate()
        {
        }

        private bool firstLoad = true;
        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            Logger = InitLogger();

            Events.BuildGameDataEvent += delegate (object s, BuildGameDataEventArgs args)
            {
                if (!firstLoad)
                    return;
                firstLoad = false;

                Appliance teleporter = GDOUtils.GetExistingGDO(ApplianceReferences.Teleporter) as Appliance;

                foreach (Appliance appliance in args.gamedata.Get<Appliance>())
                {
                    if (appliance.Properties.OfType<CApplianceTable>().Any())
                    {
                        appliance.Properties.Add(new CTableNumber());
                        GameObject label = GameObject.Instantiate(teleporter.Prefab.transform.Find("Label 1").gameObject);
                        TableNumberView tableNumberView = appliance.Prefab.AddComponent<TableNumberView>();
                        label.transform.SetParent(appliance.Prefab.transform);
                        tableNumberView.Label = label.GetComponent<TextMeshPro>();
                    }
                }
            };
        }
    }
}