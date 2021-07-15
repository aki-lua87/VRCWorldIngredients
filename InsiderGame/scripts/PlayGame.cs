
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace aki_lua87.UdonScripts.insider
{
    public class PlayGame : UdonSharpBehaviour
    {
        [SerializeField] private UdonBehaviour gameManager;
        [SerializeField] private int playerNum;

        public override void Interact()
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(SendGameManagerEvents));
        }

        public void SendGameManagerEvents()
        {
            gameManager.SetProgramVariable("playerNum", playerNum);
            gameManager.SendCustomEvent("StartGame");
        }
    }
}