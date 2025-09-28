#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Custom Effects - https://docs.monogame.net/articles/content/custom_effects.html
// High-level shader language (HLSL) - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl
// Programming guide for HLSL - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-pguide
// Reference for HLSL - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-reference
// HLSL Semantics - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-semantics

float4x4 World;
float4x4 View;
float4x4 Projection;

uniform texture ModelTexture;
sampler2D TextureSampler = sampler_state
{
	Texture = <ModelTexture>;
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	AddressU = WRAP;
	AddressV = WRAP;
};
//float3 DiffuseColor;

float Time = 0;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TextureCoodinates : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 WorldPos : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;
    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);	
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);

	output.WorldPos = worldPosition;

    return output;
}


float4 MainPS(VertexShaderOutput input) : COLOR
{
	float3 BottomColor = float3(0.4, 0.3, 0.1);; // marrón tierra
	float3 TopColor    = float3(0.05, 0.3, 0.05); // verde pasto	
	
	//smoothstep(edge0, edge1, x)//edge0:valor donde empieza la transición (factor = 0), edge1: valor donde termina la transición (factor = 1) , x:valor que queremos normalizar (en tu caso
	float factor = smoothstep(-490.0, 100.0, input.WorldPos.y); 

	//lerp(a, b, t) a: valor inicial, b: valor final, t factor de interpolacion
	float3 color = lerp(BottomColor, TopColor, factor);
    return float4(color, 1);

}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};