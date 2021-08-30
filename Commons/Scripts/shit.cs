using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

namespace aki_lua87.UdonScripts.Common
{
// ON/OFFのInteractを発火させるオブジェクトが変更されるケース
    [AddComponentMenu("aki_lua87/UdonScripts/shit")]
    public class shit : UdonSharpBehaviour
    {
        [SerializeField] private RuntimeAnimatorController anim;
        [SerializeField] private VRC.SDK3.Components.VRCStation isu1;
        [SerializeField] private VRC.SDKBase.VRCStation isu2;
        public override void Interact()
        {
            var p = Networking.LocalPlayer;
            // p.PushAnimations(anim);
            p.UseAttachedStation();
            p.Immobilize(false);

            
        }
    }
}