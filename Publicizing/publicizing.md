# How to publicize `Assembly-CSharp`?

## Why should you publicize your assembly
It gives you as a developer more space to have in controll of the game. Almost every plugin advanced or not uses publicized assembly.

## How to do it?

This method uses [BepInEx Publicizer](https://github.com/BepInEx/BepInEx.AssemblyPublicizer). You will need to download it from nuget package manager. If you dont know how to do it watch tutorial for [VS](https://www.youtube.com/watch?v=h8_1z3qXDqs) or for [Rider](https://youtu.be/JNPtcOXpBSk?t=14). We will download the one named `BepInEx.AssemblyPublicizer.MSBuild` version `0.4.3`.

Your `<ItemGroup>` should look have this in it.
```xml
    <PackageReference Include="BepInEx.AssemblyPublicizer.MSBuild" Version="0.4.3">
          <PrivateAssets>all</PrivateAssets>
          <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
```

After you have downloaded it go to your `.csproj` file and add this. The main concern is adding `<AllowUnsafeBlock>true</AllowUnsafeBlocks>`
```xml
    <PropertyGroup>
        <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
    </PropertyGroup>
```
After you have done that head over to your references and add `Publicize="true"` to it. This is how it looks like in my projects.
```xml
    <Reference Include="Assembly-CSharp" Publicize="true">
            <HintPath>$(SL_REFERENCES)\Assembly-CSharp.dll</HintPath>
    </Reference>
```
Now the project should compile and you should have acces to all the `private` / `internal` / `protected` stuff from the game.