using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace aki_lua87.UdonScripts.OnPrehab
{
    public class ObjectRotation : UdonSharpBehaviour
    {
        [SerializeField] private float rotationSpeed = 0;

        void Update () 
        {
		    this.gameObject.transform.Rotate(new Vector3(rotationSpeed, 0, 0) * Time.deltaTime);
        }
    }
}