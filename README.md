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

### Add another platform project

**TODO**
