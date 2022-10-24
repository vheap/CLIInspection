public static bool IsPEFile(string file)
        {
            byte[] buffer = new byte[2];
            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                fileStream.Read(buffer, 0, buffer.Length);
            if (buffer[1] == (byte) 90)
            {
                return buffer[0] == (byte)77;
            }     
            return false;
        }
 public static int GetSectionIndex(int rva)
        {
            //Used to return the index of a section in the list.
            //To be used to directly use the section from IMAGE_SECTION_HEADER[]
            //Like: IMAGE_SECTION_HEADER[Index].VirtualAddress
            int x = 0;
            foreach (var i in ImageSectionHeaders)
            {
                if (i.VirtualAddress == rva)
                {
                    return x; 
                }
                x++;
            }
            return 0;
        }
public static int GetSectionFromRVA(int rva)
        {
            //See which section contains the given rva, then return that section's own rva.
            foreach (var i in PEHeaders)
            {
                if (i.Value > rva)
                {
                    return (int)i.Value;
                }
            }
            return 0;
        }
public static Object GetSectionRVAObj(int rva)
        {
            //Retrieve the section where this rva resides.
            int x = 0;
            foreach (var i in ImageSectionHeaders)
            {
                if (x == 0 && i.VirtualAddress > rva)
                {
                    return i;
                }
                else if (i.VirtualAddress > rva && x > 1)
                {
                    return ImageSectionHeaders[x - 1];
                }
                x++;
            }
            return null;
        }
public static string GetSectionName(int rva)
        {
            //See which section contains the given rva, then return that section's own rva.
            int x = 0;
            foreach (var i in PEHeaders)
            {
                if (i.Value == rva)
                {
                    return i.Key;
                }
                else if (i.Value > rva)
                {
                    return PEHeaders.ElementAt(x - 1).Key;
                }
                x++;
            }
            return "Null";
        }
public static byte[] GetFileBytes(int size, int offset, string path)
        {
            if (offset < 0 || offset > FileSize)
            {
                ThrowInformation("Critical->GetFileBytes out of boundaries: " + offset, "ERROR");
                return null;
            }
            byte[] test = new byte[size];
            using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open)))
            {
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);
                reader.Read(test, 0, size);
            }
            return test;
        }
        public static int GetFieldOffset(Type str, string field)
        {
            return (int)Marshal.OffsetOf(str, field);
        }
