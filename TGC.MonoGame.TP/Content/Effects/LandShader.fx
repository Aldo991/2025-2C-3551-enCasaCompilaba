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
    float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 WorldPos : TEXCOORD0;
    float4 Normal : TEXCOORD1;
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
    output.Normal = mul(float4(normalize(input.Normal.xyz), 1.0), InverseTransposeWorld);

    return output;
}


float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Vectores que inciden en el modelo
	float3 LightDirection = normalize(LightPosition - input.WorldPos.xyz);
	float3 viewDirection = normalize(EyePosition - input.WorldPos.xyz);
	float3 halfVector = normalize(LightDirection + viewDirection);
	float3 normal = normalize(input.Normal.xyz);
	
	// Luz difusa
	float NdotL = saturate(dot(normal ,LightDirection));
	float3 diffuseLight = KDiffuse * DiffuseColor * NdotL;
	// Luz especular
	float NdotH = saturate(dot(normal, halfVector));
	float3 specularLight = KSpecular * SpecularColor * pow(saturate(NdotH), Shininess);

	float3 color = saturate(AmbientColor * KAmbient + diffuseLight) + specularLight;
	
	return float4(color, 1.0);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};