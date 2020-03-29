# NeoFPS_OotiiMotionController
Scripts and assets created to integrate NeoFPS and the Third Person Motion Controller by Ootii in order to create NPCs.

## Requirements
This repository was created using Unity 2018.4

It requires the assets [NeoFPS](https://assetstore.unity.com/packages/templates/systems/neofps-150179?aid=1011l58Ft) and [Ootii's Third Person Motion Controller](https://assetstore.unity.com/packages/templates/systems/third-person-motion-controller-15672?aid=1011l58Ft).

## Installation
This integration example is intended to be dropped in to a fresh project along with NeoFPS and Behavior Designer.

1. Import NeoFPS and apply the required Unity settings using the NeoFPS Settings Wizard. You can find more information about this process [here](https://docs.neofps.com/manual/neofps-installation.html).

2. Import Ootii's Third Person Motion Controller asset (do not allow the motion controller to overrite project layers, you will need to customize these layers for your setup as the default layers override the Neo FPS layers)

3. Clone this repository to a folder inside the project Assets folder such as "NeoFPS_OotiiMotionController".

4. Resolve the errors relating to overriding methods and accessing fields in BasicDamageHandler by making the methods virtual and the fields protected [this will not be necessary after a future release of Neo SPS]

5. Open the `Demo Scene` in the root of this project.
	
## Integration

This project is intended as a means to add NPC characters to your NeoFPS projects. It is not intended to add support for switching between first and third person player characters.

Alone this integration will not do anything. The motion controller is used to manage animations and motions for NPCs. Your NPCs will need some form of AI as well. See below.

The integration comes with a demo scene. See `NeoFPS_OotiiMotionController/Demo Scene`.

### Setting up the Player

To setup your player you will need to add a Motion Controller `Actor Core` component.
This component has a `Neo FPS Damage Reactor` applied. This reactor converts damage messages from Motion Controller combatants into damage for your player.

The fastest way to setup the Neo FPS player is to use the `NeoFPS_MotionController_TestSetup` prefab that comes in this integration. This is a modified version of the TestSetup that comes
with Neo FPS. The only real difference is that it uses the `MotionControllerDemoSoloCharacter` prefab which is already setup with the Actor Core component.

### Setting Up NPCs

In order to use the Motion Controller for your on NPCS you will need to do the following:

  1. Configure your NPC to use Motion Controller as you would normally (`Window -> ootii Tools -> Character Wizard` works well, be sure to select the NPC option)
  2. Add a Neo FPS Health Manager, such as `Basic Health Manager`
  3. Add a Neo FPS Damage Handler, such as `Basic Damage Handler`
  4. Add a Neo FPS Surface, such as `Simple Surface`
  5. Add `MotionControllerAIController` to the root of the character (even if you don't plan to use this for AI, it handles things like death motions)
  6. [OPTIONAL] Add an AI to control the NPC, such as the Neo FPS [Behavior Designer Integration](https://github.com/YondernautsGames/NeoFPS_BehaviorDesigner) 
     (see below for a description of the super simple AI used in the demo)

## AI

NPCs typically need some form of AI. There is a very basic "AI" system included with this integration, however, it is not in any way a real AI system. Maybe it will grow up one
day but for now it's only real purpose is to enable an out of the box demo for Neo FPS and Motion Controller integration. If you are strapped for cash and want to enhance the
AI in this integration please do submit partches to improve the system here. Otherwise you might want to look at the [Behavior Designer Integration](https://github.com/YondernautsGames/NeoFPS_BehaviorDesigner).

### Using the AI

The system consists of `AIBehaviourController` components and `AIBehaviour` scriptable objects. 
The controllers will process an ordered list of behaviours each `tick` (the frequency of the `tick` is defined)
in the component settings. There can be multiple AIBehaviourControllers attached to a single NPC. Each 
controller having a different set of behaviours and can have a different tick frequency.

Each controller will iterate through each of the behaviours listed in the order listed.
Each will be tested to see if can be executed based on whether the behaviour is currently
active and if the conditions for execution are satisfied. If it can be, it will be executed. The conditions for
execution can be set by assigning AICondition Scriptable Objects to the behavour. Once a behaviour is executed, and 
it returns success then processing stops for this tick. That is only a single behaviour in each controller will be 
fired in each tick.

There are currently a very limited set of behaviourstime. As noted above the goal of this system is not to be a true 
AI system, but rather provide enough of an AI system to enable a self contained demo for this integration. That said,
it is growing. If it should become sufficiently viable it may be broken out as an AI in itelf. Contributions are 
welcome.

Here are some of the behaviours currently included:

  1. MotionControllerEquipWeapon - fire the Ootii motion controller Equip Weapon motion
  2. NavMeshWander - have a NavMesh agent wander semi-randomly
  3. NavMeshGoTo - go to a specific place on the navmesh
  4. NavMeshSeek - if an object with a given tag is detected then seek it (move towards it)

And the conditions:

  1. CanSenseTaggedObject - detects objects with a particular tag

 ## Open Game Art

 This integration includes some Game Art from http://opengameart.org. These assets are under a CC0 license unless 
 otherwsie stated. You are free to use them without restrictions. Please support [Open Game Art](://opengameart.org) 
 and the artists who publish there.