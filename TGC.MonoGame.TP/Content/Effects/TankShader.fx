#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

texture Texture;
sampler TextureSampler = sampler_state
{
	Texture = <Texture>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

float4x4 World;
float4x4 View;
float4x4 Projection;
float3 DiffuseColor;
float ScrollOffset = 0.0f;

struct VS_INPUT
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float3 Normal : NORMAL0;
};

struct PS_INPUT
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
};

PS_INPUT VS_Main(VS_INPUT input)
{
	PS_INPUT output;

	// Transformar vértices
	float4 worldPosition = mul(input.Position, World);
	output.Position = mul(worldPosition, View);
	output.Position = mul(output.Position, Projection);

	// ✅ aplicar desplazamiento de textura
	output.TexCoord = input.TexCoord + float2(ScrollOffset, 0);

	return output;
}

float4 PS_Main(PS_INPUT input) : COLOR
{
	return tex2D(TextureSampler, input.TexCoord);
}

technique BasicTechnique
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL VS_Main();
		PixelShader = compile PS_SHADERMODEL PS_Main();
	}
};

// Versión simple sin textura (solo color)
struct VertexShaderInput
{
	float4 Position : POSITION0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	return output;
}

