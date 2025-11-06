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
    private const string ContentFolderHuds = "hud";
    private const string ContentFolder3D = "Models";
    private const string ContentFolderSounds = "Sounds/";
    private const string ContentFolderTextures = "Textures";
    public const string ContentFolderMusic = "Music/";
    public const string ContentFolderSpriteFonts = "SpriteFonts/";
    private const string ContentFolderBushes = "/bushes";
    private const string ContentFolderHouses = "/houses";
    private const string ContentFolderLands = "/land";
    private const string ContentFolderProjectiles = "/projectiles";
    private const string ContentFolderStones = "/stones";
    private const string ContentFolderTanks = "/tanks";
    private const string ContentFolderTrees = "/trees";
    private const string ContentFolderWalls = "/walls";

    private static string _rootDirectory;
    private static Model[] _bushModels;
    private static Model[] _houseModels;
    private static Model[] _landModels;
    private static Model[] _projectileModels;
    private static Model[] _stoneModels;
    private static Texture2D[] _stoneTextures;
    private static Model[] _tankModel;
    private static Texture2D[] _tankTextures;
    private static Model[] _treeModels;
    private static Model[] _wallModels;

    private static Texture2D[] _hudTextures;
    private static SpriteFont _spriteFont;
    private static Song _shootTank;

    public static void Load(ContentManager content)
    {
        var aux1 = Directory.GetCurrentDirectory();
        _rootDirectory = Directory.GetParent(aux1).Parent.Parent.FullName + "/Content/";

        // Cargo los modelos de arbustos
        LoadBushModels(content);

        // Cargo los modelos de casas
        LoadHouseModels(content);

        // Cargo los modelos de terrenos
        LoadLandModels(content);

        // Cargo los modelos de los proyectiles
        LoadProjectileModels(content);

        // Cargo los modelos de piedras
        LoadStonesTextures(content);
        LoadStoneModels(content);

        // Cargo los modelos de tanques
        LoadTankTextures(content);
        LoadTankModels(content);

        // Cargo los modelos de árboles
        LoadTreeModels(content);

        // Cargo los modelos de muros
        LoadWallModels(content);

        LoadSpriteFonts(content);
        LoadHudTextures(content);
        LoadSounds(content);
    }

    private static void LoadBushModels(ContentManager content)
    {
        var _paths = Directory.GetFiles(_rootDirectory + ContentFolder3D + ContentFolderBushes + "/", "*.fbx");
        _bushModels = new Model[_paths.Length];
        for (int i = 0; i < _paths.Length; i++)
        {
            var pathWithoutExtension = Path.GetFileNameWithoutExtension(_paths[i]);
            _bushModels[i] = content.Load<Model>(ContentFolder3D + ContentFolderBushes + "/" + pathWithoutExtension);
            Effect effect = content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in _bushModels[i].Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
            }
        }
    }
    private static void LoadHouseModels(ContentManager content)
    {
        var _paths = Directory.GetFiles(_rootDirectory + ContentFolder3D + ContentFolderHouses + "/", "*.fbx");
        _houseModels = new Model[_paths.Length];
        for (int i = 0; i < _paths.Length; i++)
        {
            var pathWithoutExtension = Path.GetFileNameWithoutExtension(_paths[i]);
            _houseModels[i] = content.Load<Model>(ContentFolder3D + ContentFolderHouses + "/" + pathWithoutExtension);
            Effect effect = content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in _houseModels[i].Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
            }
        }
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
        var paths = Directory.GetFiles(_rootDirectory + ContentFolder3D + ContentFolderProjectiles + "/", "*.fbx");
        _projectileModels = new Model[paths.Length];
        for (int i = 0; i < paths.Length; i++)
        {
            var pathWithoutExtension = Path.GetFileNameWithoutExtension(paths[i]);
            _projectileModels[i] = content.Load<Model>(ContentFolder3D + ContentFolderProjectiles + "/" + pathWithoutExtension);
            Effect effect = content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in _projectileModels[i].Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
            }
        }
    }
    private static void LoadStonesTextures(ContentManager content)
    {
        _stoneTextures = new Texture2D[1];
        _stoneTextures[0] = content.Load<Texture2D>("Textures/stones/Rocks011");
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
                    meshPart.Effect.Parameters["Texture"].SetValue(_stoneTextures[0]);
                }
            }
        }
    }
    private static void LoadTankModels(ContentManager content)
    {
        var _paths = Directory.GetFiles(_rootDirectory + ContentFolder3D + ContentFolderTanks + "/", "*.fbx");
        _tankModel = new Model[_paths.Length];
        Effect effect = content.Load<Effect>(ContentFolderEffects + "TankShader");
        for (int i = 0; i < _paths.Length; i++)
        {
            var pathWithoutExtension = Path.GetFileNameWithoutExtension(_paths[i]);
            var model = content.Load<Model>(ContentFolder3D + ContentFolderTanks + "/" + pathWithoutExtension);
            foreach (var mesh in model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = effect;
                }
            }
            _tankModel[i] = model;
        }
    }
    private static void LoadTankTextures(ContentManager content)
    {
        _tankTextures = new Texture2D[3];
        // _tankTextures[0] = content.Load<Texture2D>(ContentFolderTextures + ContentFolderTanks + "/");
        _tankTextures[0] = content.Load<Texture2D>("Textures/tanks/T90/hullA");
        _tankTextures[1] = content.Load<Texture2D>("Textures/tanks/T90/hullB");
        _tankTextures[2] = content.Load<Texture2D>("Textures/tanks/T90/hullC");
    }
    private static void LoadTreeModels(ContentManager content)
    {
        var _paths = Directory.GetFiles(_rootDirectory + ContentFolder3D + ContentFolderTrees + "/", "*.fbx");
        _treeModels = new Model[_paths.Length];
        for (int i = 0; i < _paths.Length; i++)
        {
            var pathWithoutExtension = Path.GetFileNameWithoutExtension(_paths[i]);
            _treeModels[i] = content.Load<Model>(ContentFolder3D + ContentFolderTrees + "/" + pathWithoutExtension);
            Effect effect = content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in _treeModels[i].Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
            }
        }
    }
    private static void LoadWallModels(ContentManager content)
    {
        var _paths = Directory.GetFiles(_rootDirectory + ContentFolder3D + ContentFolderWalls + "/", "*.fbx");
        _wallModels = new Model[_paths.Length];
        for (int i = 0; i < _paths.Length; i++)
        {
            var pathWithoutExtension = Path.GetFileNameWithoutExtension(_paths[i]);
            _wallModels[i] = content.Load<Model>(ContentFolder3D + ContentFolderWalls + "/" + pathWithoutExtension);
            Effect effect = content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in _wallModels[i].Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
            }
        }
    }
    private static void LoadSpriteFonts(ContentManager content)
    {
        _spriteFont = content.Load<SpriteFont>("hud/DefaultFont");
    }
    private static void LoadHudTextures(ContentManager content)
    {
        var _paths = Directory.GetFiles(_rootDirectory + ContentFolderHuds + "/", "*.png");
        _hudTextures = new Texture2D[_paths.Length];
        for (int i = 0; i < _paths.Length; i++)
        {
            var pathWithoutExtension = Path.GetFileNameWithoutExtension(_paths[i]);
            _hudTextures[i] = content.Load<Texture2D>(ContentFolderHuds + "/" + pathWithoutExtension);
        }
        // 0 es brújula, 1 health
    }
    private static void LoadSounds(ContentManager content)
    {
        string path = ContentFolderSounds + "shoot";
        _shootTank = content.Load<Song>(path);
    }
    public static Model GetModel(string modelName, int index)
    {
        return modelName switch
        {
            "house" => _houseModels[index],
            "bush" => _bushModels[index],
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
            "tank" => _tankTextures[index],
            "hud" => _hudTextures[index],
            _ => throw new ArgumentException("Invalid texture name"),
        };
    }
    public static SpriteFont GetSpriteFont()
    {
        return _spriteFont;
    }
    public static Song GetSoundEffect()
    {
        return _shootTank;
    }
}