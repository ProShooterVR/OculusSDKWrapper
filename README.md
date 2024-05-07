# OculusSDKWrapper
A wrapper for Oculus Platform SDK, with accessible and easy functions

![GitHub Downloads (all assets, latest release)](https://img.shields.io/github/downloads/superpanos/OculusSDKWrapper/latest/total)
[![Download](https://img.shields.io/badge/Download-blue.svg)](https://github.com/superpanos/OculusSDKWrapper/releases)
![Discord](https://img.shields.io/discord/1045754111842320414)

## Features ⚙️
- Get Oculus User Data (Oculus User ID/Oculus Username/Oculus Display name)
- IAPs Implementation
- Purchase Durable Item
- Purchase Consumable Item
- [PlayFabWrapper](https://github.com/superpanos/PlayFabWrapper) integration
### More to come

# How To Install and Configure📲
### Installation ⬇️:
1. Get and Download **Meta XR All-in-One SDK** from [Unity Asset Store](https://assetstore.unity.com/packages/tools/integration/meta-xr-all-in-one-sdk-269657)
2. Import **Meta XR All-in-One SDK**
3. On **GitHub**, go in **Releases**, download and import **OculusSDKWrapper.unitypackage**
4. Create an **Empty Game Object**, and put **OculusSDKWrapper** on it

### Platform SDK Configuration 🔧:
To get OculusSDKWrapper working we need to configure Platform SDK first

1. Go to **Meta Quest Developer Dashboard**, open your App, and find **API** tab, click it
2. In **API** tab, copy your **App ID**
3. Go in Unity, go to **Oculus** -> **Platform** -> click **Edit Settings**
4. In **Edit Settings** windows, put your **App ID** in **Meta Quest/2/Pro** field
5. Then, go to **Oculus** -> **Tools** -> click **Create store-compatible AndroidManifest.xml**
### Done!

### Data Use Checkup 📊
In order to use current **OculusSDKWrapper**, you must complete **Data Use Checkup**. You can do that by going to **Meta Quest Developer Dashboard** -> **Your App** -> **Requirements** -> **Data Use Checkup**, and completing **Data Use Checkup** for **User ID**, **User Profile**, and **In-App Purchases**. If you having troubles with **Data Use Checkup**, see [official documentation](https://developer.oculus.com/resources/publish-data-use/) on how to complete it

### OculusSDKWrapper Setup ✔️
1. Choose whether you want to use IAPs in your game, by setting **Use Oculus IAPs** 
2. If you chose to use IAPs in your game, then add all of your **Item SKUs** in **Items Skus** list

# Documentation 📑

#### (All the methods have summaries in the script too)
⚠️All variables are accessible through the instance. For example: `OculusSDKWrapper.instance.OculusUserData` is the Oculus User Data

### Get Oculus User Data

Saves:

- Saves received Oculus User Data in the instance (`OculusUserData` type of `oculusUserData`)

Example Usage:
```csharp
    OculusSDKWrapper.GetOculusUserData(r => {
        //success
        //you can access data from success event (r)
        //Example: r.OculusUserID
    }, () => {
        //error
    });
```

### Purchase Durable Item

Example Usage:
```csharp
    OculusSDKWrapper.PurchaseOculusDurable("itemSku", r => {
        //⚠️Do this if statement!
        if(r){
            //success
        } else {
            //error
        }
    }, () => {
        //error
    });
```

### Purchase Consumable Item

Uses `ConsumeOculusConsumablePurchase` from [PlayFabWrapper](https://github.com/superpanos/PlayFabWrapper) to consume purchase

Example Usage:
```csharp
    OculusSDKWrapper.PurchaseOculusConsumable("itemSku", r => {
        //⚠️Do this if statement!
        if(r){
            //success
        } else {
            //error
        }
    }, () => {
        //error
    });
```

# Contribution 🤝
You can contribute by suggesting changes, fixes, and new functiononality by suggesting it in `#unity-help` channel in my [discord](https://discord.gg/catosvr)

# Credits 🙌

- OculusSDKWrapper - [superpanos](https://github.com/superpanos) (me)
- Testing - [superpanos](https://github.com/superpanos) (me)
