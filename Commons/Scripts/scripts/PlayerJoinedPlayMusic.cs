using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;    

namespace aki_lua87.UdonScripts.OnPrehab
{
    public class PlayerJoinedPlayMusic : UdonSharpBehaviour
    {
        // AudioSource 人が入ってきたときになる音
        [SerializeField] private AudioSource audioSource;

        // 人が入ってきたときのハンドラ
        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            audioSource.Play();
        }
    }
}