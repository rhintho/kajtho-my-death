# My Death

# Unity version
2018.2.8f

# Installationsguide
## Android
 1. Go to ``File>BuildSettings``
 2. Under ``Platform``choose ``Android``
 3. Connect your mobile device to the PC
 4. Hit ``Build and run``

## iOS (wonky, since i dont have a mac)
 1. Go to ``File>BuildSettings``
 2. Under ``Platform``choose ``iOS``
 3. Connect your mobile device to the Mac
 4. Hit ``Build and run``
 5. XCode will open, make a coffe and wait
 6. Google any link-errors, probably need to grant camera rights to the app.
 7. Build

# App Guide  
## main scene: "main"
You will start on the home screen, every "app" is an inactive ``Prefab`` in the hierarchy.
The app buttons on the Homescreen will set each respective app (aka GameObject in hierarchy) to active.

So far we have no logic / story / states integrated

## Assets
* Modern UI Pack (ui assets and animations) https://assetstore.unity.com/packages/tools/gui/modern-ui-pack-114792
* FlowCanvas (not used yet) https://assetstore.unity.com/packages/tools/visual-scripting/flowcanvas-33903


As of 18.01.2019
