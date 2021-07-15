using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace aki_lua87.UdonScripts.marubatu
{
    public class masu : UdonSharpBehaviour
    {
        // 図形オブジェクト参照
        [SerializeField] private GameObject maru;
        [SerializeField] private GameObject batu;
        [SerializeField] private UdonBehaviour parent;
        public override void Interact()
        {
            // 書き込まれていた場合は挿入不可
            if(maru.activeSelf || batu.activeSelf){
                return;
            }
            var isGameEnd = (bool) parent.GetProgramVariable("isGameEnd");
            if(isGameEnd){
                // ゲーム終了してたら書けないように
                return;
            }
            
            var isFigureMaru = (bool) parent.GetProgramVariable("isFigureMaru");

            // 描写
            if(!isFigureMaru){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WriteMaru");
            }else{
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WriteBatu");
            }
        }

        public void WriteMaru()
        {
            parent.SendCustomEvent("NextFigure");
            maru.SetActive(true);
            parent.SendCustomEvent("GameEndCheck");
        }

        public void WriteBatu()
        {
            parent.SendCustomEvent("NextFigure");
            batu.SetActive(true);
            parent.SendCustomEvent("GameEndCheck");
        }
    }
}