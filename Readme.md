# Minimal C++ Plugin for Unity3D

[Christian Schladetsch](mailto:christian@twobulls.com) - TwoBulls

This document will guide you through the minimal steps needed to write and use a C++ plugin from Unity3d.

**Software Used**:

* Unity3d v5.5.1f1 app
* Unity3d VScode asset v2.7
* VScode v1.8 app, with following extensions
	* C/C++ v0.10.0 **ms-vscode.csharp**
	* C# Extensions v1.1.0 **jchannon.csharpextensions**
	* CMake v0.0.15 **twxs.cmake**
	* CMake Tools v0.8.8 **vector-of-bool.cmake-tools**

Note that we are using *VScode* to both write the Unity scripts, and to write the C++ plugin.

We are also using VScode to do the build process for the plugin itself.

## Make the folder structure

	$ mkdir PluginDemo
	$ mkdir !$/CppPlugin

## Setup Unity3d
Make a new Unity3d project in the PluginDemo folder called Unity.

Make a new script called *Main.cs*. Attach it to the *Main Camera* in the Scene.

Open *Main.cs* in **VScode**. Replace the contents of the file with the following:

	using System.Runtime.InteropServices;
	using UnityEngine;
	
	public class Main : MonoBehaviour
	{
			[DllImport("CppPlugin")]
			private static extern int Extern1(int n);
	
			void Awake()
			{
				Debug.Log(Extern1(42));
			}
	}

## Setup VScode
Open VScode and create a New Window. Add a new file called Plugin.cpp, containing:

	extern "C"
	{
		int Extern1(int n)
		{
			return n*2;
		}
	}

Create anoter file called CMakeLists.txt. This is used like *make*.

	cmake_minimum_required(VERSION 3.6)
	project(CppPlugin)
	
	set(SOURCE_FILES Plugin.cpp)
	set(TARGET_PATH ${CMAKE_HOME_DIRECTORY}/../Unity/Assets/Plugins)

	# un-mangle the output plugin name
	set(CMAKE_SHARED_LIBRARY_PREFIX "")
	set(CMAKE_SHARED_LIBRARY_SUFFIX ".bundle")

	# write it directly into our Unity project
	set(LIBRARY_OUTPUT_PATH ${TARGET_PATH})
	
	add_library(${PROJECT_NAME} SHARED ${SOURCE_FILES})
	
Close VScode, and re-open it. This allows it to see it as a CMake-based project and you'll get the controls in the status bar at the bottom. 

Press the 'Build' button in the bottom status bar, or press **F7**. You should see something like:

	Scanning dependencies of target CppPlugin
	[ 50%] Building CXX object CMakeFiles/CppPlugin.dir/Plugin.cpp.o
	[100%] Linking CXX shared library /Users/christian/Local/PluginDemo/Unity/Assets/Plugins/CppPlugin.bundle
	[100%] Built target CppPlugin
	[vscode] cmake exited with return code 0

Ok, so now we have:

* A Unity Window open
* A VScode window open, editing Main.cs
* Another VScode window open, making the plugin

## Trying it out

Go back to Unity, Save the scene, and run it.

The console should display a single Log message saying '84', which is twice the 42 you passed to the Extern1 C++ function.

# Gotchas

* Sometimes, Unity3d doesn't pickup changes make to remade plugins. 
* Even re-importing either the plugin, the Plugins folder, or all Assets, does nothing 
	* But sometimes it does.
* The only "fix" for this seems to be to delete the Unity/Library folder and restart Unity3d.
* By default, CMake will call your shard library 'libPlugin.dylib'. 
	* Unity doesn't understand .dylib, and the leading lib is useless. 
	* So I changed the CMakeLists.txt file to name it correctly, and also 
	* write it directly to Unity's Plugin folder.
* The plugin must support at least 64-bit to work in the Editor.
	* You can change the word size per-platform for each plugin/platform combination. 
* I am still working on marshalling structures, arrays and arrays of stuctures between Unity3d and C++, and also accessing Unity systems like Debug.Log fro C++.


# Why do this at all?

There are decades worth of great C++ libraries out there. 

Being able to use them quickly and with a tiny, well-practised setup procedure gives us access to those libraries.

Also, for some tasks you need simple speed and/or memory management and for that C++ is great.

Note everything in this document works for C as well.
