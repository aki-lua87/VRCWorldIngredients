using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

namespace aki_lua87.UdonScripts.Common
{
// ON/OFFのInteractを発火させるオブジェクトが変更されるケース
    [AddComponentMenu("aki_lua87/UdonScripts/TogglObjectSwitch2Local")]
    public class TogglObjectSwitch2Local : UdonSharpBehaviour
    {
        private bool isSwitchedOn = false;
        [SerializeField] private GameObject TargetObject;
        [SerializeField] private GameObject ON_Swittch;
        [SerializeField] private GameObject OFF_Switch;

        public override void Interact()
        {
            if(isSwitchedOn)
            {
                SwitchOFF();
            }else{
                SwitchON();
            }
        }

        // 対象のオブジェクトをトグル、ONスイッチを非表示 OFFスイッチを表示
        private void SwitchON()
        {
            TargetObject.SetActive(!TargetObject.activeSelf);
            ON_Swittch.SetActive(false);
            OFF_Switch.SetActive(true);
            isSwitchedOn = true;
        }

        // 対象のオブジェクトをトグル、ONスイッチを表示 OFFスイッチを非表示
        private void SwitchOFF()
        {
            TargetObject.SetActive(!TargetObject.activeSelf);
            ON_Swittch.SetActive(true);
            OFF_Switch.SetActive(false);
            isSwitchedOn = false;
        }
    }
}