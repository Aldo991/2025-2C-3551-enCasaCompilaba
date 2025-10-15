#region File Description
//-----------------------------------------------------------------------------
// GameManager.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
/*
Conclusión: 
Todas las clases necesitan un atributo del tipo Model para cargar el modelo 3D,
GameManager las carga por una única vez, en vez de un LoadContent() en cada clase.
*/
#endregion

#region Using Statements
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using TGC.MonoGame.TP;
#endregion

// namespace TGC.MonoGame.TP;
/// <summary>
/// This is the main type for your game
/// </summary>
public class GameManager
{
    private static GameManager instance;
    private const string RootDirectory = "C:/Users/matil/OneDrive/Documentos/Repos/TGC/Nueva Carpeta/2025-2C-3551-enCasaCompilaba/TGC.MonoGame.TP/Content/";
    //private const string RootDirectory = "D:/GitHub_TGC/tgc-monogame-tp/TGC.MonoGame.TP/Content/";
    private const string ContentFolder3D = "Models";
    private const string ContentFolderTextures = "Textures";
    private const string ContentFolderBushes = "/bushes";
    private const string ContentFolderHouses = "/houses";
    private const string ContentFolderLands = "/land";
    private const string ContentFolderProjectiles = "/projectiles";
    private const string ContentFolderStones = "/stones";
    private const string ContentFolderTanks = "/tanks";
    // private const string ContentFolderHuds = "/hud";
    private const string ContentFolderTrees = "/trees";
    private const string ContentFolderWalls = "/walls";
    private const string ContentFolderEffects = "Effects/";

    private Model[] _bushModels;
    private Model[] _houseModels;
    private Model[] _landModels;
    private Model[] _projectileModels;
    private Model[] _stoneModels;
    private Texture2D[] _stoneTextures;
    private Model[] _tankModel;
    private Texture2D[] _tankTextures;
    private Model[] _treeModels;
    private Model[] _wallModels;
    private List<GameObject> _gameObjects;
    private bool _isPause;
    private bool _isPressingPause;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }
    public void Initialize()
    {
        _gameObjects = new List<GameObject>();
        _isPause = false;
    }
    public bool IsPause
    {
        get => _isPause;
        set => _isPause = value;
    }
    public bool IsPressingPause
    {
        get => _isPressingPause;
        set => _isPressingPause = value;
    }
    // LoadModels carga los modelos 3D
    public void LoadModels(ContentManager content)
    {
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
        LoadTankModels(content);
        LoadTankTextures(content);

        // Cargo los modelos de árboles
        LoadTreeModels(content);

        // Cargo los modelos de muros
        LoadWallModels(content);
    }

    public void LoadBushModels(ContentManager content)
    {
        if (_bushModels == null)
        {
            var _paths = Directory.GetFiles(RootDirectory + ContentFolder3D + ContentFolderBushes + "/", "*.fbx");
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
    }

    private void LoadHouseModels(ContentManager content)
    {
        if (_houseModels == null)
        {
            var _paths = Directory.GetFiles(RootDirectory + ContentFolder3D + ContentFolderHouses + "/", "*.fbx");
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
    }

    private void LoadLandModels(ContentManager content)
    {
        if (_landModels == null)
        {
            var _paths = Directory.GetFiles(RootDirectory + ContentFolder3D + ContentFolderLands + "/", "*.fbx");
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
    }

    private void LoadProjectileModels(ContentManager content)
    {
        if (_projectileModels == null)
        {
            var paths = Directory.GetFiles(RootDirectory + ContentFolder3D + ContentFolderProjectiles + "/", "*.fbx");
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
    }

    private void LoadStonesTextures(ContentManager content)
    {
        _stoneTextures = new Texture2D[1];
        _stoneTextures[0] = content.Load<Texture2D>("Textures/stones/Rocks011");
    }


    private void LoadStoneModels(ContentManager content)
    {
        if (_stoneModels == null)
        {
            var _paths = Directory.GetFiles(RootDirectory + ContentFolder3D + ContentFolderStones + "/", "*.fbx");
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
    }

    private void LoadTankModels(ContentManager content)
    {
        if (_tankModel == null)
        {
            var _paths = Directory.GetFiles(RootDirectory + ContentFolder3D + ContentFolderTanks + "/", "*.fbx");
            _tankModel = new Model[_paths.Length];
            for (int i = 0; i < _paths.Length; i++)
            {
                var pathWithoutExtension = Path.GetFileNameWithoutExtension(_paths[i]);
                _tankModel[i] = content.Load<Model>(ContentFolder3D + ContentFolderTanks + "/" + pathWithoutExtension);
                Effect effect = content.Load<Effect>(ContentFolderEffects + "TankShader");
                foreach (var mesh in _tankModel[i].Meshes)
                {
                    foreach (var meshPart in mesh.MeshParts)
                        meshPart.Effect = effect;
                }
            }
        }
    }

    private void LoadTankTextures(ContentManager content)
    {
        _tankTextures = new Texture2D[3];
        // _tankTextures[0] = content.Load<Texture2D>(ContentFolderTextures + ContentFolderTanks + "/");
        _tankTextures[0] = content.Load<Texture2D>("Textures/tanks/T90/hullA");
        _tankTextures[1] = content.Load<Texture2D>("Textures/tanks/T90/hullB");
        _tankTextures[2] = content.Load<Texture2D>("Textures/tanks/T90/hullC");
    }

    private void LoadTreeModels(ContentManager content)
    {
        if (_treeModels == null)
        {
            var _paths = Directory.GetFiles(RootDirectory + ContentFolder3D + ContentFolderTrees + "/", "*.fbx");
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
    }

    private void LoadWallModels(ContentManager content)
    {
        if (_wallModels == null)
        {
            var _paths = Directory.GetFiles(RootDirectory + ContentFolder3D + ContentFolderWalls + "/", "*.fbx");
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
    }

    public Model GetModel(string modelName, int index)
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
    public Texture2D GetTexture(string modelName, int index)
    {
        return modelName switch
        {
            "tank" => _tankTextures[index],
            //_hudModels => _hudModels[index],
            _ => throw new ArgumentException("Invalid texture name"),
        };
    }

    public void ApplyTextureToModel(Model model, Texture2D texture)
    {
        foreach (var mesh in model.Meshes)
        {
            foreach (var effect in mesh.Effects)
            {
                effect.Parameters["Texture"].SetValue(texture);
            }
        }
    }
}