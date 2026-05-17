# Prefix patches

> A prefix is a method that is executed before the original method[^1]

Why and whenshould you use prefixes?
- They are easy
- You can quickly prototype
- They can work with Postfixes[^2]
- You can completely prevent the original method from running
- You can change the arguments

# Void example
Lets say we want to check if player is a Class-D. If so and he uses and adrenaline we kill him.
>[!NOTE]
>Always remeber to check if there is already an event in this case this is an example so we don't care.

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

We will patch the `Adrenaline::OnEffectsActivated()`.
Let's create a new class named `AdrenalineOnEffectsActivatedPatch`.

```cs
using HarmonyLib;
using InventorySystem.Items.Usables;

//Target Method
[HarmonyPatch(typeof(Adrenaline), nameof(Adrenaline.OnEffectsActivated))]
internal static class AdrenalineOnEffectsActivatedPatch
{
}
```

We target here the method. After that let's add the prefix. But before we do that we want to think will we stop the exectucion of the original method.
If you want to control whether the original method executes,
your prefix should return `bool`.
Returning `false` skips the original method.

```cs
using HarmonyLib;
using InventorySystem.Items.Usables;

[HarmonyPatch(typeof(Adrenaline), nameof(Adrenaline.OnEffectsActivated))]
internal static class AdrenalineOnEffectsActivatedPatch
{
    internal static bool Prefix()
    {
        return true;
    }
}
```

Now let's talk about how does Harmony know it is a prefix or not. It checks the method name in this case `Prefix` or checks if there is a `[HarmonyPrefix]` attribute. If the attribute is existing you can name the method whatever you want.

But how do we get the `Adrenaline` instance? Harmony has a [list](https://harmony.pardeike.net/articles/patching-injections.html) of supported names and what they do.
We will use `__instance`. This is just `this` keyword but in a patch. So lets write our patch.

```cs
using HarmonyLib;
using InventorySystem.Items.Usables;
using PlayerRoles;
using PlayerStatsSystem;

[HarmonyPatch(typeof(Adrenaline), nameof(Adrenaline.OnEffectsActivated))]
internal static class AdrenalineOnEffectsActivatedPatch
{
    [HarmonyPrefix]
    internal static bool Prefix(Adrenaline __instance)
    {
        // We check if the player is a class D
        if (__instance.Owner.GetRoleId() == RoleTypeId.ClassD)
        {
            // We deal the damage. We use -1 so it's a guaranteed kill
            __instance.Owner.playerStats.DealDamage(new CustomReasonDamageHandler("Heart Attack", -1));
            // We return false so the original method is not called
            return false;
        }

        // If the player is not a class D, we return true so the original method is called
        return true;
    }
}
```

# Changing the output
Now we want to check the players role and if they are Facility Guard we don't allow them to heal the `Burned` effect.
This can be done either by `Prefix` or a `Postifx`
> [!NOTE]
> It is not recommended to skip the original unless you want to completely change the way it works. If you only want a small change or a side effect, using a postfix or a transpiler is always preferred since it allows for multiple users changing the original without each implementation fighting over how the original should behave.

And in this case we really dont care since its an example.
Let's look at the source code.
```cs
public bool IsHealable(ItemType it)
{
	return it == ItemType.Medkit || it == ItemType.SCP500;
}
```
> Nortwood Studios ©

Let's create the patch class.
```cs
using CustomPlayerEffects;
using HarmonyLib;

[HarmonyPatch(typeof(Burned), nameof(Burned.IsHealable))]
internal static class BurnedIsHealeablePatch
{
    [HarmonyPrefix]
    internal static bool Prefix(Burned __instance)
    {
        return true;
    }
}
```
Well but how do we change the output since `IsHealeable` returns a bool? As we can read [here](https://harmony.pardeike.net/articles/patching-injections.html) there is smth called `__result` and this is used for changing the result.

```cs
using CustomPlayerEffects;
using HarmonyLib;

[HarmonyPatch(typeof(Burned), nameof(Burned.IsHealable))]
internal static class BurnedIsHealeablePatch
{
    [HarmonyPrefix]
    internal static bool Prefix(Burned __instance, ref bool __result)
    {
        return true;
    }
}
```
It's worth noting that the `__result` uses `ref` keyword before.
Now lets create the patch.
```cs
using CustomPlayerEffects;
using HarmonyLib;
using PlayerRoles;

[HarmonyPatch(typeof(Burned), nameof(Burned.IsHealable))]
internal static class BurnedIsHealeablePatch
{
    [HarmonyPrefix]
    internal static bool Prefix(Burned __instance, ref bool __result)
    {
        // We get the player so it's more readable
        ReferenceHub owner = __instance.Hub;
        // We check if the player is a facility guard
        if (owner.GetRoleId() == RoleTypeId.FacilityGuard)
        {
            // If they are we don't allow them to be healed
            __result = false;
            // And we return false so the original method doesn't run
            return false;
        }

        // If they aren't we return true so the original method runs
        return true;
    }
}
```

# Changing the arguments 
In this example we want to make it so whenever the server gives ammo to the player it gives twice as much.
Here is the source code of the method:
```cs
public static void ServerSetAmmo(this Inventory inv, ItemType ammoType, int amount)
{
	if (!NetworkServer.active)
	{
		throw new InvalidOperationException("Method ServerSetAmmo can only be executed on the server.");
	}
	amount = Mathf.Clamp(amount, 0, 65535);
	inv.UserInventory.ReserveAmmo[ammoType] = (ushort)amount;
	inv.SendAmmoNextFrame = true;
}
```
>Nortwood Studios ©

It's worth noting that its a static method so we won't be using `__instance` and `__return` since the base method is a `void`. And we won't be changing either the base method executes.

```cs
using HarmonyLib;
using InventorySystem;

[HarmonyPatch(typeof(InventoryExtensions), nameof(InventoryExtensions.ServerSetAmmo))]
internal static class InventoryExtensionServerSetAmmoPatch
{
    [HarmonyPrefix]
    internal static void Prefix()
    {
    }
}
```
Now we will add the arguments.
>[!IMPORTANT]
>They need to be of the same name as int the decompiled code!

We only care about the amount so its the only thing we will add.

```cs
using HarmonyLib;
using InventorySystem;

[HarmonyPatch(typeof(InventoryExtensions), nameof(InventoryExtensions.ServerSetAmmo))]
internal static class InventoryExtensionServerSetAmmoPatch
{
    [HarmonyPrefix]
    internal static void Prefix(ref int amount)
    {
        // We take the amount and we double it
        amount *= 2;
    }
}
```
We are using `ref` because we want to change the value. If we only wanted to read it it would'nt be necessary

[^1]:[Official guide](https://harmony.pardeike.net/articles/patching-prefix.html)
[^2]:[Prefix And Postif](PrefixAndPostfix.md)