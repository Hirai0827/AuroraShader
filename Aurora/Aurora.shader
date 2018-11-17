Shader "Custom/Aurora"
{
    Properties {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Color("Top Color",color) = (1.0,1.0,1.0)
		_Color2("Bottom Color",color) = (1.0,1.0,1.0)
    }
    SubShader {
        Tags { "Queue" = "Transparent" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert alpha:fade 
		#pragma target 3.0

        sampler2D _MainTex;

        struct Input {
            float2 uv_MainTex;
			float4 color : COLOR;
			float3 objpos;
			float3 worldNormal;
      		float3 viewDir;
        };
		float4 _Color;
		float4 _Color2;
        void vert(inout appdata_full v, out Input o )
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            float amp = 0.15*sin(_Time*10 + v.vertex.x * 3) * (2 + 1 * sin(_Time + v.vertex.x * 0.01));
			float amp2 = 0.15 * sin(_Time*7.5 + v.vertex.x * 2)* (3 + 1.5 * sin(_Time + v.vertex.x * 0.01));
			float3 delta = v.normal;
            v.vertex.xyz = v.vertex.xyz + float3(0,lerp(amp,amp2,v.vertex.z),0); 
			o.objpos = v.vertex.xyz;
            //v.normal = normalize(float3(v.normal.x+offset_, v.normal.y, v.normal.z));
        }

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed2 uv = IN.uv_MainTex;
			fixed4 rimColor  = fixed4(0.5,0.7,0.5,1);
            o.Albedo = lerp(_Color,_Color2,uv.y);
            o.Alpha = 0.4 * (1 -4 * pow(0.5-uv.y,2));
			float rim = (1 - saturate(dot(IN.viewDir, o.Normal))) * 3;
     		o.Emission = lerp(_Color,_Color2,uv.y) * pow(rim, 2) * 2;
        }
        ENDCG
    }
}
