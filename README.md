# UDK Compression Tool

A static standalone application for compressing Unreal Development Kit (UDK) package files (`.upk` and `.udk`).

This tool provides a simple WPF interface for compressing packages using [RL-UPKSuite.Core](https://github.com/Martinii89/RL-UPKSuite/tree/master/Core) by **Martinii89**.

## Preview
<img width="524" height="100" alt="UDK_Compressor_Preview" src="https://github.com/user-attachments/assets/9bc327dd-3034-4f05-8e88-34f5f774fe09" />

## Usage
1. Download the latest executable from [releases](https://github.com/NathanKirby/UDK-Compressor/releases).
2. Launch **UDK Compressor**.
3. Click the **Choose UDK** button.
4. Choose the `.upk` or `.udk` file you wish to compress.
5. Click **Compress**.
6. The compressed package will be written to the same folder as the original file using this naming format:

Original:
```
MyMap.udk
```

Compressed:
```
MyMap_compressed.udk
```

The original package is **not** compressed or deleted.

## Compression Results
| Map | Before Compression | After Compression | Reduction |
|------|-------------------|------------------|-----------|
| Dribble Challenge E.Leclerc x Vitality | 600.3 MB | 233.5 MB | 61.10% |
| Lethamyr's Ice Rings | 320.6 MB | 161.7 MB | 49.53% |
| Nytro Hide n Seek | 712.7 MB | 234.3 MB | 67.13% |

## Q&A

### Q: Why is compressing my project important?
A: Compressing you project is important because it makes it easier and faster to share. Smaller files are especially useful for users with slower internet connections and help avoid file size limits on platforms like Discord.

### Q: Can I still open and edit my project after compression?
A: Yes, but it's possible that you'll have some crashes or visual issues until you save it again. This can be avoided by not compressing the version you're editing.

### Q: Will my map still be playable?
A: Yes, compression does not affect playability.

### Q: When should I compress my projects?
A: Compress your project before uploading or sending to others.

### Q: How long does compression take?
A: Compression only takes a few seconds. The app may say that it is not responding, but give it a moment and it will finish.

## Disclaimer
This project was thrown together very quickly without any care given to syntax, documentation, readability, etc. If you're planning on cloning this repo, just know that it needs some work to be presentable.
