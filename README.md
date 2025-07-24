# RainCode
Turns files into colorful pixels and back.

## What it does
RainCode converts any file into a bitmap image by encoding each byte as colored pixels. Each bit gets represented by a specific color, creating a visual representation of your data that looks like abstract pixel art.

You can then decode the image back to recover the original file perfectly.

## Why this works
Every byte gets converted to 8-bit binary, with each '1' bit represented by a specific color:
- Red, Orange, Yellow, Green, Cyan, Blue, Purple for the 8 bit positions
- Pink pixels separate each byte
- Black pixel marks the end of data

The image dimensions are calculated as a square that can fit all the colored pixels needed. Each pixel contains actual data - nothing is wasted.

## Use cases
- **Data hiding**: Store files inside innocent-looking bitmap images
- **Data transmission**: Send files through image-only channels  
- **Backup encoding**: Convert important files to visual format
- **Steganography**: Hide data in plain sight as colorful pixel art
- **File conversion**: Alternative storage format for any data type

## How to use

Run the program and choose:
- **M** - Make raincode (encode file to image)
- **R** - Read raincode (decode image to file)

### Encoding
```
Enter input file's path: C:\document.pdf
Enter destination path: C:\output.bmp
```

### Decoding  
```
Enter input file's path: C:\encoded_image.bmp
Enter destination path: C:\recovered_document.pdf
```

## Technical details
- Each byte becomes 9 pixels (8 data bits + 1 separator)
- Only '1' bits get colored pixels - '0' bits are skipped for efficiency
- Square image dimensions calculated from total pixel count needed
- Perfect reconstruction guaranteed - no data loss
- Works with any file type (executables, documents, media, etc.)

## Example
A 212 KB Windows executable becomes a 64 KB bitmap file. The compressed size depends on data density - files with more zero bits create smaller images.

![ASCIIART2CW.exe](https://i.imgur.com/ku3kjbv.png)


## Requirements
- .NET Framework
- System.Drawing support
- Input file access
- Output directory write permissions

## Limitations
- Image size depends on input file size
- No compression beyond bit-skipping optimization
- Sequential encoding only (not random access)
- Basic error handling

## Technical note
This creates a direct bit-to-pixel mapping with visual separation between bytes. The resulting images often look like colorful noise or abstract art, making the data encoding non-obvious to casual observers.

The approach provides a unique way to store data in image format while maintaining perfect fidelity for reconstruction.
