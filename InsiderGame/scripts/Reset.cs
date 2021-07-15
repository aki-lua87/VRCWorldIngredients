using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace aki_lua87.UdonScripts.insider
{
    public class Reset : UdonSharpBehaviour
    {
        [SerializeField] private UdonBehaviour gameManager;
        public override void Interact()
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(SendGameManagerEvents));
        }

        public void SendGameManagerEvents()
        {
            gameManager.SendCustomEvent("Reset");
        }
    }
}