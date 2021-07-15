using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace aki_lua87.UdonScripts.insider
{
    public class GameManager : UdonSharpBehaviour
    {
        [SerializeField] private GameObject[] startCubes;
        [SerializeField] private GameObject insiderObject;
        [SerializeField] private Text insiderOdaiText;
        [SerializeField] private GameObject masterObject;
        [SerializeField] private Text masterOdaiText;
        [SerializeField] private GameObject[] siminObjects = new GameObject[6];

        private GameObject[] gameUsingObjects = new GameObject[0];

        [SerializeField] private GameObject defaultPositionObject;

        public int playerNum = 0;

        [UdonSynced] 
        public string odai = "";
        private string[] odais = { 
            "ハンバーグ","駅弁", "カレーライス", "麦茶", "日本酒", "しじみ", "海苔" 
            , "定規", "はさみ", "ボールペン", "ノート", "付箋", "糊", "カッター" 
            , "のこぎり", "金槌", "斧", "鎌", "軍手", "ねじ" 
            , "道路", "標識", "信号機", "トンネル", "橋", "横断歩道" 
            , "神社", "役所", "小学校", "コンビニ", "発電所", "スーパー" 
            , "トウモロコシ", "ピーナッツ", "枝豆", "レントゲン", "天気予報", "ニュース"
            , "リンゴ", "サイコロ", "ボードゲーム", "トランプ", "オセロ", "将棋"
            , "壁", "椅子", "ベッド", "天井", "屋上", "地下" 
            , "Tシャツ", "めがね", "靴下", "ズボン", "パーカー", "パンツ"
            , "散歩", "ベビーカー", "ミイラ", "狐", "猫", "犬"
            , "潜水艦", "タクシー", "ヘリコプター", "ロケット", "消防車", "自動車"
            , "くすり", "テニス", "チョコレート", "作文", "歴史", "地球温暖化"
            , "VRChat", "インターネット", "タイムマシン", "金メダル", "ダンボール", "妖精"
            , "火星", "月", "火", "水", "空気", "食欲"
            , "台所", "風呂", "トイレ", "鏡", "庭", "プラネタリウム"
            , "山", "森", "島", "沼", "川", "海"
            , "弁護士", "国会議員", "教師(先生)", "警察官", "タクシー", "農家"
            , "貴族", "人間", "鳥", "天使", "吸血鬼", "ドラゴン","カニ"
            // , "", "", "", "", "", "" コピペ用
        };

        public void Reset()
        {
            gameUsingObjects = new GameObject[0];
            insiderObject.SetActive(false);
            masterObject.SetActive(false);
            foreach (var siminObject in siminObjects)
            {
                siminObject.SetActive(false);
            }
            foreach (var cube in startCubes)
            {
                cube.SetActive(true);
            }
        }

        public void StartGame()
        {
            Debug.Log("[TRACE] StartGame call");
            if(playerNum < 4)
            {
                return;
            }
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(StartGlobal));
            if (Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) StartLocal();
        }

        // 全員で実行
        public void StartGlobal()
        {
            // 利用オブジェクト作成
            Debug.Log("[TRACE] playerNum:"+playerNum.ToString());
            gameUsingObjects = new GameObject[playerNum];
            gameUsingObjects[0] = masterObject;
            gameUsingObjects[1] = insiderObject;
            for(int i = 2; i < playerNum; i++)
            {
                gameUsingObjects[i] = siminObjects[i-2];
            }
            foreach (var o in gameUsingObjects)
            {
                o.SetActive(true);
            }
            foreach (var cube in startCubes)
            {
                cube.SetActive(false);
            }
        }
        // VRCでいうマスターが実行
        public void StartLocal()
        {
            // お題共有
            ShuffleOdais();
            odai = odais[0];
            RequestSerialization();
            insiderOdaiText.text = odai;
            masterOdaiText.text = odai;
            // 利用オブジェクトズラシ
            ShuffleCards();
            var defaultPosition = defaultPositionObject.transform.position;
            foreach (var o in gameUsingObjects)
            {
                if (!Networking.IsOwner(Networking.LocalPlayer, o)) Networking.SetOwner(Networking.LocalPlayer, o);
                // +0.0002
                defaultPosition.y += 0.0002f;
                o.transform.position = defaultPosition;
                o.transform.rotation = Quaternion.Euler(90.0f, 180.0f, 0.0f);
            }
        }
        // お題更新時を想定
        public override void OnDeserialization()
        {
            insiderOdaiText.text = odai;
            masterOdaiText.text = odai;
        }
        private void ShuffleCards()
        {
            //Fisher-Yatesアルゴリズムでシャッフルする
            // System.Random rng = new System.Random();
            // System.RandomがMethod is not exposed to Udon
            int n = gameUsingObjects.Length;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, gameUsingObjects.Length);
                var tmp = gameUsingObjects[k];
                gameUsingObjects[k] = gameUsingObjects[n];
                gameUsingObjects[n] = tmp;
            }
        }

        private void ShuffleOdais()
        {
            int n = odais.Length;
            while (n > 1)
            {
                n--;
                var k = Random.Range(0, odais.Length);
                var tmp = odais[k];
                odais[k] = odais[n];
                odais[n] = tmp;

                // 0だけもう一回
                k = Random.Range(0, odais.Length);
                tmp = odais[k];
                odais[k] = odais[0];
                odais[0] = tmp;
            }
        }
    }
}