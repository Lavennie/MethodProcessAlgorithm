// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Barrier" {
    Properties{
        _MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
        _Color("Main Color", Color) = (1,1,1,1)
        _Height("Barrier height", Float) = 10.0
        _SizeW("Square width", Float) = 10.0
        _SizeH("Square height", Float) = 10.0
        _Distance("Distance", Float) = 10.0
    }

    SubShader{
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        LOD 100

        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 worldSpacePos : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Height;
            float _SizeW;
            float _SizeH;
            float3 _PlayerPos;
            float _Distance;

            v2f vert(appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.worldSpacePos = mul(unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = _Distance - distance(_PlayerPos, i.worldSpacePos);
                fixed4 col = tex2D(_MainTex, float2((i.worldSpacePos.x + i.worldSpacePos.z) / _SizeW, i.worldSpacePos.y / _SizeH));
                if (i.worldSpacePos.y > _Height)
                {
                    col.a = 0.0;
                }
                else if(i.worldSpacePos.y > 0)
                {
                    col.a *= 1.0 - (i.worldSpacePos.y / _Height) * (i.worldSpacePos.y / _Height);
                }

                if(dist > 0.0)
                {
                    col.a *= dist / _Distance;
                }
                else
                {
                    col.a *= 0.2;
                }
                col *= _Color;
                col.a *= 2;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
        ENDCG
        }
    }
}
