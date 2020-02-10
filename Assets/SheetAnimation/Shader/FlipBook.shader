Shader "Sprites/FlipBook"
{
    Properties
    {
        [Header(Texture Sheet)]
		// [PerRendererData]
        _MainTex ("Texture", 2D) = "white" { }
        [Header(Settings)]
        _ColumnsX ("Columns (X)", int) = 1
        _RowsY ("Rows (Y)", int) = 1
        _TotalFrame ("Total Frame", int) = 1
        _AnimationSpeed ("Frames Per Seconds", float) = 1
    }
    SubShader
    {
        Tags { 
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			"PreviewType" = "Plane" 
			"RenderType" = "Transparent" 
			"DisableBatching" = "True" 
		}
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        LOD 100
        
        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv: TEXCOORD0;
                float4 vertex: SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            uint _ColumnsX;
            uint _RowsY;
            uint _TotalFrame;
            float _AnimationSpeed;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                // get single sprite size
                float2 size = float2(1.0f / _ColumnsX, 1.0f / _RowsY);
                
                // use timer to increment index
                //float4 _Time : Time (t/20, t, t*2, t*3)
                uint index = (_Time.y * _AnimationSpeed) % _TotalFrame;
                
                // wrap x and y indexes
                uint indexX = index % _ColumnsX;
                uint indexY = index / _ColumnsX;
                
                // get offsets to our sprite index
                float2 offset = float2(size.x * indexX, -size.y * indexY);
                
                // get single sprite UV
                float2 newUV = v.uv * size;
                
                // flip Y (to start 0 from top)
                newUV.y = newUV.y + size.y * (_RowsY - 1); // - (size.y * indexY) // thats why offset y is negative

                o.uv = newUV + offset;
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
            
        }
    }
}
