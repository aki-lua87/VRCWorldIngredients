using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

namespace aki_lua87.UdonScripts.Common
{
    [AddComponentMenu("aki_lua87/UdonScripts/InteractPlayMusic")]
    public class PlayMusic : UdonSharpBehaviour
    {
        private bool isSwitchedOn = false;
        [SerializeField] private AudioSource audioSource;

        public override void Interact()
        {
            if(isSwitchedOn){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "StopAudio");
            }else{
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "StertAudio");
            }
        }

        public void StertAudio(){ 
            isSwitchedOn = true;
            audioSource.Play();
        }

        public void StopAudio(){ 
            isSwitchedOn = false;
            audioSource.Stop();
        }
    }
}