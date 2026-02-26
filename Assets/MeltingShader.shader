Shader "Custom/MeltingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Albedo Color", Color) = (1,1,1,1)

        _EmissionColor ("Emission Color", Color) = (0,0,0,1)

        _MeltAmount ("Melt Amount", Range(0,1)) = 0
        _NoiseScale ("Noise Scale", Float) = 5
        _MeltStrength ("Melt Strength", Float) = 0.5

        _DripStrength ("Drip Strength", Float) = 0.6
        _DripScale ("Drip Scale", Float) = 3
        _DripSpeed ("Drip Speed", Float) = 1

        _UVFlowStrength ("UV Flow", Float) = 0.2
        _WetSmoothness ("Wet Boost", Range(0,1)) = 0.5

        _MeltTint ("Melt Tint Color", Color) = (0.2,0.2,0.2,1)
        _TintStrength ("Tint Strength", Range(0,1)) = 0.7
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;

        float _MeltAmount;
        float _NoiseScale;
        float _MeltStrength;

        float _DripStrength;
        float _DripScale;
        float _DripSpeed;

        float _UVFlowStrength;
        float _WetSmoothness;

        float4 _Color;
        fixed4 _EmissionColor;

        fixed4 _MeltTint;
        float _TintStrength;

        struct Input
        {
            float2 uv_MainTex;
            float meltMask;
        };

        // ---------- NOISE ----------
        float hash(float2 p)
        {
            return frac(sin(dot(p, float2(127.1,311.7))) * 43758.5453);
        }

        float noise(float2 p)
        {
            float2 i = floor(p);
            float2 f = frac(p);

            float a = hash(i);
            float b = hash(i + float2(1,0));
            float c = hash(i + float2(0,1));
            float d = hash(i + float2(1,1));

            float2 u = f*f*(3.0-2.0*f);

            return lerp(a,b,u.x) +
                   (c-a)*u.y*(1.0-u.x) +
                   (d-b)*u.x*u.y;
        }

        // ---------- VERTEX STAGE ----------
        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input,o);

            // posición en mundo
            float4 worldPos4 = mul(unity_ObjectToWorld, v.vertex);
            float3 worldPos = worldPos4.xyz;

            float3 gravityDir = normalize(float3(0,-1,0));

            // ruido espacial en global
            float n = noise(worldPos.xz * _NoiseScale);

            // Altura relativa
            float3 objectOrigin = unity_ObjectToWorld._m03_m13_m23;

            // Máscara de altura por gravedad
            float heightAlongGravity =
                dot(worldPos - objectOrigin, -gravityDir);

            float heightMask = saturate(heightAlongGravity);

            float melt = n * _MeltAmount * _MeltStrength * heightMask;

            float dripNoise = noise(worldPos.xz * _DripScale + _Time.y * _DripSpeed);
            dripNoise = saturate(dripNoise - 0.6) * 2.5;
                        float drip =
                pow(dripNoise,2.5) *
                _DripStrength *
                _MeltAmount *
                heightMask;

            melt = melt + drip;

           // mover vértices en dirección elegida
            worldPos += gravityDir * melt;

            // transformar en de nuevo a object space
            float4 newLocal =
                mul(unity_WorldToObject, float4(worldPos,1));

            v.vertex = newLocal;

            // guardamos cuánto se ha derretido para el fragment shader
            o.meltMask = melt;
        }

        // ---------- SURFACE ----------
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float meltFactor = saturate(IN.meltMask * 5);

            float2 uv = IN.uv_MainTex;

            // UV FLOW
            uv.y -= meltFactor * _UVFlowStrength * _MeltAmount;

            // Albedo
            fixed4 texAlbedo = tex2D(_MainTex, uv);
            if(texAlbedo.a == 0) texAlbedo = fixed4(1,1,1,1); // fallback blanco si no hay textura
            o.Albedo = texAlbedo.rgb * _Color.rgb;
            // aplicar tint de melting
            o.Albedo = lerp(o.Albedo, o.Albedo * _MeltTint.rgb, meltFactor*_TintStrength);

            o.Alpha = texAlbedo.a;
        }

        ENDCG
    }
    FallBack "Standard"
}