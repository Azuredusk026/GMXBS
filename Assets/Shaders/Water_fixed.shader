Shader "Unlit/Water_fixed"
{
    Properties
    {
        _FoamTex ("FoamTex", 2D) = "white" {}
        _Scale("FoamScale",float) = 10
        _FoamColor("FoamColor",Color) = (255,255,255,255)
        _NoiseCutOff("NoiseCutOff",float) = 0.8

        _RefrBumpTex ("RefrBumpTex", 2D) = "white" {}
        _RampDistance("RampDistance",float) = 1
        _DeepColor("waterColor",Color) = (255,255,255,255)
        _ShallowColor("ShallowColor",Color) = (0,0,0,0)

        _ReflBumpTex ("ReflBumpTex", 2D) = "white" {}
        _ReflOffset("ReflOffset",float) = 0
        _ReflScaleX("ReflScaleX",float) = 10
        _ReflScaleZ("ReflScaleZ",float) = 10
        _ReflMaskTex ("ReflMaskTex", 2D) = "white" {}
        _ReflMaskScale("ReflMaskScale",float) = 10
        _ReflColor("ReflColor",Color) = (255,255,255,255)
        
        _Refl2BumpTex ("Refl2BumpTex", 2D) = "white" {}
        _Refl2Offset("Refl2Offset",float) = 0
        _Refl2ScaleX("Refl2ScaleX",float) = 10
        _Refl2ScaleZ("Refl2ScaleZ",float) = 10
        _Refl2MaskTex ("Refl2MaskTex", 2D) = "white" {}
        _Refl2MaskScale("Refl2MaskScale",float) = 10
        _Refl2Color("ReflColor",Color) = (255,255,255,255)

        _FoamThickness("FoamThickness",float) = 1
        _WaveSpeed("WaveSpeed",float) = -50
        _WaveForward("WaveForward",Range(0, 180)) = 0
        _DistortionX("DistortionX",Range(-1, 1)) = 0.18
        _DistortionY("DistortionY",Range(-1, 1)) = 0.18
        _DistortionW("DistortionW",Range(-1, 1)) = 0.18

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            sampler2D _FoamTex;
            sampler2D _CameraDepthTexture;
            sampler2D _RefrBumpTex;
            sampler2D _ReflBumpTex;
            sampler2D _Refl2BumpTex;
            sampler2D _ReflMaskTex;
            sampler2D _Refl2MaskTex;

            float _RampDistance;
            float _NoiseCutOff;
            float _FoamThickness;
            float _WaveSpeed;
            float _WaveForward;
            float _Scale;
            float _ReflOffset;
            float _ReflScaleX;
            float _ReflScaleZ;
            float _ReflMaskScale;
            float _Refl2Offset;
            float _Refl2ScaleX;
            float _Refl2ScaleZ;
            float _Refl2MaskScale;
            float _DistortionX;
            float _DistortionY;
            float _DistortionW;

            float _MaxDistance;

            float4 _DeepColor;
            float4 _ShallowColor;
            float4 _FoamColor;
            float4 _ReflColor;
            float4 _Refl2Color;

            struct a2v
            {
                float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 scrPos : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD2;
            };


            v2f vert (a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.scrPos = ComputeGrabScreenPos(o.pos);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);  
                return o;
            }
            
            fixed4 BlendColor (fixed4 top, fixed4 bottom)
            {
                float3 col = top.rgb * top.a + bottom.rgb * (1 - top.a);
                float a = top.a + bottom.a * (1 - top.a);
                return fixed4(col, a);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                

                float radians = _WaveForward * UNITY_PI / 180.0;
                float WaveX = i.worldPos.x / _Scale + _Time.x * _WaveSpeed * cos(radians);
                float WaveZ = i.worldPos.z / _Scale + _Time.x * _WaveSpeed * sin(radians);
                fixed4 noise1 = tex2D(_FoamTex , float2(WaveX , WaveZ));
                    

                float depth = LinearEyeDepth(tex2D(_CameraDepthTexture, i.scrPos.xy / i.scrPos.w));

                float foamDepthDifference = saturate((depth - i.scrPos.w) / _FoamThickness) * _NoiseCutOff;
                float foam = noise1 > foamDepthDifference ? 1 : 0;
                fixed4 foamColor = _FoamColor * foam;
                
                float ReflWaveX = (i.worldPos.x + _ReflOffset) / _ReflScaleX + _Time.x * _WaveSpeed * cos(radians);
                float ReflWaveZ = i.worldPos.z / _ReflScaleZ + _Time.x * _WaveSpeed * sin(radians);
                i.worldPos.x = i.worldPos.x + _ReflOffset;
                fixed4 Mask = tex2D(_ReflMaskTex , i.worldPos.xz / _ReflMaskScale);
                fixed4 noise2 = tex2D(_ReflBumpTex , float2(ReflWaveX , ReflWaveZ));
                float Refl = Mask > noise2 ? 1 : 0;
                fixed4 ReflColor = _ReflColor * Refl;
                
                float Refl2WaveX = i.worldPos.x / _Refl2ScaleX + _Time.x * _WaveSpeed * cos(radians);
                float Refl2WaveZ = i.worldPos.z / _Refl2ScaleZ + _Time.x * _WaveSpeed * sin(radians);
                i.worldPos.x = i.worldPos.x - _ReflOffset + _Refl2Offset;
                fixed4 Mask2 = tex2D(_Refl2MaskTex , i.worldPos.xz / _Refl2MaskScale);
                fixed4 noise3 = tex2D(_Refl2BumpTex , float2(Refl2WaveX , Refl2WaveZ));
                float Refl2 = Mask2 > noise3 ? 1 : 0;
                fixed4 Refl2Color = _Refl2Color * Refl2;

                fixed3 bump = UnpackNormal(tex2D(_RefrBumpTex,float2(WaveX , WaveZ)));
                float offsetX = bump.x * _DistortionX;
                float offsetY = bump.y * _DistortionY;
                float offsetW = _DistortionW;
                
                float4 scrPos = float4(i.scrPos.x + offsetX,i.scrPos.y + offsetY,i.scrPos.z,i.scrPos.w + offsetW);
                float2 uv = scrPos.xy / scrPos.w;
                depth = LinearEyeDepth(tex2D(_CameraDepthTexture, uv));
                float waterDepthDifference = saturate((depth - i.scrPos.w)/ _RampDistance);
                fixed4 waterColor = lerp(_ShallowColor,_DeepColor,waterDepthDifference);
                
                fixed4 FinalReflColor = BlendColor (Refl2Color , ReflColor);
                return BlendColor (foamColor , BlendColor (FinalReflColor , waterColor));

            }
            ENDCG
        }
    } 
}
