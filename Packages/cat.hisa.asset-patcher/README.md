# AssetPatcher

[![GitHub Release][shields-release]][github-release]

[shields-release]: https://img.shields.io/github/v/release/hisacat/Unity-AssetPatcher
[github-release]: https://github.com/hisacat/Unity-AssetPatcher/releases/latest

Tools for create and apply asset patch in Unity

You can also import this package with [VCC](https://hisacat.github.io/Unity-AssetPatcher)

<a href="https://www.buymeacoffee.com/HisaCat"><img src="https://img.buymeacoffee.com/button-api/?text=Buy me Milk&emoji=🥛&slug=HisaCat&button_colour=bd5fff&font_colour=ffffff&font_family=Bree&outline_colour=000000&coffee_colour=ffffff" /></a>

---
## Useage (Apply Patch)
* For single patch asset:
    - Select patch asset and click `Patch` button.
* For patch list asset:
    - Select patch list asset and click `Patch All` button.

## Useage (Create Patch)
1. In the top menu, find and click`Tool/AssetPatcher/Create Asset Patch`.
2. Place the original asset in `Origin File` and the modified asset in `Edited File`.
3. If you want to include the meta file of the modified asset, check `Save meta file?` (It is recommended that you check it by default.)
4. If you want to keep the GUID of the modified asset, check `Keep GUID?` (If `Save meta file?` is activated, it is automatically checked.)
5. Click the `Create Patch File` button and select the creation path for the patch asset.
6. A patch asset and a folder named "(asset name)-datas" will be created in corresponding path. This folder is a file needed for the patch, so please attach it when distributing.

Additionally, if there are multiple patch files, you can make patch files into one that called 'Patch list asset' for better user convenience.
1. Find and select `Tool/AssetPatcher/Create Asset Patch List` from the top menu.
2. Insert the patch files created through `Create Asset Patch` into Patches.
3. Click the `Create Patch List File` button and select the creation path for the patch list asset.
4. A 'Patch list asset' will be created in the corresponding path.

Additionally, When `Edited File` is modified after creating the patch file, it provides a function to update the patch asset without creating a new patch file.

1. Find and select `Tool/AssetPatcher/Update Asset Patch` from the top menu.
2. Place Patch assets that need updating into `Patches`.  
    * If you have Patch list asset, you can place all included Patch assets with place Patch list asset in `Load from Patch List` below.
3. Click the `Update patch files` button to update the placed patch assets.

---
## Mirrors
[Booth](https://hisacat.booth.pm/items/5612806)

---
## Credit

sisong's [HDiffPatch](https://github.com/sisong/HDiffPatch)

---
## Contact
HisaCat

ahisacat@gmail.com

[ [Github](https://github.com/hisacat) | [Booth](https://hisacat.booth.pm) | [Twitter](https://twitter.com/ahisacat) | [Discord](https://discord.com/users/295816286213242880) ]
