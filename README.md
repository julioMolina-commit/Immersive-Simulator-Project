# Immersive-Simulator-Project
This is an old unity3D project focused on breaking different systems, such as dialogue, interactions, creatures, etc. Into small modules that can be used with each other to so that one may trigger damage on any creature or object through, for example, a dialogue option or an interaction with a certain object.

It uses an event system to distinguish different states for the player (Pick Up Mode (The player is holding a player at the moment), Dialogue Mode (The player is in the middle of a dialogue) and Combat Mode (The standard mode that allows attacks (Punches))).

In all systems different "effects" may be triggered since events can be slotted in dialogue trees, interactions with objects or the "death" of objects. In this way one may defeat an enemy and trigger a door, activate camera shake for a couple of lines of dialogue and turn it off at the end, or even heal the player the moment he interacts with an object.

The movement allows for limited running, with Stamina; jumping and sliding when crouching.

The dialogue system allows for branching dialogues created just by placing a "Dialogue Trigger" on an object, which then allows you to generate lines of dialogue which may or may not allow up to four options with line jumps to contemplate for different paths. It also let's you trigger events at the start or end of any dialogue.
