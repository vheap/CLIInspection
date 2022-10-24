public static bool ParseSectionHeaders()
        {
            string fPath = _ImagePath;
            int impSectionAddress = 0;
            int impSectionRawPointer = 0;
            int vAddress = 0;
            int vSize = 0;
            int secval = 0;
            int offset = 264; 
            //---- Bit check
            if (is64Bit)
            {
                offset = 264;
                vAddress = (int)ImageNTHeader64.OptionalHeader.DataDirectory[1].VirtualAddress;
                vSize = (int)ImageNTHeader64.OptionalHeader.DataDirectory[1].Size;
                ImageSectionHeaders = new IMAGE_SECTION_HEADER[ImageNTHeader64.FileHeader.NumberOfSections];
            }
            else if (!is64Bit)
            {
                offset = 248;
                vAddress = (int)ImageNTHeader32.OptionalHeader.DataDirectory.VirtualAddress;
                vSize = (int)ImageNTHeader32.OptionalHeader.DataDirectory.Size;
                ImageSectionHeaders = new IMAGE_SECTION_HEADER[ImageNTHeader32.FileHeader.NumberOfSections];
            }
            Console.WriteLine(Environment.NewLine + "[] ---- Image Sections ----- []");
			
            int impOffset = 0;
            for (int headerNo = 0; headerNo < ImageSectionHeaders.Length; ++headerNo)
            {
                ImageSectionHeaders[headerNo] = fromBytes<IMAGE_SECTION_HEADER>(ImageBytes.Skip(ImageDOS.e_lfanew + offset + secval).ToArray());

                //---- Exporting the name and VA into a dictonary.
                string SectionName = new string(ImageSectionHeaders[headerNo].Name);
                PEHeaders[SectionName] = (int)ImageSectionHeaders[headerNo].VirtualAddress;
				
                ThrowInformation(SectionName + " - RVA: " + ImageSectionHeaders[headerNo].VirtualAddress, "Section");
                secval = secval + 40; //Iteration offset.
            }
            Console.WriteLine("[] ------------------------- []");
            Console.WriteLine(Environment.NewLine);

            //---- Import Table Offset
            IMAGE_SECTION_HEADER importHeader = new IMAGE_SECTION_HEADER();
            bool SectionExists = false;
            if (GetSectionRVAObj(vAddress) == null)
            {
                foreach (var i in ImageSectionHeaders)
                {
                    if (i.VirtualAddress == vAddress)
                    {
                        importHeader = i;
                        SectionExists = true;
                    }
                }

                if (!SectionExists)
                {
                    //Couldn't find the section using VA.
                    ThrowInformation("Could not parse section's VA", "ERROR");
                    return false;
                }
            }
            else
            {
                importHeader = (IMAGE_SECTION_HEADER)GetSectionRVAObj(vAddress);
            }
		}