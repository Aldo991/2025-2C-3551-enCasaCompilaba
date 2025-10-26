# Copilot Instructions for TGC MonoGame TP

## Project Overview
- This is a MonoGame/.NET Core project for the TGC (Computer Graphics Techniques) course at UTN-FRBA.
- Main game logic is in `TGC.MonoGame.TP/`.
- Game assets (models, textures, shaders) are under `TGC.MonoGame.TP/Content/` and subfolders.
- The solution file is `TGC.MonoGame.TP.sln`.

## Architecture & Key Components
- Entry point: `Program.cs`.
- Main game loop and logic: `TGCGame.cs`.
- Game objects: `GameContent/Objects/` (e.g., `Tank.cs`, `Tree.cs`, `Wall.cs`).
- Managers: `GameContent/ObjectManagers/` (e.g., `TankManager.cs`).
- Camera and HUD: `GameContent/FollowCamera.cs`, `GameContent/HUDModel.cs`.
- Shaders: `Content/Effects/*.fx`.

## Build & Run
- Build with Visual Studio or `dotnet build TGC.MonoGame.TP.sln`.
- Run with `dotnet run --project TGC.MonoGame.TP/TGC.MonoGame.TP.csproj`.
- Shaders may require Windows or Wine for compilation (see README).
- Large assets use Git LFS; ensure `git lfs pull` after cloning.

## Project-Specific Patterns
- Game objects inherit from `GameObject.cs`.
- Managers handle collections and logic for each object type.
- Shaders are written in HLSL (`*.fx`) and loaded via MonoGame pipeline.
- Content pipeline is managed via `Content.mgcb`.
- Asset paths are relative to `Content/`.

## External Integrations
- Uses MonoGame and .NET Core (see `TGC.MonoGame.TP.csproj`).
- GitHub Actions for CI: see `.github/workflows/`.
- Git LFS for large binary assets.

## Conventions
- Spanish is used for comments and documentation.
- Folder structure mirrors game logic and asset types.
- Prefer extending base classes for new game objects.
- Place new shaders in `Content/Effects/` and update pipeline as needed.

## Examples
- To add a new game object: create a class in `GameContent/Objects/`, inherit from `GameObject`, and register it in the relevant manager.
- To add a new shader: place `.fx` file in `Content/Effects/`, reference it in code, and ensure it's built via the content pipeline.

---
For more details, see `README.md` and comments in source files. If any section is unclear or missing, please provide feedback to improve these instructions.
