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

float3 AmbientColor;
float3 SpecularColor;
float KAmbient;
float KDiffuse; 
float KSpecular;
float Shininess; 
float3 LightPosition;
float3 EyePosition;

float Time = 0;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float3 Normal : NORMAL0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
	float4 WorldPosition : TEXCOORD1;
	float4 Normal : TEXCOORD2;
    float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Variable de salida
	VertexShaderOutput output = (VertexShaderOutput)0;
    // Vértice en espacio mundo
    float4 worldPosition = mul(input.Position, World);
    // Vértice en espacio vista
    float4 viewPosition = mul(worldPosition, View);	
	// Vértice en espacio proyección
    output.Position = mul(viewPosition, Projection);
	// Propagación de textura
	output.TexCoord = input.TexCoord;
	// Propagación del color??? esto sirve??
    output.Color = input.Color;
	// Normales del tanque
	output.Normal = mul(float4(normalize(input.Normal.xyz), 1.0), InverseTransposeWorld);

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	// Vectores que inciden en el modelo
	float3 LightDirection = normalize(LightPosition - input.WorldPosition.xyz);
	float3 viewDirection = normalize(EyePosition - input.WorldPosition.xyz);
	float3 halfVector = normalize(LightDirection + viewDirection);
	float3 normal = normalize(input.Normal.xyz);
	// Textura
	float4 text = tex2D(TextureSampler, input.TexCoord);
	// Luz difusa
	float NdotL = saturate(dot(normal ,LightDirection));
	float3 diffuseLight = KDiffuse * DiffuseColor * NdotL;
	// Luz especular
	float NdotH = saturate(dot(normal, halfVector));
	float3 specularLight = KSpecular * SpecularColor * pow(saturate(NdotH), Shininess);

	float3 color = saturate(AmbientColor * KAmbient + diffuseLight) * text.rgb + specularLight;
	float4 ret = float4(color, text.a);

    return ret;
    // return input.Color;
	// return float4(DiffuseColor, 1.0);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};