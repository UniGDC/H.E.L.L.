#!/bin/sh

project="HELL-the-game"

echo "Attempting to build $project for Windows"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -batchmode \
    -nographics \
    -silent-crashes \
    -logFile "./Build/unity.log" \
    -projectPath "./" \
    -buildWindowsPlayer "./Build/windows/$project.exe" \
    -quit

echo "Attempting to build $project for OS X"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -batchmode \
    -nographics \
    -silent-crashes \
    -logFile "./Build/unity.log" \
    -projectPath "./" \
    -buildOSXUniversalPlayer "./Build/osx/$project.app" \
    -quit

echo "Attempting to build $project for Linux"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -batchmode \
    -nographics \
    -silent-crashes \
    -logFile "./Build/unity.log" \
    -projectPath "./" \
    -buildLinuxUniversalPlayer "./Build/linux/$project" \
    -quit
  
echo "Attempting to build $projecr for WebGL"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -batchmode \
    -nographics \
    -silent-crashes \
    -logfile "./Build/unity.log" \
    -projectPath "./" \
    -executeMethod "WebGLBuilder.Build" \
    -quit