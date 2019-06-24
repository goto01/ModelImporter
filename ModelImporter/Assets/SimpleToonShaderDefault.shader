Shader "SimpleToon/Default" 
{
    Properties 
	{
		[Enum(Back, 2, Front, 1, Off, 0)] _Culling("Culling", int) = 2
		//Texture
        _MainTexture ("Main texture", 2D) = "white" {}
		_Tint ("Tint", color) = (1,1,1,1)
		_HighLightColor ("Highlight color", color) = (1,1,1,1)
		_HighLightColorPower ("Power", Float) = 1
		[MaterialToggle] _EnableTextureTransparent ("Enable Texture Transparent", Float ) = 0
		//Outline
		[Toggle(Outline_ON)] Outline_ON ("Outline", Float ) = 1
		//Features
		[Toggle(OutlineFeature)] OutlineFeature ("Outline", Float ) = 1
		_OutlineWidth ("Width", Float) = 5
		_OutlineZPosition ("Outline z position", Float) = 0
		_OutlineColor ("Color", color) = (0, 0, 0, 1)
		[MaterialToggle] _OutlineByDistance ("Outline Width Affected By View Distance", Float ) = 0
		_OutlineDistanceMaxWidth ("Distance of max width", Float ) = 5
    }
    SubShader {
        Tags {
            "Queue"="Geometry"
            "RenderType"="Opaque"
        }
		Pass 
		{
            Name "Outline"
            Tags {
					"LightMode" = "Always"
            }
            
			Cull Front
			
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			
            #include "UnityCG.cginc"
			
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
			#pragma multi_compile_instancing

			#pragma shader_feature OutlineFeature
			#pragma shader_feature N_F_CO_ON

            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform fixed _EnableTextureTransparent;
            uniform half _Cutout;
            uniform fixed _UseSecondaryCutout;
            uniform sampler2D _SecondaryCutout; uniform float4 _SecondaryCutout_ST;
            uniform fixed _AlphaBaseCutout;
			
			uniform half _OutlineWidth;
			uniform half _OutlineZPosition;
            uniform half4 _OutlineColor;
            uniform fixed _OutlineByDistance;
			uniform half _OutlineDistanceMaxWidth;
            
            uniform fixed _TexturePatternStyle;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 vertexColor : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;

				UNITY_SETUP_INSTANCE_ID (v);

                o.uv0 = v.texcoord0;

                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                half node_1229 = distance(objPos.rgb,_WorldSpaceCameraPos);

				float3 _OEM = UnityObjectToWorldNormal(v.vertexColor.rgb);
				//float3 _OEM = v.vertexColor.rgb;
				//_OEM = v.normal;

                o.pos = UnityObjectToClipPos(float4(v.vertex.xyz + _OEM*lerp(_OutlineWidth, clamp(_OutlineWidth*node_1229, _OutlineWidth, _OutlineDistanceMaxWidth), _OutlineByDistance)*0.01,1));
               
				#if defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)
					o.pos.z = o.pos.z + _OutlineZPosition * 0.0005;
				#else
					o.pos.z = o.pos.z - _OutlineZPosition * 0.0005;
				#endif

				UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                half4 _MainTex_var = tex2D(_MainTexture, TRANSFORM_TEX(i.uv0, _MainTexture));
                clip(lerp(1.0, _MainTex_var.a, _EnableTextureTransparent) - .5);

				fixed4 finalRGBA = fixed4(_OutlineColor.rgb, 0);
				UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
		
		Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
			
			Cull [_Culling]
                        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog

			#pragma multi_compile_instancing

            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
			uniform half4 _Tint;
			uniform half4 _HighLightColor;
			uniform half _HighLightColorPower;
			uniform fixed _EnableTextureTransparent;
            
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD5;
				float3 normal : NORMAL;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
			
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID (v);
				o.normal = v.normal;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                half4 _MainTex_var = tex2D(_MainTexture,TRANSFORM_TEX(i.uv0, _MainTexture));
				clip(lerp(1.0, _MainTex_var.a, _EnableTextureTransparent) - .5);
				half3 final_rgb = _MainTex_var.rgb;
				final_rgb *= _Tint.rgb;
				half3 high_light_color = _HighLightColor.rgb * _HighLightColorPower;
				final_rgb *= high_light_color;
                UNITY_APPLY_FOG(i.fogCoord, final_rgb);
				float4 final_rbga = float4(final_rgb, 1);
				final_rbga.rgb = i.normal.rgb;
                return final_rbga;
            }
            ENDCG
        }
    }
	CustomEditor "SimpleToonShader.Scripts.Editor.SimpleToonShaderDefaultEditor"
}