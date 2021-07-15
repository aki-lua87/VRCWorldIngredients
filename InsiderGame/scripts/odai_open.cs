
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace aki_lua87.UdonScripts.insider
{
    public class odai_open : UdonSharpBehaviour
    {
        [SerializeField] private GameObject openObject;

        // 押したとき
        public override void OnPickupUseDown()
        {
            // TogglOpenOdai();
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(OpenOdai));
        }

        public override void OnPickupUseUp()
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(CloseOdai));
            // if(openObject.activeSelf)
            // {
                
            // }
            // else
            // {
                
            // }
            
            // TogglOpenOdai();
        }

        public void OpenOdai()
        {
            openObject.SetActive(true);
        }
        public void CloseOdai()
        {
            openObject.SetActive(false);
        }
    }
}