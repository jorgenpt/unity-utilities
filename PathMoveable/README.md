# Path Moveable

`PathMoveable` is a component you can use with
[`CharacterController`][charactercontroller] and [AngryAnt's pathfinding
library, Path][path]. It implements a path *walker* (as opposed to Path's
[`Navigator`][path-navigator], which is a path *finder*.)

## Using it

To use `PathMoveable`, you simply have to add the following three components to
the game object you want to move:

 1. Add `CharacterController` (This is built-in in Unity)
 1. Add `Navigator` (This is a part of Path)
 1. Add `PathMoveable` (From this directory. You can also add
    `EditorSpeedMoveable` which is a subclass that allows you to set the
    movement speed in the editor via a public field.)

When this is done, configure the components as you usually would. You can control the object in one of two ways:

```cs
    // Start movement:
    obj.SendMessage ("StartMovingTo", new Vector3 (5f, 0.5f, 3f));
    obj.GetComponent<PathMoveable> ().StartMovingTo(new Vector3 (5f, 0.5f, 3f));

    // Stop movement:
    obj.SendMessage ("StopMoving");
    obj.GetComponent<PathMoveable> ().StopMoving ();
```

I recommend checking out the source if you want to tweak - it should hopefully
be readable and well-commented.

## Problems

If you have any problems, feel free to contact me at jorgenpt@gmail.com. If you
find any bugs, please report them on the [issue tracker][issues].

[charactercontroller]: http://unity3d.com/support/documentation/ScriptReference/CharacterController.html
[path-navigator]: http://angryant.com/path/documentation/#Navigator
[path]: http://angryant.com/path/download/
[issues]: https://github.com/jorgenpt/unity-utilities/issues
