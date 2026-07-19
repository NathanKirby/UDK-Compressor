# UDK Compression Tool

A static standalone application for compressing Unreal Development Kit (UDK) package files (`.upk` and `.udk`).

This tool provides a simple WPF interface for compressing packages using `RlUpk.Core` by **Martinii89**.

## Usage
1. Download the latest executable from https://github.com/NathanKirby/UDK-Compressor/releases
2. Launch **UDK Compressor.exe**.
3. Click the **Choose UDK** button.
4. Choose the `.upk` or `.udk` file you wish to compress.
5. Click **Compress**.
6. The compressed package will be written to the same folder as the original file using the naming format:

Original:
```
MyMap.udk
```

Compressed result:
```
MyMap_compressed.udk
```

The original package is **not** compressed or deleted.

## Requirements

* Windows 10 or later
* 64-bit operating system

The standalone release includes the .NET runtime and requires no additional installation.

## Credits

Compression functionality is powered by **RlUpk.Core**.

Additional open-source libraries include:

* Syroot.BinaryData
* LazyCache
* Microsoft.Extensions.DependencyInjection
* Microsoft.Extensions.FileSystemGlobbing

See the application's **Licenses** section for third-party license information.

## License

This project is licensed under the license included in this repository.

Third-party components are licensed under their respective licenses, which are included with the application.
