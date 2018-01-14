# PaToRo MonoGame-Template for 2D-Games developed for GameJams

## Prerequisites

* Git
* Visual Studio 2017
* MonoGame 3.6

## Structure

This solution consists of multiple vs-projects to support compiling for different platforms.

### Project "MonoGame-Content"

This is a *vs-shared-project*, ie. every file is in this project is soft-linked into each project, which
references this one.

It basically contains all assets and the MonoGame-Contentpipeline file.

### Project "MonoGame-Engine"

This project contains all engine-related classes thus providing a thin layer on top of Mono/XNA to support
rapid prototyping, and it is also a *vs-shared-project*.

### Project "MonoGame-Shared"

This project hosts the game-related classes and is realized as a *vs-shared-project*.

### Project "MonoGame-Desktop"

This is a platform dependent project, which contains the startup-code for Mono-Desktop-Applications using OpenGL.

## How-Tos

### Add assets to the content-pipeline

**TODO**

### Live reloading assets

Live-Reloading assets currently only works in Debug-Builds.

In the content-project there is a file called `assetmap.ini`, which contains a mapping of asset files.
For files to be reloadable, you need to pass the filename inculding extension to the contentmanager.

By pressing `F5` or saving changes to the `assetmap.ini` file, you trigger a hot-reload.

Have Fun !

### Add another platform project

**TODO**
