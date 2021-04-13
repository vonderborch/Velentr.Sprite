# Velentr.Sprite
A handy growing library of sprite/texture enhancements.


# Installation
There are nuget packages available for Monogame and FNA.
- Monogame: [Velentr.Sprite.Monogame](https://www.nuget.org/packages/Velentr.Sprite.Monogame/)
- FNA: [Velentr.Sprite.FNA](https://www.nuget.org/packages/Velentr.Sprite.FNA/)

# Features
- Auto texture atlas creation: handle loading textures into texture atlases automatically
  - includes functionality to auto-rebalance texture atlases based on usage and size of textures

# Usage
### Texture Atlas
```

// the manager should be declared as a class variable
private TextureManager manager;

// this code should go in your game's setup method (or equivalent)
manager = new TextureManager(GraphicsDevice);
var textures = new List<string>()
{
    "11-6.jpg",
    "beautiful-sunset-33.jpg",
    "chloe4.jpg",
    "gorgeous-f2-goldendoodle-puppies-5a7de534c591b.jpg",
    "OIP.jpg",
    "R3d398e62bb9b79f54b5c49b4e5f32925.jpg",
    "Rdb919eda1b98929d3a17685a6d3d7ef8.jpg",
    "Refd96265ac3da1f5b5dc5277f1082d15.jpg"
};

var texturesLoadInfos = new List<TextureLoadInfo>();
for (var i = 0; i < textures.Count; i++)
{
    texturesLoadInfos.Add(new TextureLoadInfo($"image{i}", $"Content\\{textures[i]}", new Point(256, 256)));
}
manager.LoadTextures(texturesLoadInfos);

// this code is optional and would go in your game's update method (or equivalent)
manager.Update(gameTime); // this handles auto-texture atlas re-balancing, if enabled.

// this code should go in your game's draw method (or equivalent)
//manager.DrawFullAtlas(_spriteBatch, 0, GraphicsDevice.Viewport.Bounds, Color.White); // draws the full atlas (useful for debugging)
_spriteBatch.Draw(manager, "image0", new Vector2(50, 50), Color.White);

```

![Screenshot](https://github.com/vonderborch/Velentr.Sprite/blob/main/Example.PNG?raw=true)

# Future Plans
See list of issues under the Milestones: https://github.com/vonderborch/Velentr.Sprite/milestones
