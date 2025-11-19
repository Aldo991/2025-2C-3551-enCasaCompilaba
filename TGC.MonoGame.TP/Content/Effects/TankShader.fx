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

float Time = 0;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float4 Normal : NORMAL0;

};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
	float4 WorldPosition : TEXCOORD1;
	float4 Normal : TEXCOORD2;

};

float3 getNormalFromMap(float2 textureCoordinates, float3 worldPosition, float3 worldNormal)
{
    float3 tangentNormal = tex2D(NormalSampler, textureCoordinates).xyz * 2.0 - 1.0;

    float3 Q1 = ddx(worldPosition);
    float3 Q2 = ddy(worldPosition);
    float2 st1 = ddx(textureCoordinates);
    float2 st2 = ddy(textureCoordinates);

    worldNormal = normalize(worldNormal.xyz);
    float3 T = normalize(Q1 * st2.y - Q2 * st1.y);
    float3 B = -normalize(cross(worldNormal, T));
    float3x3 TBN = float3x3(T, B, worldNormal);

    return normalize(mul(tangentNormal, TBN));
}

VertexShaderOutput MainVS(VertexShaderInput input)
{
	// Variable de salida
	VertexShaderOutput output = (VertexShaderOutput)0;
	// Vértice en espacio mundo
	float4 worldPosition = mul(input.Position, World);
	// Vértice en espacio vista
	float4 viewPosition = mul(worldPosition, View);
	// Vértice en espacio proyección
	output.Position = mul(viewPosition, Projection);
	// Propagación de la posición en el mundo
	output.WorldPosition = worldPosition;
	// Propagación de textura
	output.TexCoord = input.TexCoord + float2(0, TreadmillsOffset);

	
	// Normales del tanque
	output.Normal = mul(input.Normal, InverseTransposeWorld);
	// output.Normal = mul(float4(normalize(input.Normal.xyz), 1.0), InverseTransposeWorld);

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	// Vectores que inciden en el modelo
	float3 LightDirection = normalize(LightPosition - input.WorldPosition.xyz);
	float3 viewDirection = normalize(EyePosition - input.WorldPosition.xyz);
	float3 halfVector = normalize(LightDirection + viewDirection);
	// float3 normal = normalize(input.Normal.xyz);
	float3 normal = getNormalFromMap(input.TexCoord, input.WorldPosition.xyz, normalize(input.Normal.xyz));
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
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};