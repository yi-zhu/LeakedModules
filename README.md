# EmuTarkov-Modules
Client-side runtime patches to alter the client's behaviour.

## Modules
- EmuTarkov.Common: utilities used across modules.
- EmuTarkov.Core: required patches for the game to run.
- EmuTarkov.SinglePlayer: simulating online game offline.

## Setup
All dependencies are provided, no additional setup required.

## Build
1. Visual Studio -> menubar -> rebuild.
2. Copy-paste `Shared/Nlog.config.nlog` into `Build/<Configuration>/`.
3. Copy-paste `Shared/References/Assembly-CSharp.dll` into `Build/<Configuration>/`.

## Run
1. Follow the build instructions
2. Copy-paste all files inside `Build/<Configuration>/` into `EscapeFromTarkov_Data/Managed/`, overwrite when prompted.

## Credits
- stx09: almost all the patches
- craik: almost all the patches
- InNoHurryToCode: initial code + refactoring into modules