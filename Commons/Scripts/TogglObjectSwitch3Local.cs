using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

namespace aki_lua87.UdonScripts.Common
{
// ON/OFFのInteractを発火させるオブジェクトが変更されるケース
    [AddComponentMenu("aki_lua87/UdonScripts/TogglObjectSwitch3Local")]
    public class TogglObjectSwitch3Local : UdonSharpBehaviour
    {
        private bool isSwitchedOn = false;

        [SerializeField] private GameObject ON_Object;
        [SerializeField] private GameObject OFF_Object;
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
            ON_Swittch.SetActive(false);
            ON_Object.SetActive(true);
            OFF_Switch.SetActive(true);
            OFF_Object.SetActive(false);
            isSwitchedOn = true;
        }

        // 対象のオブジェクトをトグル、ONスイッチを表示 OFFスイッチを非表示
        private void SwitchOFF()
        {
            ON_Swittch.SetActive(true);
            ON_Object.SetActive(false);
            OFF_Switch.SetActive(false);
            OFF_Object.SetActive(true);
            isSwitchedOn = false;
        }
    }
}