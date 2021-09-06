using System;
using System.Numerics;

namespace PAPrefabToolkit.Test
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
            baseObj.RotationKeyframes.Add(new RotationKeyframe() { Time = 0.0f, Value = 0.0f });
            baseObj.RotationKeyframes.Add(new RotationKeyframe() { Time = 2.5f, Value = 90.0f });
            baseObj.RotationKeyframes.Add(new RotationKeyframe() { Time = 4.0f, Value = 10.0f });
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
            prefab.ExportToFile("procedural_bomb.lsp", PrefabBuildFlags.AbsoluteRotation);
        }
    }
}
