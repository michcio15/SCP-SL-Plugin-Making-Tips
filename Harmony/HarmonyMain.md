# What is Harmony?
Harmony is a library used to change the code in the game. We will be using in this guide version `2.4.2` because its the newest. You can download it from Nuget and drop it into LabAPI dependencies.

# What can it be used for?
It can be used for:
- Adding new features like events etc.
- Fixing base game bugs
- Blocking code execution
- And much more.

# How to use it?

Here is the link to [official docs](https://harmony.pardeike.net/api/index.html) and [offical guides](https://harmony.pardeike.net/articles/intro.html). The offical guides are worth reading but here i will try to shorten them.

>[!TIP]
>Remember to always check if there is already a event that allows you to do it since there is no need to patch if it already exists.
Harmony has 3 main patching types:
- [Prefix](PrefixGuide.md) - Here the code is executed **before** the function so you can alter the output, skip the method entirely or change input values
- Postfix - The code is executed **after** the original function. Its mostly used for well executing after.
- Transpiler - Most advanced and the most powerfull of them all. It allows you to **alter** the instruction.

## Patching
We create a new harmony instance and then call `Harmony::PatchAll()`


```cs
private static Harmony _harmony = null!;

public static void PatchAll()
{
    // We create a new harmony instance
    _harmony = new("com.example.myharmony");
    // Patch all
    _harmony.PatchAll();
}

public static void UnpatchAll()
{
    // We unpatch all based on the ID
    _harmony.UnpatchAll(harmony.Id);
}
```

## Targeting methods
Let's say you want to do something when players has used adrenaline and its effect are given. Lets look at the decompiled code.

```cs
using System;
using CustomPlayerEffects;
using PlayerStatsSystem;

namespace InventorySystem.Items.Usables
{
	// Token: 0x02000D9B RID: 3483
	public class Adrenaline : Consumable
	{
		// Token: 0x06004C18 RID: 19480 RVA: 0x00103BA8 File Offset: 0x00101DA8
		protected override void OnEffectsActivated()
		{
			base.Owner.playerStats.GetModule<StaminaStat>().AddAmount(1f);
			base.Owner.playerStats.GetModule<AhpStat>().ServerAddProcess(40f);
			base.Owner.playerEffectsController.EnableEffect<Invigorated>(8f, true);
			base.Owner.playerEffectsController.UseMedicalItem(this);
			base.Owner.playerEffectsController.DisableEffect<AmnesiaVision>();
		}

		// Token: 0x04003604 RID: 13828
		private const float StaminaRegenerationPercent = 100f;

		// Token: 0x04003605 RID: 13829
		private const float InvigoratedTargetDuration = 8f;

		// Token: 0x04003606 RID: 13830
		private const bool InvigoratedDurationAdditive = true;

		// Token: 0x04003607 RID: 13831
		private const float AhpAddition = 40f;
	}
}
```
> Nortwood Studios ©

We want to do smth with the `OnEffectsActivated`. We will use a attribute `[HarmonyPatch]` given us by Harmony. You can add this to a class or to a method.
```cs
[HarmonyPatch(typeof(Adrenaline), nameof(Adrenaline.OnEffectsActivated))]
```
 this is an example when we wanna do smth with the `Adrenaline::OnEffectsActivated()`. If you dont see it you have to either:
- Type the name manually
- [Publicize your `Assembly-Csharp`](/Publicizing/publicizing.md)

Read more here:   

## Can I patch setters / getters
The anwser is yes! Let's say that for shits and giggles we want to patch getter of the `ReferenceHub::PlayerId`
```cs
public int PlayerId
{
	get => return this._playerId.Value;
}
```
To target the method we do this
```cs
[HarmonyPatch(typeof(ReferenceHub), nameof(ReferenceHub.PlayerId),  MethodType.Getter)]
```
and if we wanna patch a setter we just change the `MethodType` to `MethodType.Setter`

## What if there are 2 methods named the same way?

 Let's say e.g we wanna patch
`ReferenceHub.GetHub(GameObject gameObject)` but we can see there are 2 methods named this way.

```cs
// WE WANT TO PATCH THIS
public static ReferenceHub GetHub(GameObject player)
{
	ReferenceHub referenceHub;
	if (!ReferenceHub.TryGetHub(player, out referenceHub))
	{
		return null;
	}
	return referenceHub;
}

// NOT THIS
public static ReferenceHub GetHub(MonoBehaviour player)
{
	ReferenceHub referenceHub;
	if (!ReferenceHub.TryGetHub(player.gameObject, out referenceHub))
	{
		return null;
	}
	return referenceHub;
}
```
> Nortwood Studios ©

So how do we do it?
The first way and easier one is just to add the types of the arguments into the attribute.
It would look like this.
```cs
[HarmonyPatch(typeof(ReferenceHub), nameof(ReferenceHub.GetHub), new[] { typeof(GameObject) })]
```

---

In the other way we stil add the `HarmonyPatch` attribute but we leave it empty. It should look smth like this. 
```cs
[HarmonyPatch]
```
then inside the class we add a `public static MethodBase TargetMethod()` method. If we dont want to name it exactly like this above the method that returns `MethodBase` we add an attribute `HarmonyTargetMethod`. In this case the method should look like this.

```cs
public static MethodBase TargetMethod()
{
    return AccessTools.Method(typeof(ReferenceHub), nameof(ReferenceHub.GetHub), new[] { typeof(GameObject) });
}
```
The `new[] { typeof(GameObject) }` are the arguments that the function requires.

Read more here: https://harmony.pardeike.net/articles/patching-auxiliary.html#targetmethods

## Can i patch multiple methods the same way

Its the same way as above but you change it to `IEnumerable<MethodBase> TargetMethods()`.

Read more here: https://harmony.pardeike.net/articles/patching-auxiliary.html#targetmethods


