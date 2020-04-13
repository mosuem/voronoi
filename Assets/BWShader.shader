Shader "Hidden/BWDiffuse" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma fragment frag
            #pragma vertex vert
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            half4 _MainTex_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };
            uniform float4 _colors[200];
            uniform float4 _positionVec[200];
            uniform float4 _advantage[200];
            uniform float _minRad[200];
            uniform float _positionVecCount;

            float getDist(float2 isIn, float2 pos2){
                return sqrt((isIn.x - pos2.x)*(isIn.x - pos2.x) + (isIn.y - pos2.y)*(isIn.y - pos2.y));
            }

            int GetNext(float2 testPos)
            {
                int minDot =-1;
                float minDist = 100000;
                for(int index = 0; index < _positionVecCount; index++){
                    float2 dotPos = _positionVec[index] * _ScreenParams.xy;
                    float thisDist = getDist(testPos, dotPos);
                    if(thisDist < _minRad[index]){
                        thisDist = _advantage[index].y*thisDist - _advantage[index].x;
                        if(thisDist < minDist){
                            minDist = thisDist;
                            minDot = index;
                        }
                    }
                }
                if(minDist<0.1){
                    
                }
                return minDot;
            }

            float4 GetColor(int generator)
            {
                return _colors[generator];
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target {
                float2 screenPos = i.screenPos;
                float2 worldpos = i.screenPos * _ScreenParams.xy;
                int nextDot = GetNext(worldpos);

                if(nextDot == -2)
                {
                    return float4(0,0,0,0);
                }
                if(nextDot == -1)
                {
                    return tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST));
                }
                
                return GetColor(nextDot);
            }
            ENDCG
        }
    }
}