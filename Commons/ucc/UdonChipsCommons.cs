using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UCS;
using UnityEngine.UI;
using VRC.Udon.Common;

namespace aki_lua87.UdonScripts.Commons
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class UdonChipsCommons : UdonSharpBehaviour
    {
        private UCS.UdonChips udonChips = null;
        private float localUC = 0f;

        // プレイヤー全体表示用オブジェクト
        [SerializeField] private Text allPlayersUdonChipsText;

        // 同期用配列
        [UdonSynced]
        private float[] syncAllPlayersUdonChips = new float[64]; // やけクソ初期化
        // 全プレイヤーのUdonChipsを管理、indexはプレイヤーID
        private float[] allPlayersUdonChips = new float[64]; 
		
        void Start()
		{
			udonChips = GameObject.Find("UdonChips").GetComponent<UdonChips>();
            var me = Networking.LocalPlayer;
            allPlayersUdonChips[me.playerId] = udonChips.money;
            ShowAllPlayerUdonChips();
		}

        int t = 0;
        void Update() 
        {
            if (udonChips == null)
                return;
            // なんとなく間隔をあける
            t++;
            if(t > 100)
            {
                t = 0;
                if(localUC != udonChips.money)
                {
                    // SyncUCの中でlocalUC更新
                    PushUdonChips();
                }
            }
        }
        public override void OnPostSerialization(SerializationResult result)
        {
            // 変数更新
            localUC = udonChips.money;
            // 自分の同期用変数をクリア
            var me = Networking.LocalPlayer;
            syncAllPlayersUdonChips[me.playerId] = 0;
        }

        public override void OnDeserialization()
        {
            var index = 0;
            foreach (var uc in syncAllPlayersUdonChips)
            {
                if(uc == 0f)
                {
                    index++;
                    continue;
                }
                allPlayersUdonChips[index] = syncAllPlayersUdonChips[index];
                syncAllPlayersUdonChips[index] = 0;
                break;
            }
            ShowAllPlayerUdonChips();
        }

        public float GetMoney()
        {
            return udonChips.money;
        }

        public void SetMoney(float f)
        {
            udonChips.money = f;
        }

        public void PushUdonChips()
        {
            // 同期
            var me = Networking.LocalPlayer;
            if (!Networking.IsOwner(me, this.gameObject))
                Networking.SetOwner(me, this.gameObject);
            allPlayersUdonChips[me.playerId] = udonChips.money;
            syncAllPlayersUdonChips[me.playerId] = udonChips.money;
            RequestSerialization();
            ShowAllPlayerUdonChips();
        }

        public void AddMoney(float f)
        {
            SetMoney(udonChips.money + f);
        }

        public void SubtractMoney(float f)
        {
            SetMoney(udonChips.money - f);
        }

        public bool IsAndOver(float f)
        {
            return udonChips.money >= f;
        }

        // 後方互換性担保用(1.3.1)
        public bool isOverMoney(float f)
        {
            return udonChips.money >= f;
        }

        private void ShowAllPlayerUdonChips()
        {
            var text = "";
            var index = 0;
            foreach (var uc in allPlayersUdonChips)
            {
                var p = VRCPlayerApi.GetPlayerById(index++);
                if(p == null)
                {
                    continue;
                }
                var pname = p.displayName;
                text += $"{pname} : {(int)uc} uc.";
                text += "\n";
            }
            if(allPlayersUdonChipsText == null)
            {
                return;
            }
            allPlayersUdonChipsText.text = text; 
        }

        public float GetMoneyByPlayerID(int id)
        {
            if(id > allPlayersUdonChips.Length) return 0f;
            return allPlayersUdonChips[id];
        }
    }
}