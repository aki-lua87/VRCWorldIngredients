using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

namespace aki_lua87.UdonScripts.Common
{

// 他と違ってONでオブジェクトがアクティブ OFFでオブジェクトが非アクティブ
[AddComponentMenu("aki_lua87/UdonScripts/TogglObjectSwitchGlobal")]
public class TogglObjectSwitchGlobal : UdonSharpBehaviour
{
    [UdonSynced(UdonSyncMode.None)]
    bool isSwitchedOnSync = false;
    private bool isSwitchedOnLocal = false;
    [SerializeField] private GameObject TargetObject;
    
    // [UdonSynced(UdonSyncMode.None)]
    bool initObjectState;

    void Start()
    {
        // ターゲットのオブジェクトの初期位相を変数に格納 ()
        initObjectState = TargetObject.activeSelf;
    }

    public override void Interact()
    {
        // 同期変数を弄るためにオーナーを変更
        if (!Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        
        if(isSwitchedOnLocal)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SwitchOFF");
            isSwitchedOnSync = false;
        }else{
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SwitchON");
            isSwitchedOnSync = true;
        }
        
    }

    public void SwitchON()
    {
        isSwitchedOnLocal = true;
        TargetObject.SetActive(!initObjectState);
    }

    public void SwitchOFF()
    {
        isSwitchedOnLocal = false;
        TargetObject.SetActive(initObjectState);
    }

    // 新規入室者同期用
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        // 同期変数を参照し関数をローカルで実行
        // 現在の状態を反映させるためInteractとは逆の関数コール
        if(Networking.LocalPlayer == player)
        {
            if(isSwitchedOnSync)
            {
                SwitchON();
            }else{
                SwitchOFF();
            }
        }
    }
}
}