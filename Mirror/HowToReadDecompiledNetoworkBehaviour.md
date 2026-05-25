# How to read decompiled NetworkBehaviours?
While compiling mirror adds their code to make it work.

## What is added?
This here is the original code:

```cs
using Mirror;
using UnityEngine;

public class ExampleNetworkBehaviour : NetworkBehaviour
{
    [SyncVar] public string SyncString;
    [SyncVar(hook = nameof(SetInt))] public int SyncInt;

    public void SetInt(int oldSyncInt, int newSyncInt)
    {
        Debug.Log($"SyncInt changed from {oldSyncInt} to {newSyncInt}");
    }

    [Command]
    // Here we as a client tell the server that we have show a weapon
    // This code will be executed on the server
    public void CmdShoot()
    {
        Debug.Log("Hello server, i have shot a weapon");
        RpcShot(1);
    }

    [ClientRpc]
    // Here we send all the clients (because of [ClientRpc]) this owner has shot x amount of bullets
    // This code will be executed on all clients
    public void RpcShot(int amount)
    {
        Debug.Log($"Client has shot {amount} bullets");
        SpawnBullet(amount);
    }

    private void SpawnBullet(int amount)
    {
        throw new System.NotImplementedException();
    }
}
```
Right now we don't about the comments. Now let's take a look at the code decompiled by dnSpy:
```cs
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class ExampleNetworkBehaviour : NetworkBehaviour
{
	public void SetInt(int oldSyncInt, int newSyncInt)
	{
		Debug.Log(string.Format("SyncInt changed from {0} to {1}", oldSyncInt, newSyncInt));
	}

	[Command]
	public void CmdShoot()
	{
		NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void ExampleNetworkBehaviour::CmdShoot()", 2023791171, networkWriterPooled, 0, true);
		NetworkWriterPool.Return(networkWriterPooled);
	}

	[ClientRpc]
	public void RpcShot(int amount)
	{
		NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get();
		NetworkWriterExtensions.WriteVarInt(networkWriterPooled, amount);
		this.SendRPCInternal("System.Void ExampleNetworkBehaviour::RpcShot(System.Int32)", -618949732, networkWriterPooled, 0, true);
		NetworkWriterPool.Return(networkWriterPooled);
	}

	private void SpawnBullet(int amount)
	{
		throw new NotImplementedException();
	}

	public ExampleNetworkBehaviour()
	{
		this._Mirror_SyncVarHookDelegate_SyncInt = new Action<int, int>(this.SetInt);
	}

	public override bool Weaved()
	{
		return true;
	}

	public string NetworkSyncString
	{
		get
		{
			return this.SyncString;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.SyncString, 1UL, null);
		}
	}

	public int NetworkSyncInt
	{
		get
		{
			return this.SyncInt;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.SyncInt, 2UL, this._Mirror_SyncVarHookDelegate_SyncInt);
		}
	}

	protected void UserCode_CmdShoot()
	{
		Debug.Log("Hello server, i have shot a weapon");
		this.RpcShot(1);
	}

	protected static void InvokeUserCode_CmdShoot(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdShoot called on client.");
			return;
		}
		((ExampleNetworkBehaviour)obj).UserCode_CmdShoot();
	}

	protected void UserCode_RpcShot__Int32(int amount)
	{
		Debug.Log(string.Format("Client has shot {0} bullets", amount));
		this.SpawnBullet(amount);
	}

	protected static void InvokeUserCode_RpcShot__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcShot called on server.");
			return;
		}
		((ExampleNetworkBehaviour)obj).UserCode_RpcShot__Int32(NetworkReaderExtensions.ReadVarInt(reader));
	}

	static ExampleNetworkBehaviour()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(ExampleNetworkBehaviour), "System.Void ExampleNetworkBehaviour::CmdShoot()", new RemoteCallDelegate(ExampleNetworkBehaviour.InvokeUserCode_CmdShoot), true);
		RemoteProcedureCalls.RegisterRpc(typeof(ExampleNetworkBehaviour), "System.Void ExampleNetworkBehaviour::RpcShot(System.Int32)", new RemoteCallDelegate(ExampleNetworkBehaviour.InvokeUserCode_RpcShot__Int32));
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteString(this.SyncString);
			NetworkWriterExtensions.WriteVarInt(writer, this.SyncInt);
			return;
		}
		NetworkWriterExtensions.WriteVarULong(writer, this.syncVarDirtyBits);
		if ((this.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteString(this.SyncString);
		}
		if ((this.syncVarDirtyBits & 2UL) != 0UL)
		{
			NetworkWriterExtensions.WriteVarInt(writer, this.SyncInt);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.SyncString, null, reader.ReadString());
			base.GeneratedSyncVarDeserialize<int>(ref this.SyncInt, this._Mirror_SyncVarHookDelegate_SyncInt, NetworkReaderExtensions.ReadVarInt(reader));
			return;
		}
		long num = (long)NetworkReaderExtensions.ReadVarULong(reader);
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.SyncString, null, reader.ReadString());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.SyncInt, this._Mirror_SyncVarHookDelegate_SyncInt, NetworkReaderExtensions.ReadVarInt(reader));
		}
	}

	[SyncVar]
	public string SyncString;

	[SyncVar(hook = "SetInt")]
	public int SyncInt;

	public Action<int, int> _Mirror_SyncVarHookDelegate_SyncInt;
}
```
As we can see the decompiled code is ~2 as long as the original. Now we will look at every piece alone

