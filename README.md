# UMD Smith School Marketing VR Research Project


## Overview
This project is developed for the Marketing Department of Robert H. Smith School of Business at the University of Maryland. This project focuses on XR interaction analytics in Unity, enabling users to interact with dynamically spawned objects in VR while capturing structured behavioral data such as grab frequency, interaction duration, and visual attention.

The system is designed for user behavior analysis, interaction research, and XR prototyping, with an emphasis on flexible data logging and extensibility.

## Features
- Dynamic Object Spawning
- XR Interaction Analytics
- View-Based Attention Tracking
- Structured Data Logging
- More to be added

## Project Structure
```
Assets/
├── Scenes/
│   ├── UserStudy.unity
│
├── Scripts/
│   ├── DataRecorder.cs
│   ├── InteractionLogger.cs
│   ├── ObjectSpawner.cs
│   ├── SelectorMenuBuilder.cs
│   ├── SelectorMenuInteractions.cs
│   ├── StatsDisplay.cs
│
├── Resources/
│   ├── SpawnableObjects/

```
## Requirements
- Unity 6.4 (project developed and tested on version 6000.4.1f1)
- Meta Interaction SDK
- Unity USD Importer package

## Setup Instructions 
1. Clone the repository:

```git clone https://github.com/DDXPP/Smith_School_Marketing_VR_Research_Project.git```

2. Open the project in Unity 6.4
3. Install required packages (via Package Manager):
    -   Meta XR SDKs
    -   USD Importer (if using USDZ assets)
    
    Detailed instructions on setting up Unity Editor and setting up Unity 3D Project can be found in [Meta Horizon documentation](https://developers.meta.com/horizon/documentation/unity/unity-tutorial-hello-vr/#step-2-set-up-your-unity-3d-project).   

4. Open `UserStudy` scene
5. Build and run on an connected XR device (project developed and tested on Meta Quest 2)

## Usage Guide
### Controller inputs
- X Button
    - Toggle stats display (show/hide)
- Y Button (Single Press)
    - Open object selection menu
    - Press again to cycle through available objects
    - Use "DESTOY ALL" button to clear all spawned objects in the scene
- Y Button (Long Press/Hold)
    - Confirm the current selection
    - Controller vibration confirms successful selection

### Spawnable Object Prep
To make an object spawnable in the project:
1. Place the prefab inside:

    ```Assets/Resources/SpawnableObjects/```


2. Ensure the prefab contains the following components:
    - `Mesh Renderer`
    - `Mesh Collider`

3. If importing a raw 3D model:
    - Drag the model into the scene
    - Configure components if needed (you may need to manually select the mesh in `Mesh Collider`)
    - Create a prefab from the model before placing it into the `SpawnableObjects` folder


### Exporting Logged Data
Logged data files are stored locally on the Meta Quest 2 at:

```/storage/emulated/0/Android/data/<your.package.name>/files/```

File name: `YYYYMMDD_HHMMSS_[object_name]_vr_data.csv`

You can access the files by:
- Connecting the headset to a computer via USB
- Using Android File Transfer tools
- Browsing the device storage directly

## Configurable Variables
Several important variables are exposed as public fields in the Unity inspector, allowing you to tune system behavior without modifying code. To configure the following variables, first click the GameObject `ObjectManager` in Unity's hierarchy window; then locate the component `Object Spawner (Script)` in the inspector where you can enter the desired values.

### Spawn Position
- Controls where objects are instantiated in the scene
- Default: above the table surface in the scene

### Samples Per Second
- Controls how frequently interaction data is collected
- Default: `10` samples per second

### Save Interval
- Controls how often collected data is written/saved
- Default: every `2` seconds

## Logged Data
Each object records (in `.csv` file):
- Date & time
- Runtime
- Object name
- Object position (`x,y,z`)
- Object rotation (`rx,ry,rz`)
- Touch status
- Grab status
- Time to grab from touching
- Distance from touch to grab
- Grab events (`GrabStart, GrabEnd, TouchNoGrab`)
- Grab count
- Grab durations
- Hover without grab count
- Viewing side
- Viewing angles in dot products:
    - Front 
    - Right
    - Up

<!-- ---
Keywords: XR, VR, Unity, Interaction Tracking, Data Logging, User Behavior Analytics -->