# Pathfindr

![gif](http://ryanwebb.com/images/pathfindrr.gif)

Pathfinding system using the principles of the A* search algorithm.

Main code located in Assets/Scripts/Pathfindr should you want to port it into your project or extend it.

Environment assets in the preview gif are from Mikael Gustafsson’s Stylized Nature Pack: http://u3d.as/gmF


## Usage

Create a PFGrid and use the Evaluate() method to return a list of obstacle nodes within the scene. Then create an instance of PFEngine, passing in the desired grid resolution and list of obstacles. Finally call the GetPath() method of PFEngine with your start and end co-ordinates to find the fastest path between the two points.


## Todo

Add Jump Point Search Algorithm capability to speed up pathfinding in complex scenes.