## SyncVars
In the original code we have 2 sync vars one with hook one with without 
```cs
[SyncVar] public string SyncString;
[SyncVar(hook = nameof(SetInt))] public int SyncInt;
```
and in the decompiled we also have them
```cs
[SyncVar]
public string SyncString;

[SyncVar(hook = "SetInt")]
public int SyncInt;
```
however now there also came new properties with *Network* prefix.
```cs
    public string NetworkSyncString
	{
		get
		{
			return this.SyncString;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.SyncString, 1UL, null);
		}
	}

	public int NetworkSyncInt
	{
		get
		{
			return this.SyncInt;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.SyncInt, 2UL, this._Mirror_SyncVarHookDelegate_SyncInt);
		}
	}
```
let's analyze the code on the example of *NetworkSyncInt*
```cs
    public int NetworkSyncInt
	{
		get
		{
            // We return the field that is the sync var.
			return this.SyncInt;
		}
		[param: In]
		set
		{
            // If we are setting it we mirrors method which gives us information about the dirty bit and hook (or we can get from name on the syncvar)
			base.GeneratedSyncVarSetter<int>(
            /*The value we are setting*/ value,
            /*What will be set*/ ref this.SyncInt,
            /*Dirty bit usefull for fake syncing is equal to 1UL << 1*/ 2UL, 
            /*What method / delagate will be called when its changed*/ this._Mirror_SyncVarHookDelegate_SyncInt);
		}
	}
```
We can see that here it will raise `_Mirror_SyncVarHookDelegate_SyncInt` which calls `public void SetInt(int oldSyncInt, int newSyncInt)`
also its easy to conclude that if we want to change somthing using our plugin like admintoys position we will use NetworkPosition not Position.

## Cmd / RPC
Cmd's are used by the client to tell the server that they have done something.
Rpc's are divided into [TargetRpc] which are sent only to one client and [ClientRpc] which are sent to all of the client's.
The rpc's are executed on the client are are mostly used when server tell's client to do something like spawn a bullet. 
[More about it here.](https://mirror-networking.gitbook.io/docs/manual/guides/communications/remote-actions)

In this example we will use only Cmd but its the same for Rpc's.
```cs
    [Command]
	public void CmdShoot()
    {
        Debug.Log("Hello server, i have shot a weapon");
        RpcShot(1);
    }
```
Decompiled:
```cs
    [Command]
	public void CmdShoot()
	{
		NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get();
        // Under the hood it's just a NetworkMessage, so e.g Rpc calls can be faked.
		base.SendCommandInternal("System.Void ExampleNetworkBehaviour::CmdShoot()", 2023791171, networkWriterPooled, 0, true);
		NetworkWriterPool.Return(networkWriterPooled);
	}
```
we can see here that this is **NOT** the original code so where is it?.
There are 2 ways to look for it.
- Look by the name. They will have the `CmdShoot` in them but with some prefixes like `InvokeUserCode_` or `UserCode_`. More on them  later.
- Try to find them in the static constructor of this class.
We will choose the second method. 
```cs
    static ExampleNetworkBehaviour()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(ExampleNetworkBehaviour), "System.Void ExampleNetworkBehaviour::CmdShoot()", new RemoteCallDelegate(ExampleNetworkBehaviour.InvokeUserCode_CmdShoot), true);
		RemoteProcedureCalls.RegisterRpc(typeof(ExampleNetworkBehaviour), "System.Void ExampleNetworkBehaviour::RpcShot(System.Int32)", new RemoteCallDelegate(ExampleNetworkBehaviour.InvokeUserCode_RpcShot__Int32));
	}
```
Here there is `RemoteProcedureCalls.RegisterCommand` using the same string `System.Void ExampleNetworkBehaviour::CmdShoot()` as in the `CmdShoot()`. That's how we can find them. Let's now trace what is it calling.
```cs
    // The actual code
	protected void UserCode_CmdShoot()
	{
		Debug.Log("Hello server, i have shot a weapon");
		this.RpcShot(1);
	}

    // This is a safe guard so we are sure that the code WONT be executed on the client.
	protected static void InvokeUserCode_CmdShoot(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdShoot called on client.");
			return;
		}
		((ExampleNetworkBehaviour)obj).UserCode_CmdShoot();
	}
```
It turn's out that the original method code is made `protected` and has added `UserCode_` prefix.
To be sure that the Rpc's are the same we will look at them.

Original:
```cs
    [ClientRpc]
    // Here we send all the clients (because of [ClientRpc]) this owner has shot x amount of bullets
    // This code will be executed on all clients
    public void RpcShot(int amount)
    {
        Debug.Log($"Client has shot {amount} bullets");
        SpawnBullet(amount);
    }
```

Decompiled:
```cs
    [ClientRpc]
	public void RpcShot(int amount)
	{
		NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get();
		NetworkWriterExtensions.WriteVarInt(networkWriterPooled, amount);
		this.SendRPCInternal("System.Void ExampleNetworkBehaviour::RpcShot(System.Int32)", -618949732, networkWriterPooled, 0, true);
		NetworkWriterPool.Return(networkWriterPooled);
	}
    ...

    protected void UserCode_RpcShot__Int32(int amount)
	{
		Debug.Log(string.Format("Client has shot {0} bullets", amount));
		this.SpawnBullet(amount);
	}

	protected static void InvokeUserCode_RpcShot__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
        // Make sure that its not called on the server
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcShot called on server.");
			return;
		}
		((ExampleNetworkBehaviour)obj).UserCode_RpcShot__Int32(NetworkReaderExtensions.ReadVarInt(reader));
	}
```
