### Playing global audio

```cs
// Creates global audio player which everyone can hear from any location.
public void CreateGlobal()
{
    AudioPlayer audioPlayer = AudioPlayer.CreateOrGet($"Global AudioPlayer", onIntialCreation: (p) =>
    {
        // This created speaker will be in 2D space ( audio will be always playing directly on you not from specific location ) but make sure that max distance is set to some higher value.
        Speaker speaker = p.AddSpeaker("Main", isSpatial: false, maxDistance: 5000f);
    });

    audioPlayer.AddClip("shot");
}
```
---
The example code is either taken from offical documentation which is credited to the author or is made by me.
