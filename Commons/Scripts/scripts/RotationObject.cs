using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

namespace aki_lua87.UdonScripts.Common
{
    [AddComponentMenu("aki_lua87/UdonScripts/RotationObject")]
    public class RotationObject : UdonSharpBehaviour
    {
        [SerializeField] private float RotateSpeedX;
        [SerializeField] private float RotateSpeedY;
        [SerializeField] private float RotateSpeedZ;
        void Update()
        {
            this.gameObject.transform.Rotate(RotateSpeedX,RotateSpeedY,RotateSpeedZ);
        }
    }
}