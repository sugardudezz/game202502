Shader "Custom/Vignette"
{
    Properties{
        _MainTex("Source", 2D) = "white" {}
        _TimeScale("Time Scale", float) = 1
    }

    SubShader{
        Cull Off ZWrite Off ZTest Always

        Pass{
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // xy = 1/width,height
            float _Time;

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(uint id : SV_VertexID){
                v2f o;

                // Fullscreen triangle trick (no mesh needed)
                float2 uv = float2((id<<1) & 2, id & 2);
                o.pos = float4(uv * 2 - 1, 0, 1);
                o.uv = uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // Example effect: animated distortion
                float2 uv = i.uv;
                float d = distance(uv.xy, float3(0.5,0.5,1.));
                float3 col = float3(d,d,d);
                col = float3(1.0, 1.0, 1.0)-col;
                col = col*col;
                col += 0.1;
                col *= 0.8;
                return float4(col, 1.0);
            }

            ENDHLSL
        }
    }
}