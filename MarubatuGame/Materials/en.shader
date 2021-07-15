Shader "Unlit/en"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Transparent"
				"RenderType" = "Transparent"
			}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 色
                fixed4 white   = fixed4(1,1,1,1);
                fixed4 brack = fixed4(0,0,0,1);

                // 画面中心からの距離 中心
                fixed len = distance(i.uv, fixed2(0.5,0.5));

                // step(t, x) xの値がtよりも小さい場合には0、大きい場合には1
                // 0未満の時に描写しない
                float area = step(0.999, sin(len*4));
                clip(area - 1); 
                return lerp(white, brack, area);
            }
            ENDCG
        }
    }
}
