using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace aki_lua87.UdonScripts.marubatu
{
    public class reset : UdonSharpBehaviour
    {
        [SerializeField] private UdonBehaviour parent;
        public override void Interact()
        {
            parent.SendCustomEvent("ResetGame");
        }
    }
}