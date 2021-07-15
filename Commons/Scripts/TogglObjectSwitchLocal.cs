using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

namespace aki_lua87.UdonScripts.Common
{
    // ON/OFFのInteractを発火させるオブジェクトが同一であること
    [AddComponentMenu("aki_lua87/UdonScripts/TogglObjectSwitchLocal")]
    public class TogglObjectSwitchLocal : UdonSharpBehaviour
    {
        [SerializeField] private GameObject TargetObject;

        public override void Interact()
        {
            TargetObject.SetActive(!TargetObject.activeSelf);
        }
    }
}