
/* 
reference: https://community.monogame.net/t/writing-your-own-2d-pixel-shader-in-monogame-for-absolute-beginners/10883
^ if anyone wants to get into shaders as well! 

https://www.youtube.com/watch?v=3eTO0CJWh1U this guy has a great tutorial walkthrouh as well
*/

// cross platform supprot for shader selection (OpenGL vs. DirectX)
#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture; // texture to be rendered

sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
    MipFilter = POINT;
    MinFilter = POINT;
    MagFilter = POINT;
};

float4 TintColor; // day night cycle color (RGBA)

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0; // passed thorugh entity draw
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // applies texture * tint * vertex color
    float4 texColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    return texColor * TintColor * input.Color;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};

