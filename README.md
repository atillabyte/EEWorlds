<h1> Everybody Edits World Library <img src="https://img.shields.io/nuget/v/EEWorlds.svg?style=flat-square&label=nuget+(EEWorlds)"/> <h1>

<p align="center">
  <img width="460" height="300" src="https://i.imgur.com/SSU2oxl.png">
</p>

## Description
A library to handle loading Everybody Edits worlds from various formats. such as the following:
- TSON    (by miou)
- EELVL   (by LukeM)
- JSON    (by miou)
- EELEVEL (by cyph1e & capasha)

## Usage
```csharp
// TSON
var world = WorldManager.LoadFromTSON(File.ReadAllBytes("PW_Dc2Pqq8a0I.tson"));

// EEditor
var world = WorldManager.LoadFromEEditor(File.ReadAllBytes("PW1Ecl-na0I.eelevel"), EELevelVersion.V6);

// EELVL
var world = WorldManager.LoadFromEELVL(File.ReadAllBytes("PWXFk-UKg_b0I.eelvl"));

// You can retrieve the world properties.
var title = $"'{world.Name}' by {world.Owner} with {world.Plays} plays.";

// You can modify the world with BotBits.
if (world.At(x, y).Foreground.Id == Foreground.Basic.Red)
    world.At(x, y).Set(Foreground.Basic.Blue);

// You can generate a minimap of the world, too.
var bitmap = world.Minimap.GetBitmap();
```
