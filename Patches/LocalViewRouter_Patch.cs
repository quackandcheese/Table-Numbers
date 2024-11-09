using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenTableNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableNumbers.Views;
using UnityEngine;

namespace TableNumbers.Patches
{
    [HarmonyPatch]
    static class LocalViewRouter_Patch
    {
        [HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
        [HarmonyPostfix]
        static void GetPrefab_Postfix(ref LocalViewRouter __instance, ViewType view_type, ref GameObject __result)
        {
            if (view_type == ViewType.TableIndicator && __result != null && __result.GetComponentInChildren<TableNumberView>() == null)
            {
                Appliance teleporter = GDOUtils.GetExistingGDO(ApplianceReferences.Teleporter) as Appliance;
                GameObject label = GameObject.Instantiate(teleporter.Prefab.transform.Find("Label 2").gameObject);
                TableNumberView indicatorView = __result.AddComponent<TableNumberView>();
                label.transform.SetParent(__result.transform);
            }
        }
    }
}
