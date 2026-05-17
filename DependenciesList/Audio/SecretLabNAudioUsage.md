### Playing global audio
```cs
using SecretLabNAudio.Core;
using SecretLabNAudio.Core.Extensions;
using SecretLabNAudio.Core.Pools;

public static void PlayGlobalAudio(string path)
{
    AudioPlayer player = AudioPlayerPool.RentGloballyAudible();
    player.UseFile(path).PoolOnEnd();
}
```

### Playing spatial audio

```cs
using SecretLabNAudio.Core;
using SecretLabNAudio.Core.Extensions;
using SecretLabNAudio.Core.Pools;

public static void PlaySpatialAudio(string path, Vector3 position)
{
    AudioPlayer player = AudioPlayerPool.RentDefault(position);
    player.UseFile(path).PoolOnEnd();
}
```

---
The example code is either taken from offical documentation which is credited to the author or is made by me.