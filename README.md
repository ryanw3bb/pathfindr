# Pathfindr

![gif](https://ryanwebb.com/images/pathfindr.gif)

Pathfinding using the A* search algorithm.


## Usage

Create a PFGrid and use the Evaluate() method to return a list of obstacle nodes within the scene. Then create an instance of PFEngine, passing in the desired grid resolution and list of obstacles. Finally call the GetPath() method of PFEngine with your start and end co-ordinates to find the fastest path between the two points.


## Todo

Add Jump Point Search Algorithm capability to speed up pathfinding in complex scenes.
