# Project Arrhythmia Prefab Toolkit
 A library for modifying and creating [Project Arrhythmia](https://store.steampowered.com/app/440310/Project_Arrhythmia/) prefabs.

# Important!
This library is finished, but it most likely bugs.

If you encounter any bugs/mistakes or you have a feature request, please open an issue.

If you want to implement a feature, feel free to open a pull request.

## Installing the library (VS 2019)

 1. Right click your project in the Solution explorer
 2. Click "Manage NuGet packages"
 3. Search "PAPrefabToolkit" and select the first result
 4. Click "Install"

# Using the library
## Creating a prefab
It is recommended to use the constructor that takes in a `string name` and a `PrefabType type`.
```csharp
Prefab myPrefab = new Prefab("Hello Prefab Toolkit!", PrefabType.Misc1);
```
## Creating an object
To create an object, call `prefab.CreateObject(name);`. This returns an object which can be modified.
```csharp
Prefab myPrefab = new Prefab("Hello Prefab Toolkit!", PrefabType.Misc1);
PrefabObject myObject = myPrefab.CreateObject("Hello Prefab Object!");
```
## Modifying the object
Setting the object's name
```csharp
myObject.Name = "Hello world!";
```
Toggling position/scale/rotation parenting
```csharp
myObject.PositionParenting = true;
myObject.ScaleParenting = false;
myObject.RotationParenting = true;
```
Similarly for parent offset
```csharp
myObject.PositionParentOffset = true;
myObject.ScaleParentOffset = false;
myObject.RotationParentOffset = true;
```
Changing render depth
```csharp
myObject.RenderDepth = 5;
```
Changing object type
```csharp
myObject.ObjectType = PrefabObjectType.Helper;
```
Changing object shape
```csharp
myObject.Shape = PrefabObjectShape.Circle;
```
Changing object sub-shape
```csharp
myObject.ShapeOption = (int)PrefabCircleOption.HalfHollow;
```
Changing object text
```csharp
myObject.Text = "The quick brown fox jumps over the lazy dog."; // Note: This will be ignored unless your object shape is Text.
```
Changing object auto kill type
```csharp
myObject.AutoKillType = PrefabObjectAutoKillType.Fixed;
```
Changing object auto kill offset
```csharp
myObject.AutoKillOffset = 5.0f;
```
Changing object origin
```csharp
myObject.Origin = new Vector2(0.5f, 0.5f);
```
Changing editor locked/collapsed state and/or bin/layer
```csharp
myObject.EditorLocked = false;
myObject.EditorCollapse = true;
myObject.EditorBin = 1;
myObject.EditorLayer = 0;
```
## Animating the object
There are four lists of different keyframe types inside the object. Each list is empty initially. You can add keyframes to those list to animate them. Note that there should be at least one keyframe per list when finished.
```csharp
myObject.PositionKeyframes.Add(new PositionKeyframe());
myObject.ScaleKeyframes.Add(new ScaleKeyframe() { Value = Vector2.One });
myObject.RotationKeyframes.Add(new RotationKeyframe() { Value = 0.0f });
myObject.ColorKeyframes.Add(new ColorKeyframe() { Value = 0 });
```
## Building the prefab
You can either get a JSON object representing the prefab or export it to a file. Note that if the file already exists, it will be overwritten.
```csharp
prefab.ExportToFile("my_new_prefab.lsp");
JSONNode json = prefab.ToJson();
```
### Build flags
Build flags are certain flags that modify the behavior of the prefab builder.

There are current three build flags - `SortObjects`, `SortKeyframes` and `AbsoluteRotation`.

`SortObjects` sorts your objects in the prefab by start time in ascending order.

`SortKeyframes` sorts your keyframes in the prefab by time in ascending order.

`AbsoluteRotation` converts your rotation keyframes from absolute rotation to relative rotation on build.

```csharp
PrefabBuildFlags flags = PrefabBuildFlags.SortKeyframes | PrefabBuildFlags.AbsoluteRotation;
prefab.ExportToFile("my_new_prefab.lsp",  flags);
JSONNode json = prefab.ToJson(flags);
```
## Example program
### Procedural bomb generator
```csharp
using System;
using System.Numerics;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new prefab
            Prefab prefab = new Prefab("Procedural Bomb", PrefabType.Bombs);

            // Create a new parent object
            PrefabObject baseObj = prefab.CreateObject("Bomb Base");
            baseObj.ObjectType = PrefabObjectType.Empty;

            baseObj.PositionKeyframes.Add(new PositionKeyframe());
            baseObj.ScaleKeyframes.Add(new ScaleKeyframe() { Value = Vector2.One });
            baseObj.RotationKeyframes.Add(new RotationKeyframe() { Value = 0.0f });
            baseObj.ColorKeyframes.Add(new ColorKeyframe() { Value = 0 });

            // Create bullets
            for (int i = 0; i < 8; i++)
            {
                Vector2 v = new Vector2(MathF.Cos(i / 4.0f * MathF.PI), MathF.Sin(i / 4.0f * MathF.PI));

                PrefabObject bullet = prefab.CreateObject("Bullet");
                bullet.EditorBin = 1;
                bullet.Shape = PrefabObjectShape.Circle;

                bullet.SetParent(baseObj);

                bullet.PositionKeyframes.Add(new PositionKeyframe() { Time = 0.0f, Value = Vector2.Zero });
                bullet.PositionKeyframes.Add(new PositionKeyframe() { Time = 10.0f, Value = v * 60.0f });

                bullet.ScaleKeyframes.Add(new ScaleKeyframe() { Time = 0.0f, Value = Vector2.One });
                bullet.RotationKeyframes.Add(new RotationKeyframe() { Time = 0.0f, Value = 0.0f });
                bullet.ColorKeyframes.Add(new ColorKeyframe() { Time = 0.0f, Value = 0 });
            }

            // Export to file
            prefab.ExportToFile("procedural_bomb.lsp");
        }
    }
}
```
