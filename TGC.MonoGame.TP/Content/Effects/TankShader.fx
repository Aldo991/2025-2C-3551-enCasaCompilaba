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

texture NormalTexture;
sampler NormalSampler = sampler_state
{
	Texture = (NormalTexture);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MINFILTER = LINEAR;
	MAGFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 InverseTransposeWorld;
float3 DiffuseColor;
float TreadmillsOffset = 0.0f;
float3 AmbientColor;
float3 SpecularColor;
float KAmbient;
float KDiffuse; 
float KSpecular;
float Shininess; 
float3 LightPosition;
float3 EyePosition;
// float2 Tiling;

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
	float4 WorldPosition: TEXCOORD1;
	float4 Normal : TEXCOORD2;
};

PS_INPUT VS_Main(VS_INPUT input)
{
	PS_INPUT output = (PS_INPUT)0;

	// Transformar vértices
	float4 worldPosition = mul(input.Position, World);
	output.Position = mul(worldPosition, View);
	output.Position = mul(output.Position, Projection);

	// Desplazamiento de textura
	output.TexCoord = input.TexCoord + float2(0, TreadmillsOffset);

	// Normales del tanque
	output.Normal = mul(float4(normalize(input.Normal.xyz), 1.0), InverseTransposeWorld);

	return output;
}

float4 PS_Main(PS_INPUT input) : COLOR
{
	// Vectores que inciden en el modelo
	float3 LightDirection = normalize(LightPosition - input.WorldPosition.xyz);
	float3 viewDirection = normalize(EyePosition - input.WorldPosition.xyz);
	float3 halfVector = normalize(LightDirection + viewDirection);
	float3 normal = normalize(input.Normal.xyz);

	// Textura
	float4 text = tex2D(TextureSampler, input.TexCoord);

	// Luz Difusa
	float NdotL = saturate(dot(normal ,LightDirection));
	float3 diffuseLight = KDiffuse * DiffuseColor * NdotL;

	// Luz Especular
	float NdotH = saturate(dot(normal, halfVector));
	float3 specularLight = KSpecular * SpecularColor * pow(saturate(NdotH), Shininess);

	float3 color = saturate(AmbientColor * KAmbient + diffuseLight) * text.rgb + specularLight;
	float4 ret = float4(color, text.a);

	return ret;
	// return tex2D(TextureSampler, input.TexCoord);
	// return float4(DiffuseColor, 1.0f);
}

technique BasicTechnique
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL VS_Main();
		PixelShader = compile PS_SHADERMODEL PS_Main();
	}
};
