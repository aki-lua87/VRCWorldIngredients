using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace aki_lua87.UdonScripts.koyote
{
    public class GameManager : UdonSharpBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private GameObject[] cards = new GameObject[35];

        private Vector3 defaultPosition;

        void Start()
        {
            if (Networking.IsOwner(Networking.LocalPlayer, this.gameObject))
            {
                Shuffle();
                Reset();
            }
        }

        public override void Interact()
        {
            Shuffle();
            Reset();
        }

        private void Reset()
        {
            defaultPosition = parent.position;
            foreach (var card in cards)
            {
                if (!Networking.IsOwner(Networking.LocalPlayer, card)) Networking.SetOwner(Networking.LocalPlayer, card);
                // +0.0002
                defaultPosition.y = defaultPosition.y + 0.0002f;
                card.transform.position = defaultPosition;
                card.transform.rotation = Quaternion.Euler(90.0f, 180.0f, 0.0f);
            }
        }

        private void Shuffle()
        {
            //Fisher-Yatesアルゴリズムでシャッフルする
            // System.Random rng = new System.Random();
            // System.RandomがMethod is not exposed to Udon
            int n = cards.Length;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, cards.Length);
                var tmp = cards[k];
                cards[k] = cards[n];
                cards[n] = tmp;
            }
        }
    }
}