sampler2D SpriteTexture : register(s0);
float4 TintColor; // Set from code

float4 PixelShaderFunction(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 texColor = tex2D(SpriteTexture, texCoord);
    // Multiply the texture color by the TintColor and vertex color.
    return texColor * TintColor * color;
}

technique Technique1
{
    pass P0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
