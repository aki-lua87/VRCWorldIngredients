using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace aki_lua87.UdonScripts.marubatu
{
    public class marubatsugame : UdonSharpBehaviour
    {
        [SerializeField] private GameObject headerMaru;
        [SerializeField] private GameObject headerBatu;
        [SerializeField] private GameObject endText;
        [SerializeField] private GameObject[] maru;
        [SerializeField] private GameObject[] batu;

        // 次の挿入図形
        private bool isFigureMaru = false;
        private bool isGameEnd = false;
        
        public void GameEndCheck(){
            // 終了を判定
            // 0 1 2
            // 3 4 5
            // 6 7 8

// 実際のオブジェクト配置
// 9 4 7
// 8 5 6
// 3 1 2

            // maru
            if (maru[0].activeSelf && maru[1].activeSelf && maru[2].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinMaru");
            }else if (maru[3].activeSelf && maru[4].activeSelf && maru[5].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinMaru");
            }else if (maru[6].activeSelf && maru[7].activeSelf && maru[8].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinMaru");
            }else if (maru[0].activeSelf && maru[3].activeSelf && maru[6].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinMaru");
            }else if (maru[1].activeSelf && maru[4].activeSelf && maru[7].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinMaru");
            }else if (maru[2].activeSelf && maru[5].activeSelf && maru[8].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinMaru");
            }else if (maru[0].activeSelf && maru[4].activeSelf && maru[8].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinMaru");
            }else if (maru[2].activeSelf && maru[4].activeSelf && maru[6].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinMaru");
            }

            // batu
            if (batu[0].activeSelf && batu[1].activeSelf && batu[2].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinBatu");
            }else if (batu[3].activeSelf && batu[4].activeSelf && batu[5].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinBatu");
            }else if (batu[6].activeSelf && batu[7].activeSelf && batu[8].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinBatu");
            }else if (batu[0].activeSelf && batu[3].activeSelf && batu[6].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinBatu");
            }else if (batu[1].activeSelf && batu[4].activeSelf && batu[7].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinBatu");
            }else if (batu[2].activeSelf && batu[5].activeSelf && batu[8].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinBatu");
            }else if (batu[0].activeSelf && batu[4].activeSelf && batu[8].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinBatu");
            }else if (batu[2].activeSelf && batu[4].activeSelf && batu[6].activeSelf){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WinBatu");
            }
        }

        public void NextFigure(){
            isFigureMaru = !isFigureMaru;
        }

        public void ResetGame(){
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ResetMasu");
        }

        public void ResetMasu()
        {
            isFigureMaru = false;
            isGameEnd = false;
            endText.SetActive(false);
            headerMaru.SetActive(false);
            headerBatu.SetActive(false);
            foreach(GameObject obj in maru){
                obj.SetActive(false);
            }
            foreach(GameObject obj in batu){
                obj.SetActive(false);
            }
        }

        public void WinMaru(){
            endText.SetActive(true);
            headerMaru.SetActive(true);
            isGameEnd = true;
        }
        public void WinBatu(){
            endText.SetActive(true);
            headerBatu.SetActive(true);
            isGameEnd = true;
        }
    }
}