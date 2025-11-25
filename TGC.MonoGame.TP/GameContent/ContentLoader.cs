#region File Description
#endregion

#region Using Statements
using System;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
#endregion

/// Cargar los modelos, texturas, sprites, sonidos, etc. que se van
/// a usar durante la ejecución del programa
public static class ContentLoader
{
    // Folders
    private const string ContentFolderEffects = "Effects/";
    private const string ContentFolder3D = "Models";
    public const string ContentFolderMusic = "Music/";
    public const string ContentFolderSpriteFonts = "SpriteFonts/";
    private const string ContentFolderBushes = "/bushes";
    private const string ContentFolderLands = "/land";
    private const string ContentFolderStones = "/stones";
    private const string ContentFolderTrees = "/trees";
    private const string ContentFolderWalls = "/walls";

    private static string _rootDirectory;
    private static Model[] _bushModels;
    private static Texture2D[] _bushTextures;
    private static Model[] _houseModels;
    private static Texture2D[] _houseTextures;
    private static Texture2D[] _houseNormals;
    private static Model[] _landModels;
    private static Model[] _projectileModels;
    private static Texture2D[] _projectileTextures;
    private static Texture2D[] _projectileNormals;
    private static Model[] _stoneModels;
    private static Texture2D[] _stoneTextures;
    private static Model[] _tankModel;
    private static Texture2D[] _tankTextures;
    private static Texture2D[] _tankTreadmillsTextures;
    private static Texture2D[] _tankNormals;
    private static Texture2D[] _tankTreadmillsNormals;
    private static Model[] _treeModels;
    private static Texture2D[] _treeTextures;
    private static Model[] _wallModels;
    private static Texture2D[] _wallTextures;
    private static Texture2D[] _hudTextures;
    private static SpriteFont _spriteFont;
    private static Song[] _music;
    private static SoundEffect[] _soundEffects;
    private static Texture2D[] _heightmapTerrain;
    private static Effect _terrainEffect;
    
