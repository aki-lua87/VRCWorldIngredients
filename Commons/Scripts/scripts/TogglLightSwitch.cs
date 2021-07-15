using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

namespace aki_lua87.UdonScripts.Common
{
    [AddComponentMenu("aki_lua87/UdonScripts/TogglLightSwitch")]
    public class TogglLightSwitch : UdonSharpBehaviour
    {
        // デフォルトは電気がついている状態
        private bool isSwitchedOn = true;
        [SerializeField] private Light Light;
        [SerializeField] private float LightONIntensity; // ex. 0.06
        [SerializeField] private float LightOFFIntensity; // ex. 0.4
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
            Light.intensity = LightONIntensity;
            ON_Swittch.SetActive(false);
            OFF_Switch.SetActive(true);
            isSwitchedOn = true;
        }

        // 対象のオブジェクトをトグル、ONスイッチを表示 OFFスイッチを非表示
        private void SwitchOFF()
        {
            Light.intensity = LightOFFIntensity;
            ON_Swittch.SetActive(true);
            OFF_Switch.SetActive(false);
            isSwitchedOn = false;
        }
    }
}