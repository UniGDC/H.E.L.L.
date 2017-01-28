#!/bin/sh

##
# Unity Download Script for Travis CI Objective-C deploy environment
#
# This script downloads Unity Editor from Unity website.
# The download link template looks like this:
#   http://download.unity3d.com/download_unity/$UNITY_HASH/MacEditorInstaller/Unity-$UNITY_VERSION.pkg
# 
# Export $UNITY_HASH and $UNITY_VERSION to set the target Unity version.
# Export $UNITY_BASEURL if the base url is changed.

##
# This repository uses the following $UNITY_HASH and $UNITY_VERSION.
# http://netstorage.unity3d.com/unity/88d00a7498cd/MacEditorInstaller/Unity-5.5.1f1.pkg
#
# $UNITY_HASH = 88d00a7498cd
# $UNITY_VERSION = 5.5.1f1
#
# Additional package requirement
# Check http://netstorage.unity3d.com/unity/88d00a7498cd/unity-5.5.1f1-osx.ini for more packages.
#   - UnityEditor WebGL Build support
#   - UnityEditor Windows Build support
#   - UnityEditor OSX Build support
#   - UnityEditor Linux Build support


install_extras() {
    filename="MacEditorTargetInstaller/UnitySetup-$1-Support-for-Editor-$UNITY_VERSION.pkg"
    url="$UNITY_BASEURL/$UNITY_HASH/$filename"
    echo "Downloading package `basename $filename`..."
    curl -o `basename $filemane` $url
    echo
    echo "Installing `basename $filename`..."
    sudo installer -dumplog -package `basename "$filename"` -target /
}

if `test -z $UNITY_VERSION` || `test -z $UNITY_HASH` ; then
    echo "Either UNITY_VERSION or UNITY_HASH environment variable is not set. Have you exported those variables?"
    echo "Continue building with a default Unity version."
    UNITY_HASH="88d00a7498cd"
    UNITY_VERSION="5.5.1f1"
    echo "The default Unity version is selected. Attempting to build using Unity version $UNITY_VERSION..."
    echo
    echo
fi

if `test -z $UNITY_BASEURL` ; then
    echo "UNITY_BASEURL is not defined. Using default value..."
    UNITY_BASEURL="https://download.unity3d.com/download_unity"
    echo
    echo
fi

echo "Installing Unity3D version $UNITY_VERSION on Travis CI virtual machine."
echo "The hash value of this version is $UNITY_HASH."

echo

echo "Builing URL from the environment variables..."
filename="MacEditorInstaller/Unity-$UNITY_VERSION.pkg"
url="$UNITY_BASEURL/$UNITY_HASH/$filename"

echo

echo "Downloading Unity Editor `basename $filename` from $url..."
curl -o `basename $filename` $url

echo

echo "Installing `basename $filename`..."
sudo installer -dumplog -package `basename "$filename"` -target /

echo
echo

echo "Unity Editor is installation finished."
echo "Installing additionally required packages..."

##
# The package name is case-sensative. Watch out when typing.
##

install_extras "WebGL"
install_extras "Windows"
install_extras "Linux"

echo "Finished the installation process."