# CLIInspection
Lightweight PE parser 

• Lightweight, efficient parser for MSDOS and PE executables.

• Parse 32 and 64bit image headers and sections.

• Detection of compiler, code packers and obfuscators using byte-signatures.

• Made from scratch heuristic scan engine of imports, strings and embedded executables and scripts (.AHK etc), capable of bypassing obfuscated Import Tables and packed/encrypted data.

• Heuristic scan of the code's structure based on entropy value to determine likelyhood of obfuscated code.


![Preview](https://i.imgur.com/Z0psGhC.jpeg)


• Entropy calculation is based on each individual section header. 

![Preview](https://i.imgur.com/gLN9YQX.jpeg)
