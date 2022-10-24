 public static void DisplayData32()
        {
            Console.WriteLine(Environment.NewLine + "[] --- 32bit Information --- []");
            Console.WriteLine("Image Size: " + FileSize / 1000000 + "mb");
            Console.WriteLine("Sections Count: " + ImageNTHeader32.FileHeader.NumberOfSections);
            Console.WriteLine("TimeDateStamp: " + ImageNTHeader32.Signature);
            Console.WriteLine("AddressOfEntryPoint: " + ImageNTHeader32.OptionalHeader.AddressOfEntryPoint);
            Console.WriteLine("Magic: " + ImageNTHeader32.OptionalHeader.Magic);
            Console.WriteLine("Subsystem: " + ImageNTHeader32.OptionalHeader.Subsystem);
            Console.WriteLine("DllCharacteristics: " + ImageNTHeader32.OptionalHeader.DllCharacteristics);
            Console.WriteLine("NumberOfRvaAndSizes: " + ImageNTHeader32.OptionalHeader.NumberOfRvaAndSizes);
            Console.WriteLine("[] ------------------------- []");
        }
        public static void DisplayData64()
        {
            Console.WriteLine(Environment.NewLine + "[] --- 64bit Information --- []");
            Console.WriteLine("Image Size: " + FileSize / 1000000 + "mb");
            Console.WriteLine("Sections Count: " + ImageNTHeader64.FileHeader.NumberOfSections);
            Console.WriteLine("TimeDateStamp: " + ImageNTHeader64.Signature);
            Console.WriteLine("AddressOfEntryPoint: " + ImageNTHeader64.OptionalHeader.AddressOfEntryPoint);
            Console.WriteLine("Magic: " + ImageNTHeader64.OptionalHeader.Magic);
            Console.WriteLine("Subsystem: " + ImageNTHeader64.OptionalHeader.Subsystem);
            Console.WriteLine("DllCharacteristics: " + ImageNTHeader64.OptionalHeader.DllCharacteristics);
            Console.WriteLine("NumberOfRvaAndSizes: " + ImageNTHeader64.OptionalHeader.NumberOfRvaAndSizes);
            Console.WriteLine("[] ------------------------- []");
        }
private static bool ParseDOSHeader()
        {
            ImageDOS = fromBytes<IMAGE_DOS_HEADER>(ImageBytes);
            if (ImageDOS.e_lfanew == 0 || ImageDOS.e_magic == 0)
            {
                
                return false;
            }
            return true;
        }
        private static bool ParseMagic()
        {
            ImageMagic = fromBytes<IMAGE_MAGIC>(ImageBytes.Skip(ImageDOS.e_lfanew + 24).ToArray());
            if (ImageMagic.Magic == 0x10b)
            {
                is64Bit = false;
                ObjectHeader = ImageNTHeader32;
                return true; 
            }else if (ImageMagic.Magic == 0x20b)
            {
                is64Bit = true;
                ObjectHeader = ImageNTHeader64;
                return true;
            }
            return false;
        }
        private static bool ParseNTHeader()
        {
            if (is64Bit)
            {
                ImageNTHeader64 = fromBytes<IMAGE_NT_HEADERS64>(ImageBytes.Skip(ImageDOS.e_lfanew).ToArray());
                return true;
            }
            else
            {
                ImageNTHeader32 = fromBytes<IMAGE_NT_HEADERS32>(ImageBytes.Skip(ImageDOS.e_lfanew).ToArray());
                return true;
            }
        }
public static List<String> ParseNormal(string path)
        {
            List<String> Information = new List<string>();
            ImageBytes = GetFileBytes(4092, 0, path);
            if (ImageBytes == null)
            {
                Information.Add("[ERROR] File could not be accessed or read for normal parsing.");
                return Information;
            }
            if (ParseDOSHeader())
            {
                Information.Add("[INFO] Image DOS Header Parsed Successfully.");
            }
            else
            {
                //Corrupted? Return.
                Information.Add("[ERROR] Image DOS Header Cannot be Parsed.");
                ResetMemory();
                return Information;
            }
            if (ValidatePEMagic && ParseMagic())
            {
                Information.Add("[INFO] Image Magic Parsed Successfully.");
            }
            else
            {
                Information.Add("[ERROR] Image Magic Cannot be Parsed.");
            }

            if (ParseNTHeader())
            {
                Information.Add("[INFO] NT Header Parsed Successfully. Is 64bit: " + is64Bit);  
            }
            else
            {
                Information.Add("[ERROR] NT Header Cannot be Parsed");
            }
            return Information;
        }
		
		