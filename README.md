# OxidePack - Plugin packer for Oxide

Simply pack more than one .cs files into one plugin.

A little bit about OxidePack.
OxidePack primarily simplifies the development of plugins for Oxide (uMod).
It has both server and client side.
P.s. For privacy you can not worry, the server does not collect data from your plugins, their errors, and certainly does not distribute them.
Features:
1) Building plugins from multiple files (modularity).
2) Ready modules (Configuration).
3) The whole cycle from the creation of folders for the project to the creation of plugin files is automated.
4) Automatic pick-up of changes from the configuration files.
6) Quick and easy management of the project's dependencies with plugins (download/update/connect).
7) Obfuscation or "Encryption" of the plugin in 1 click.
8) Automatically increase the version of the plugin when building
9) If there is an error, the program will show a window with the contents of the plugin and a table of errors. Double-click on the table line, the program moves the pointer in a text editor and highlights the error location.


- toools/Protobuf - Protocol between Client (WinForms) and Server (.NetCore)

- OxidePack.Client - Client (WinForms)
- OxidePack.Common - Common classes for client & server
- OxidePack.CoreLib.Benchmarks - Benchmark
- OxidePack.CoreLib - Build & Encrypt plugin
- OxidePack.Server - TCP server recieves tasks from client and return result of plugin building
