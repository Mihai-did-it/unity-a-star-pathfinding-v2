# Unity A* Pathfinding (v2)

A Unity C# implementation of the **A\*** pathfinding algorithm designed for grid-based and wall-connected navigation systems.  
This version includes improvements to node selection, midpoint tracking, and cost handling to ensure accurate obstacle-aware paths.

---

## Overview
The goal of this project was to build a working A* pathfinding algorithm inside Unity.  
The algorithm explores nodes in order of total cost (`g + h`) — where `g` is the distance so far and `h` is the estimated distance to the goal — and reconstructs the shortest path using midpoint data from connecting walls.

---

## How It Works
1. Maintains two lists:
   - **Open list** – nodes yet to explore  
   - **Closed list** – nodes already visited  
2. For each node:
   - Selects the node with the lowest total cost  
   - Checks neighbors and updates their `g` and `f` values if a better path is found  
   - Records where each node came from using `cameFrom`  
3. Once the goal is reached:
   - Backtracks through `cameFrom` to reconstruct the full path  
   - Uses each wall’s midpoint (`neighbor.GetWall().midpoint`) to build a smooth navigation path  
   - Reverses the collected path and appends the target position  

---

## Features
- Precise midpoint-based path reconstruction  
- Automatic cost recalculation for updated targets  
- Clean path generation avoiding obstacles  
- Modular structure for integration with Unity game logic  

---

## Results
- The system produced **optimal paths** for all provided test maps.  
- Verified through Unity console logs that path lengths matched expected results.  
- Handles dynamic targets and reinitializes properly on each run.  

---

## Challenges & Fixes
- Initially the car ignored walls because midpoints weren’t used.  
  → Fixed by adding `neighbor.GetWall().midpoint` in the reconstruction phase.  
- Paths included extra or missing nodes.  
  → Fixed by reversing the traced path and appending the final goal.  
- Target switching caused variable persistence issues.  
  → Fixed by resetting all variables at the start of each A* run.  

---

## Folder Structure


---

## Run in Unity
1. Open the folder in **Unity Hub**.  
2. Load the project and open the provided framework scene. 
4. Press **Play** and observe path visualization in the Scene view or Game view.


---
