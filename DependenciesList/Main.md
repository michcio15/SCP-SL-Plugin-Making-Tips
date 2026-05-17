# List of good dependencies 
This will **NOT** be a tutorial on how to do it yourself. It will be more of a list of good libraries / plugins / dependencies idk with good documentations which can help you with that.


# Audio
| Name                | Link                                          | Description from the repo                                                                                                                                                           | Example Usage
| ------------------- | --------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | - |
| **SecretLabNAudio** | https://github.com/Axwabo/SecretLabNAudio/    | An advanced audio player API for SCP: Secret Laboratory using NAudio                                                                                                                | [Link](Audio/SecretLabNAudioUsage.md)|
| **AudioPlayerApi**  | https://github.com/Killers0992/AudioPlayerApi | AudioPlayerApi is a dependency for plugins that provides advanced capabilities for managing and playing audio clips, complete with support for spatial audio and multiple speakers. | [Link](Audio/AudioPlayerApiUsage.md)

# Schematics / Maps

| Name                  | Link                                               | Description from the repo                                                                                                                                                                                                                                                                              | Remarks                                                                                                                                                                                                                 |
| --------------------- | -------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **ProjectMER**        | https://github.com/Michal78900/ProjectMER          | SCP: Secret Laboratory plugin allowing to spawn and modify various objects.                                                                                                                                                                                                                            | It's the most tested plugin in this field. However its last update was on 02.11.2025, since as the author said he hadn't had much time for updating.                                                                    |
| **ThaumielMapEditor** | https://github.com/Thaumiel-Team/ThaumielMapEditor | Thaumiel Map Editor (TME) is a Unity based map editor designed to help developers and designers create, edit, and manage game maps. TME provides an in editor interface for placing and configuring a wide range of game objects, from interactive elements and lighting to props and navigation aids. | This plugin is still in early development (15.05.2026) so bugs could happen however it has more out of the box functionality that ProjectMER like client sided primitives to not have performence impact on the server. |

# Server Specific Settings

| Name                | Link                                | Description from the repo                                                                               | Remarks                                                                                                                                        |
| ------------------- | ----------------------------------- | ------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------- |
| Actually Simple SSS | https://github.com/Someone-193/ASS  | ASS is a plugin that fundamentally reworks base game ServerSpecificSettings to include better features. | This plugin uses more of base game approach like lists and events.                                                                             |
| SecretAPI           | http://github.com/obvEve/SecretAPI/ | SecretAPI is a plugin that extends LabAPI by providing extra features to help devs.                     | This plugin uses object like approach to settings so the whole setting like keybind is encapsulated inside and a class inherting from an base. |

# Hints

>[!WARNING]
>You only use one they are not compatible

| Name            | Link                                           | Description from the repo                                                                                                                    |
| --------------- | ---------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------- |
| RueI            | https://github.com/pawslee/RueI                | RueI is a hint framework, designed to be the definitive way to display multiple hints at once.                                               |
| HintServiceMeow | https://github.com/MeowServer/HintServiceMeow/ | HintServiceMeow (HSM) is a SCP: Secret Laboratory framework that allows plugins to display text on a selected position on a player's screen. |


---
If you want to have your plugin listed here open an issue or contact me on discord or smth.