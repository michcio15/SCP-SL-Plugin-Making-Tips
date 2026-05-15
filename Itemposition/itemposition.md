# How to get player's current item transform

>[!WARNING]
>The position is not exactly synced so there could be some small offsets and for some items the rotation could be wrong so be careful.

## Item

To get the items transform you will need to get players InventorySubcontroller.

```cs
if (player.RoleBase is not IFpcRole fpcRole)
{
    return;
}
if (fpcRole.FpcModule.CharacterModelInstance is not AnimatedCharacterModel characterModel)
{
    return;
}
if (!characterModel.TryGetSubcontroller(out InventorySubcontroller inventorySubcontroller))
{
    return;
}

return inventorySubcontroller._lastThirdpersonInstance._tr;
```
The code is not beatufil since it had to fit here and be readable.

## Firearm

For firearm you just do a lookup from a dictionary using the firearms serial. However the method for items should also work but i didnt test it.

```cs
Transform transform = FirearmWorldmodel.Instances[serial].transform;
```
Careful for an exception better use `TryGetValue`