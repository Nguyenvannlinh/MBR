using System;
using System.Text;
using System.IO;
namespace Master_boot_Record
{
    class partition
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            string path = @"D:\\C#\\hệ điều hành\\Disk Images\\FAT16.vhd";
            BinaryReader f;
            try
            {
                f = new BinaryReader(new FileStream(path, FileMode.Open));
            }
            catch
            {
                Console.Error.WriteLine();
                return;
            }
            byte[] data = new byte[512];
            try
            {
                for (int i = 0; i < 512; i++)
                {
                    data[i] = f.ReadByte();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            f.Close();
            int count = 0;
            int[] o = {0x1BE, 0x1CE, 0x1DE, 0x1EE, 0x1FE};
            for (int i = o.Length; i < 0x200; i++)
            {
                Console.Write(data[i].ToString() + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < 4; i++)
            {   
                Console.WriteLine();
                Console.WriteLine($"Thông tin trên partition {++count}");
                string a;
                if (data[o[i]] == 0x80)
                {
                    a = ("Yes");
                }
                else { a = ("No"); }
                Console.Write($"\n  -partition có khả năng hoạt khởi động: {a}");
                Console.Write($"\n  -Head bắt đầu: {data[o[i] + 1]}");
                Console.Write($"\n  -Cylinder bắt đầu: {(data[o[i] + 2] & 0xC0 << 2) + data[o[i] + 3]}");
                Console.Write($"\n  -Sector bắt đầu: {data[o[i] + 2] & 0x3F}");

                string b = "";
                switch (data[o[i] + 4])
                {
                    case 0:
                        b = "Non-pos";
                        break;
                    case 2:
                        b = "FAT 12";
                        break;
                    case 5:
                        b = "extended";
                        break;
                    case 6:
                        b = "FAT 16";
                        break;
                    case 4:
                        b = "FAT 16";
                        break;
                    case 11:
                        b = "FAT 32";
                        break;
                    case 27:
                        b = "FAT 32";
                        break;
                }
                Console.Write($"\n  -Hệ thông file sử dùng trên partition {b}");


                Console.Write($"\n  -Head kết thúc: {data[o[i] + 5]}");

                Console.Write($"\n  -Cylinder kết thúc: {((data[o[i] + 6] & 0xC0) << 1) + data[o[i] + 7]}");

                Console.Write($"\n  -Sector kết thúc: {data[o[i] + 6] & 0x3F}");
                int relec = data[o[i] + 8] + (data[o[i] + 9] << 8) + (data[o[i] + 10] << 16) + (data[o[i] + 11] << 24);
                Console.Write($"\n  -Số sector tương đối bắt đầu: {relec}");
                int sumsec = data[o[i] + 12] + (data[o[i] + 13] << 8) + (data[o[i] + 14] << 16) + (data[o[i] + 15] << 24);
                Console.Write($"\n  -tổng sector: {sumsec}");
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}