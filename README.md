Demo to illustrate the function of an A* pathfinding system on a hex based grid.

This concept shown was made a while back when I was starting to pick up programing so there are some things that should be refactored if you are interested in implementing this concept.
- In this build, a majority of functions go through one class, the game controller. Spreading out the functionality and making the classes less dependent on this one class will help with the overall structure of your own project. My own creations after this one did improve upon this too.

```

How To:

This game demo was made using Unity v2021.1.10f1

When playing the scene within unity editor, you can left click on a "Unit" and right click anywhere else on the grid, this will show you a generated path for that unit to take. Left clicking again deselects that unit. When you click play turn the units will move towards their destinations.

One thing to note, the pathfinder in this version does not take into account collisions or the possibility of two units occupying the same space, so further development to avoid such instances would be needed.