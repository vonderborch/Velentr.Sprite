# Velentr.Sprite
A handy growing library of sprite/texture enhancements.


# Installation
There are nuget packages available for Monogame and FNA.
- Monogame: [Velentr.Sprite.Monogame](https://www.nuget.org/packages/Velentr.Sprite.Monogame/)
- FNA: [Velentr.Sprite.FNA](https://www.nuget.org/packages/Velentr.Sprite.FNA/)


# Features
- Texture Manager: handles creating texture atlases automatically and stores textures
- Auto texture atlas creation: handle loading textures into texture atlases automatically
  - includes functionality to auto-rebalance texture atlases based on usage and size of textures (optional, disabled by default)
- Sprite System
  - Simple Sprites: simple image/texture with positioning information. Class: `SimpleSprite`
  - Animated Sprites: sprite using multiple sub-images to replicate an animation. Class: `AnimatedSprite`
  - Composite Sprites: a combination of other sprite merged that are considered one entity (scene graph). Class: `CompositeSprite`
  - Omni Sprites: a sprite that internally creates one of the other sprite types and impersonates it. Class: `Sprite`

# Usage
A full example can be seen in the dev environment: https://github.com/vonderborch/Velentr.Sprite/blob/main/Core/Velentr.Sprite.Core.DevEnv/Game1.cs

### Texture Atlas
```

// the manager should be declared as a class variable
private SpriteManager manager;

// this code should go in your game's setup method (or equivalent)
manager = new SpriteManager(GraphicsDevice);
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

### Sprites
All sprites utilize the TextureManager system and require a texture manager to be enabled (and have the textures they require loaded)

##### Simple Sprite
Simple image/texture with positioning information
```
var sprite = new SimpleSprite(manager, "SPRITE_NAME_HERE", "TEXTURE_NAME_HERE", new Vector2(size.X, size.Y));
sprite.Draw(_spriteBatch);
```
![Screenshot](https://github.com/vonderborch/Velentr.Sprite/blob/main/simple_sprite.PNG?raw=true)

##### Animated Sprite
Sprite using multiple sub-images to replicate an animation
```
var frameTime = 75;
var animatedSprite = new AnimatedSprite(
    manager,
    "walking_women",
    new Vector2(animationSize.X, animationSize.Y),
    new Animation(
        "walking",
        true,
        new Frame[]
        {
            new Frame("f1", "woman1", frameTime),
            new Frame("f2", "woman2", frameTime),
            new Frame("f3", "woman3", frameTime),
            new Frame("f4", "woman4", frameTime),
            new Frame("f5", "woman5", frameTime),
            new Frame("f6", "woman6", frameTime)
        }
    ),
    new Animation(
        "walking2",
        true,
        new Frame[]
        {
            new Frame("f1", "woman1", frameTime * 2),
            new Frame("f2", "woman2", frameTime * 2),
            new Frame("f3", "woman3", frameTime * 2),
            new Frame("f4", "woman4", frameTime * 2),
            new Frame("f5", "woman5", frameTime * 2),
            new Frame("f6", "woman6", frameTime * 2)
        }
    )
);
animatedSprite.Update(gameTime);
animatedSprite.Draw(_spriteBatch);
```
![Screenshot](https://github.com/vonderborch/Velentr.Sprite/blob/main/animated_example.gif?raw=true)

##### Composite Sprite
A combination of other sprite merged that are considered one entity (scene graph)

Composite sprites can be made up of simple sprites...
```
var compositeSprite = new CompositeSprite(
    manager,
    "car_1",
    hullSizeVector,
    new Sprite(manager, "hull", "car_hull", hullSizeVector),
    new Sprite(manager, "tire1", "car_tire", tire1Location, Color.White, tireSizeVector, 0f, Vector2.Zero, SpriteEffects.None, 0f),
    new Sprite(manager, "tire2", "car_tire", tire2Location, Color.White, tireSizeVector, 0f, Vector2.Zero, SpriteEffects.None, 0f)
);
```
Or a mixture of any sprite type (Simple Sprite, Animated Sprite, or even other Composite Sprites)...
```
var tire_animation = new Animation(
    "tire",
    true,
    new Frame[]
    {
        new Frame("t1", "car_tire", 100),
        new Frame("t1", "car_tire2", 100),
        new Frame("t1", "car_tire3", 100),
        new Frame("t1", "car_tire4", 100)
    }
);
var compositeAnimatedSprite1 = new CompositeSprite(
    manager,
    "a_car_1",
    hullSizeVector,
    new Sprite(manager, "hull", "car_hull", hullSizeVector),
    new AnimatedSprite(
        manager,
        "tire1",
        tire1Location,
        tireSizeVector,
        animations: new Animation(tire_animation)
    ),
    new AnimatedSprite(
        manager,
        "tire2",
        tire2Location,
        tireSizeVector,
        animations: new Animation(tire_animation)
    )
);
compositeAnimatedSprite1.Update(gameTime);
compositeAnimatedSprite1.Draw(_spriteBatch);
```

![Screenshot](https://github.com/vonderborch/Velentr.Sprite/blob/main/composite_animated_example.gif?raw=true)
![Screenshot](https://github.com/vonderborch/Velentr.Sprite/blob/main/composite_animated_example_2.gif?raw=true)

##### Omni Sprites
Omni sprites are initialized via the same techniques as the above, but rather than specifying a specific sub-sprite type, instead initialize as a `Sprite` but with the parameters for the sub-type you want.

##### Extensibility
Additional Sprite types can be created by extending the Sprites\Base\VelentrSprite.cs class.


# Future Plans
See list of issues under the Milestones: https://github.com/vonderborch/Velentr.Sprite/milestones
