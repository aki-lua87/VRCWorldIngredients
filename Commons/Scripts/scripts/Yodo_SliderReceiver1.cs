using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

namespace aki_lua87.UdonScripts.Common
{
    // ON/OFFのInteractを発火させるオブジェクトが同一であること
    [AddComponentMenu("aki_lua87/UdonScripts/Yodo_SliderReceiver1")]
    public class Yodo_SliderReceiver1 : UdonSharpBehaviour
    {
        [SerializeField] private Material mat;

        public bool Yodo_isReceiveSliderValueChangeEvent = true;
        public float Yodo_PPWeight = 0.0f;

        void Start()
        {
            setDim(0.35f);
        }

        public void Yodo_OnSliderValueChanged()
        {
            setDim(Yodo_PPWeight*0.7f);
        }

        private void setDim(float x)
        {
            if(x == 0.0f)
            {
                return;
            }
            if (mat.HasProperty("_dim")) {
                mat.SetFloat("_dim", x);
            }
        }
    }
}