    public static void Load(ContentManager content)
    {
        var aux1 = Directory.GetCurrentDirectory();
        _rootDirectory = Directory.GetParent(aux1).Parent.Parent.FullName + "/Content/";
        // LoadSong(content);

        // Cargo los modelos de arbustos
        LoadBushModels(content);
        LoadBushTextures(content);

        // Cargo los modelos de casas
        LoadHouseModels(content);
        LoadHouseTextures(content);
        LoadHouseNormals(content);

        // Cargo los modelos de terrenos
        LoadLandModels(content);

        // Cargo los modelos de los proyectiles
        LoadProjectileModels(content);
        LoadProjectileTextures(content);
        LoadProjectileNormals(content);

        // Cargo los modelos de piedras
        LoadStonesTextures(content);
        LoadStoneModels(content);

        // Cargo los modelos de tanques
        LoadTankTextures(content);
        LoadTankModels(content);
        LoadTankNormals(content);

        // Cargo los modelos de árboles
        LoadTreeModels(content);
        LoadTreeTextures(content);

        // Cargo los modelos de muros
        LoadWallModels(content);
        LoadWallTextures(content);
        

        LoadSpriteFonts(content);
        LoadHudTextures(content);
        LoadSoundEffects(content);
        LoadMusic(content);

        LoadHeightmapTerrain(content);
    }
    private static void LoadBushModels(ContentManager content)
    {
        var _paths = Directory.GetFiles(_rootDirectory + ContentFolder3D + ContentFolderBushes + "/", "*.fbx");
        _bushModels = new Model[_paths.Length];
        for (int i = 0; i < _paths.Length; i++)
        {
            var pathWithoutExtension = Path.GetFileNameWithoutExtension(_paths[i]);
            _bushModels[i] = content.Load<Model>(ContentFolder3D + ContentFolderBushes + "/" + pathWithoutExtension);
            Effect effect = content.Load<Effect>(ContentFolderEffects + "BushShader");
            foreach (var mesh in _bushModels[i].Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
            }
        }
    }
    private static void LoadHouseModels(ContentManager content)
    {
        _houseModels = new Model[1];
        Effect effect = content.Load<Effect>("Effects/HouseShader");
        _houseModels[0] = content.Load<Model>("Models/houses/house0");
        foreach(var model in _houseModels)
            LoadEffectOnModel(model, effect);
    }
    private static void LoadHouseTextures(ContentManager content)
    {
        _houseTextures = new Texture2D[1];
        _houseTextures[0] = content.Load<Texture2D>("Textures/houses/house0");
    }
    private static void LoadHouseNormals(ContentManager content)
    {
        _houseNormals = new Texture2D[1];
        _houseNormals[0] = content.Load<Texture2D>("Textures/houses/house0-normal");
    }
    private static void LoadLandModels(ContentManager content)
    {
        var _paths = Directory.GetFiles(_rootDirectory + ContentFolder3D + ContentFolderLands + "/", "*.fbx");
        _landModels = new Model[_paths.Length];
        for (int i = 0; i < _paths.Length; i++)
        {
            var pathWithoutExtension = Path.GetFileNameWithoutExtension(_paths[i]);
            _landModels[i] = content.Load<Model>(ContentFolder3D + ContentFolderLands + "/" + pathWithoutExtension);
            Effect effect = content.Load<Effect>(ContentFolderEffects + "LandShader");
            foreach (var mesh in _landModels[i].Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
            }
        }
    }
    private static void LoadProjectileModels(ContentManager content)
    {
        _projectileModels = new Model[1];
        _projectileModels[0] = content.Load<Model>("Models/projectiles/projectile0");
        Effect effect = content.Load<Effect>("Effects/ProjectileShader");
        foreach(var model in _projectileModels)
            LoadEffectOnModel(model, effect);
    }
    private static void LoadProjectileTextures(ContentManager content)
    {
        _projectileTextures = new Texture2D[1];
        _projectileTextures[0] = content.Load<Texture2D>("Textures/projectiles/projectile0-base-color");
    }
    private static void LoadProjectileNormals(ContentManager content)
    {
        _projectileNormals = new Texture2D[1];
        _projectileNormals[0] = content.Load<Texture2D>("Textures/projectiles/projectile0-normal");
    }
    private static void LoadStonesTextures(ContentManager content)
    {
        _stoneTextures = new Texture2D[4];
        _stoneTextures[0] = content.Load<Texture2D>("Textures/stones/stone0");
        _stoneTextures[1] = content.Load<Texture2D>("Textures/stones/stone1");
        _stoneTextures[2] = content.Load<Texture2D>("Textures/stones/stone2");
        // _stoneTextures[3] = content.Load<Texture2D>("Textures/stones/stone3");
        // _stoneTextures[4] = content.Load<Texture2D>("Textures/stones/stone4");
        // _stoneTextures[5] = content.Load<Texture2D>("Textures/stones/stone5");
    }
    private static void LoadStoneModels(ContentManager content)
    {
        var _paths = Directory.GetFiles(_rootDirectory + ContentFolder3D + ContentFolderStones + "/", "*.fbx");
        _stoneModels = new Model[_paths.Length];
        for (int i = 0; i < _paths.Length; i++)
        {
            var pathWithoutExtension = Path.GetFileNameWithoutExtension(_paths[i]);
            _stoneModels[i] = content.Load<Model>(ContentFolder3D + ContentFolderStones + "/" + pathWithoutExtension);
            Effect effect = content.Load<Effect>(ContentFolderEffects + "StoneShader");
            foreach (var mesh in _stoneModels[i].Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = effect;
                }
            }
        }
    }
    private static void LoadTankModels(ContentManager content)
    {
        _tankModel = new Model[1];
        _tankModel[0] = content.Load<Model>("Models/tanks/T90");
        Effect effect = content.Load<Effect>("Effects/TankShader");
        foreach(Model model in _tankModel)
            LoadEffectOnModel(model, effect);
    }
    private static void LoadTankTextures(ContentManager content)
    {
        _tankTextures = new Texture2D[3];
        _tankTextures[0] = content.Load<Texture2D>("Textures/tanks/T90/hullA");
        _tankTextures[1] = content.Load<Texture2D>("Textures/tanks/T90/hullB");
        _tankTextures[2] = content.Load<Texture2D>("Textures/tanks/T90/hullC");

        _tankTreadmillsTextures = new Texture2D[1];
        _tankTreadmillsTextures[0] = content.Load<Texture2D>("Textures/tanks/T90/treadmills");
    }
    private static void LoadTankNormals(ContentManager content)
    {
        _tankNormals = new Texture2D[1];
        _tankNormals[0] = content.Load<Texture2D>("Textures/tanks/T90/normal");

        _tankTreadmillsNormals = new Texture2D[1];
        _tankTreadmillsNormals[0] = content.Load<Texture2D>("Textures/tanks/T90/treadmills_normal");
    }
    private static void LoadTreeModels(ContentManager content)
    {
        var _paths = Directory.GetFiles(_rootDirectory + ContentFolder3D + ContentFolderTrees + "/", "*.fbx");
        _treeModels = new Model[_paths.Length];
        for (int i = 0; i < _paths.Length; i++)
        {
            var pathWithoutExtension = Path.GetFileNameWithoutExtension(_paths[i]);
            _treeModels[i] = content.Load<Model>(ContentFolder3D + ContentFolderTrees + "/" + pathWithoutExtension);
            Effect effect = content.Load<Effect>(ContentFolderEffects + "TreeShader");
            foreach (var mesh in _treeModels[i].Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
            }
        }
    }
    private static void LoadTreeTextures(ContentManager content)
    {
        _treeTextures = new Texture2D[1];
        _treeTextures[0] = content.Load<Texture2D>("Textures/trees/tree0");
    }
    private static void LoadWallModels(ContentManager content)
    {
        var _paths = Directory.GetFiles(_rootDirectory + ContentFolder3D + ContentFolderWalls + "/", "*.fbx");
        _wallModels = new Model[_paths.Length];
        for (int i = 0; i < _paths.Length; i++)
        {
            var pathWithoutExtension = Path.GetFileNameWithoutExtension(_paths[i]);
            _wallModels[i] = content.Load<Model>(ContentFolder3D + ContentFolderWalls + "/" + pathWithoutExtension);
            Effect effect = content.Load<Effect>(ContentFolderEffects + "WallShader");
            foreach (var mesh in _wallModels[i].Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
            }
        }
    }
    private static void LoadWallTextures(ContentManager content)
         {
             _wallTextures = new Texture2D[1];
             _wallTextures[0] = content.Load<Texture2D>("Textures/walls/brickwall_4");
         }
    private static void LoadBushTextures(ContentManager content)
    {
        _bushTextures = new Texture2D[1];
        _bushTextures[0] = content.Load<Texture2D>("Textures/bushes/hojas");
    }
    private static void LoadSpriteFonts(ContentManager content)
    {
        _spriteFont = content.Load<SpriteFont>("hud/DefaultFont");
    }
    private static void LoadHudTextures(ContentManager content)
    {
        _hudTextures = new Texture2D[3];
        _hudTextures[0] = content.Load<Texture2D>("hud/Brujula");
        _hudTextures[1] = content.Load<Texture2D>("hud/health");
        _hudTextures[2] = content.Load<Texture2D>("hud/red-arrow");
        // 0 es brújula, 1 health, 2 es flecha
    }
    private static void LoadSoundEffects(ContentManager content)
    {
        _soundEffects = new SoundEffect[6];
        _soundEffects[0] = content.Load<SoundEffect>("Sounds/dead");
        _soundEffects[1] = content.Load<SoundEffect>("Sounds/metal-hit");
        _soundEffects[2] = content.Load<SoundEffect>("Sounds/metal-hit2");
        _soundEffects[3] = content.Load<SoundEffect>("Sounds/metal-hit3");
        _soundEffects[4] = content.Load<SoundEffect>("Sounds/shoot");
        _soundEffects[5] = content.Load<SoundEffect>("Sounds/victory");
    }
    private static void LoadMusic(ContentManager content)
    {
        _music = new Song[4];
        _music[0] = content.Load<Song>("Music/defeat");
        _music[1] = content.Load<Song>("Music/defeat2");
        _music[2] = content.Load<Song>("Music/victory");
        _music[3] = content.Load<Song>("Music/war-background");
    }
    private static void LoadHeightmapTerrain(ContentManager content)
    {
        _heightmapTerrain = new Texture2D[4];
        _heightmapTerrain[0] = content.Load<Texture2D>("Terrain/heightmap-terrain");
        _heightmapTerrain[1] = content.Load<Texture2D>("Terrain/heightmap-color");
        _heightmapTerrain[2] = content.Load<Texture2D>("Terrain/ground");
        _heightmapTerrain[3] = content.Load<Texture2D>("Terrain/grass");
        _terrainEffect = content.Load<Effect>("Effects/Terrain");
    }
    private static void LoadEffectOnModel(Model model, Effect effect)
    {
        foreach (var mesh in model.Meshes)
        {
            foreach (var meshPart in mesh.MeshParts)
                meshPart.Effect = effect;
        }
    }
    public static Model GetModel(string modelName, int index)
    {
        return modelName switch
        {
            "bush" => _bushModels[index],
            "house" => _houseModels[index],
            "land" => _landModels[index],
            "projectile" => _projectileModels[index],
            "stone" => _stoneModels[index],
            "tank" => _tankModel[index],
            "tree" => _treeModels[index],
            "wall" => _wallModels[index],
            //_hudModels => _hudModels[index],
            _ => throw new ArgumentException("Invalid model name"),
        };
    }
    public static Texture2D GetTexture(string modelName, int index)
    {
        return modelName switch
        {
            "bush" => _bushTextures[index],
            "house" => _houseTextures[index],
            "hud" => _hudTextures[index],
            "projectile" => _projectileTextures[index],
            "stone" => _stoneTextures[index],
            "tank" => _tankTextures[index],
            "tank-treadmills" => _tankTreadmillsTextures[index],
            "tree" => _treeTextures[index],
            "wall" => _wallTextures[index],
            _ => throw new ArgumentException("Invalid texture name"),
        };
    }
    public static Texture2D GetNormal(string modelName, int index)
    {
        return modelName switch
        {
            "house" => _houseNormals[index],
            "projectile" => _projectileNormals[index],
            "tank" => _tankNormals[index],
            "tank-treadmills" => _tankTreadmillsNormals[index],
            _ => throw new ArgumentException("Invalid Texture Name"),
        };
    }
    public static SpriteFont GetSpriteFont()
    {
        return _spriteFont;
    }
    public static SoundEffect GetSoundEffect(string sound)
    {
        return sound switch
        {
            "dead" => _soundEffects[0],
            "metal-hit" => _soundEffects[1],
            "metal-hit2" => _soundEffects[2],
            "metal-hit3" => _soundEffects[3],
            "shoot" => _soundEffects[4],
            "victory" => _soundEffects[5],
            _ => throw new ArgumentException("Invalid Sound Name"),
        };
    }
    public static Song GetMusic(string music)
    {
        return music switch
        {
            "defeat" => _music[0],
            "defeat2" => _music[1],
            "victory" => _music[2],
            "war-background" => _music[3],
            _ => throw new ArgumentException("Invalid Music Name")
        };
    }
    public static Texture2D GetTerrain(string part)
    {
        return part switch
        {
            "heightmap" => _heightmapTerrain[0],
            "heightmap-color" => _heightmapTerrain[1],
            "ground" => _heightmapTerrain[2],
            "grass" => _heightmapTerrain[3],
            _ => throw new ArgumentException("Invalid Texture Name"),
        };
    }
    public static Effect GetTerrainEffect()
    {
        return _terrainEffect;
    }
}