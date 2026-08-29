# EB Toolbox

<a href="https://github.com/EhabYT/.github/blob/main/profile/CODE_OF_CONDUCT.md"><img alt="Code of Conduct" src="https://img.shields.io/badge/Contributor%20Covenant-2.1-4baaaa.svg?style=for-the-badge&color=1A91FF" /></a>
<img alt="Version" src="https://img.shields.io/badge/version-v0.1.16-blue?style=for-the-badge&color=1A3A7A" />
<img alt="Platform" src="https://img.shields.io/badge/platform-Windows%2010%2F11-0078D4?style=for-the-badge" />
<img alt="Build" src="https://img.shields.io/badge/build-passing-brightgreen?style=for-the-badge" />

This repository contains the **EB Toolbox** (EBOS Edition) made with C# and WinUI 3 / WindowsAppSDK.

**Supports all EBOS versions.** The compatibility check accepts any EBOS version (a `*` wildcard in `app.config:4` `EBVersion`), and a missing/undetected EBOS install no longer blocks startup.

**Want to contribute?** Please check out the [EB Contribution Guidelines](https://docs.ebos.net/contributions) for more information.

**Translations:** [Crowdin Project](https://crowdin.com/project/eb-toolbox-test-1).

## Screenshots

| Home | Config | Settings |
|------|--------|----------|
| ![Home](EBToolbox/Assets/EB-Banner.png) | ![Settings](EBToolbox/Assets/Square150x150Logo.scale-200.png) | ![Logo](EBToolbox/Assets/Logo/eb-logo.png) |

*UI v0.1.15: EB theme `#1A3A7A`, improved Home header, TileGallery, SettingsCards, Mica backdrop.*

## Changelog

### v0.1.16 - .NET 10 Upgrade
- Upgrade `net8.0` → `net10.0` (`EBToolbox.csproj:5`), `setup-dotnet v3 8.0.x` → `v4 10.0.x` (`build.yml:21`)
- NuGet: `WinAppSDK 1.7→2.4`, `Toolkit 8.1→8.2`, `NLog 5.4→6.2`, `Extensions 9→10`, `Win2D 1.3→1.4`, `BuildTools 10.0.26100→10.0.28000`, `WinUIEx 2.5→2.9`

### v0.1.15 - UI Update
- EB theme (`App.xaml:19` `EBPrimaryColor #1A3A7A`), new banner `#EAF2FF → #B8D0F5` Light / `#1A3A7A` Dark (`HomePageHeaderImage.xaml:20`)
- Redesigned `MainWindow.xaml:24` TitleBar, `HomePage.xaml:50` header, `HeaderTile.xaml:12` cards
- GitHub org `EB-OS` → `EhabYT` (`https://github.com/EhabYT/EB-Toolbox`), Discord `discord.ebos.net` → `discord.com/invite/3TdxfzrYwf`
- Rebrand `Atlas` → `EB` (namespace `EBToolbox`, `EBToolbox-WinUI3.sln`, assets)

### v0.1.14 - GitHub + Discord
- Fixed GitHub URLs and Discord invite
### v0.1.13 - Initial EB Rebrand
- Initial fork from AtlasOS

## How do I contribute?

The EB Toolbox uses C#, which is best programmed using [Visual Studio](https://visualstudio.microsoft.com/vs/), 

1. Once you've cloned the repository, you can run the `EBToolbox-WinUI3.sln` file to launch the solution and code.

You can now make all your changes and view them live!

## Credits
- [WinUI 3 Gallery](https://apps.microsoft.com/detail/9P3JFPWWDZRC) for many aspects of the Toolbox and for the home page tile galery
- [Windows Community Toolkit Gallery](https://apps.microsoft.com/detail/9NBLGGH4TLCQ) for aspects of the app such as settings cards and expanders

## 💙 Contributors
<a href="https://github.com/EhabYT/EB-Toolbox/graphs/contributors" target="_blank"><img src="https://contrib.rocks/image?repo=EhabYT/EB-Toolbox&columns=18" alt="Avatars of all contributors"></a>
