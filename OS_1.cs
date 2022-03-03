using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Collections.Generic;
using System.Xml;

namespace OS_1
{
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Выберите действие:\n");
            Console.WriteLine("1 - вывести информацию о файловой системе\n");
            Console.WriteLine("2 - работа c текстовым файлом\n");
            Console.WriteLine("3 - архивировать\n");
            Console.WriteLine("4 - работа с файлом формата json\n");
            Console.WriteLine("5 - работа с файлом формата xml\n");
            Console.WriteLine("0 - закончить работу\n");
            string choice = Console.ReadLine();
            while (choice != "0")
                switch (choice)
                {
                    case "1":
                        Program.Filesystem();
                        Console.WriteLine("Выберите действие:\n");
                        choice = Console.ReadLine();
                    break;
                    case "2":
                        Program.Files();
                        Console.WriteLine("Выберите действие:\n");
                        choice = Console.ReadLine();
                        break;
                    case "3":
                        Program.Zip();
                        Console.WriteLine("Выберите действие:\n");
                        choice = Console.ReadLine();
                        break;
                    case "4":
                        Program.Filejson();
                        Console.WriteLine("Выберите действие:\n");
                        choice = Console.ReadLine();
                        break;
                    case "5":
                        Program.Filexml();
                        Console.WriteLine("Выберите действие:\n");
                        choice = Console.ReadLine();
                        break;
                    case "0":
                        break;
                }
        }
            static void Filesystem()
            {
                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (DriveInfo drive in drives)
                {
                    Console.WriteLine($"Название: {drive.Name}");
                    Console.WriteLine($"Тип: {drive.DriveType}");
                    if (drive.IsReady)
                    {
                        Console.WriteLine($"Объем диска: {drive.TotalSize}");
                        Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                        Console.WriteLine($"Метка: {drive.VolumeLabel}");
                        Console.WriteLine($"Имя файловой системы: {drive.DriveFormat}");
                    }
                    Console.WriteLine();
                }
            }
            static void Files()
            {
                // создаем каталог для файла
                string path = "D:\\SomeDir";
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                Console.WriteLine("Введите строку для записи в файл:");
                string text = Console.ReadLine();

                // запись в файл
                using (FileStream fstream = new FileStream($"{path}/note", FileMode.OpenOrCreate))
                {
                    // преобразуем строку в байты
                    byte[] array = System.Text.Encoding.Default.GetBytes(text);
                    // запись массива байтов в файл
                    fstream.Write(array, 0, array.Length);
                    Console.WriteLine("Текст записан в файл");
                }

                // чтение из файла
                using (FileStream fstream = File.OpenRead($"{path}/note"))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[fstream.Length];
                    // считываем данные
                    fstream.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    string textFromFile = System.Text.Encoding.Default.GetString(array);
                    Console.WriteLine($"Текст из файла: {textFromFile}");
                }

                string dirName = "D:\\SomeDir";

                Console.WriteLine("Удалить каталог? 0 - нет, 1 - да");
                DirectoryInfo dirInfo1 = new DirectoryInfo(dirName);
                if (dirInfo.Exists)
                {
                    string a = Console.ReadLine();
                    switch (a)
                    {
                        case "1":
                            try
                            {
                                dirInfo1.Delete(true);
                                Console.WriteLine("Каталог удален");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case "0":
                            Console.WriteLine("Каталог сохранен");
                            break;
                    }
                }
             }

            static void Zip()
            {
                string sourceFile = "D://book.txt";
                string compressedFile = "D://test.zip";
                string targetFile = "D://book_new.txt";


                Compress(sourceFile, compressedFile);
                Decompress(compressedFile, targetFile);

                string path = @"D:\book_new.txt";
                string path1 = "D://test.zip";
                FileInfo fileInf = new FileInfo(path);
                FileInfo fileInf1 = new FileInfo(path1);
                if (fileInf.Exists)
                {
                    Console.WriteLine("Имя файла: {0}", fileInf.Name);
                    Console.WriteLine("Время создания: {0}", fileInf.CreationTime);
                    Console.WriteLine("Размер: {0}", fileInf.Length);
                }

                Console.WriteLine("Удалить файл? 0 - нет, 1 - да");
                if (fileInf.Exists)
                {
                    string a = Console.ReadLine();
                    switch (a)
                    {
                        case "1":
                            fileInf.Delete();
                            Console.WriteLine("Файл удален");
                            break;
                        case "0":
                            Console.WriteLine("Файл сохранен");
                            break;
                    }
                }

                Console.WriteLine("Удалить архив? 0 - нет, 1 - да");

                if (fileInf1.Exists)
                {


                    string a = Console.ReadLine();
                    switch (a)
                    {
                        case "1":
                            fileInf1.Delete();
                            Console.WriteLine("Архив удален");
                            break;
                        case "2":
                            Console.WriteLine("Архив сохранен");
                            break;
                    }

                }
            }
            static void Compress(string sourceFile, string compressedFile)
            {
                // поток для чтения исходного файла
                using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
                {
                    // поток для записи сжатого файла
                    using (FileStream targetStream = File.Create(compressedFile))
                    {
                        // поток архивации
                        using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                        {
                            sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                            Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                            sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
                        }
                    }
                }
            }
            static void Decompress(string compressedFile, string targetFile)
            {
                // поток для чтения из сжатого файла
                using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
                {
                    // поток для записи восстановленного файла
                    using (FileStream targetStream = File.Create(targetFile))
                    {
                        // поток разархивации
                        using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(targetStream);
                            Console.WriteLine("Восстановлен файл: {0}", targetFile);
                        }
                    }
                }
            }
            static void Filejson()
            {
                var Person = new Person
                {
                    Name = "Tom",
                    Age = 35
                };
                string fileName = @"D:\note.json";
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(Person, options);
                File.WriteAllText(fileName, jsonString);
                Console.WriteLine(jsonString);

                string path = @"D:\note.json";
                FileInfo fileInf = new FileInfo(path);

                Console.WriteLine("Удалить файл? 0 - нет, 1 - да");
                if (fileInf.Exists)
                {
                    string a = Console.ReadLine();
                    switch (a)
                    {
                        case "1":
                            fileInf.Delete();
                            Console.WriteLine("Файл удален");
                            break;
                        case "0":
                            Console.WriteLine("Файл сохранен");
                            break;
                }
            }
        }
            static void Filexml()
            {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("D://users.xml");
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            // обход всех узлов в корневом элементе
            XmlElement userElem = xDoc.CreateElement("user");
            // создаем атрибут name
            XmlAttribute nameAttr = xDoc.CreateAttribute("name");
            // создаем элементы company и age
            XmlElement companyElem = xDoc.CreateElement("company");
            XmlElement ageElem = xDoc.CreateElement("age");
            // создаем текстовые значения для элементов и атрибута
            Console.WriteLine("Введите имя: ");
            string name = Console.ReadLine();
            Console.WriteLine("Введите компанию: ");
            string company = Console.ReadLine();
            Console.WriteLine("Введите возраст: ");
            string age = Console.ReadLine();
            XmlText nameText = xDoc.CreateTextNode(name);
            XmlText companyText = xDoc.CreateTextNode(company);
            XmlText ageText = xDoc.CreateTextNode(age);

            //добавляем узлы
            nameAttr.AppendChild(nameText);
            companyElem.AppendChild(companyText);
            ageElem.AppendChild(ageText);
            userElem.Attributes.Append(nameAttr);
            userElem.AppendChild(companyElem);
            userElem.AppendChild(ageElem);
            xRoot.AppendChild(userElem);
            xDoc.Save("D://users.xml");

            foreach (XmlNode xnode in xRoot)
            {
                // получаем атрибут name
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");
                    if (attr != null)
                        Console.WriteLine(attr.Value);
                }
                // обходим все дочерние узлы элемента user
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    // если узел - company
                    if (childnode.Name == "company")
                    {
                        Console.WriteLine($"Компания: {childnode.InnerText}");
                    }
                    // если узел age
                    if (childnode.Name == "age")
                    {
                        Console.WriteLine($"Возраст: {childnode.InnerText}");
                    }
                }
                Console.WriteLine();
            }
            string path = @"D:\users.xml";
            FileInfo fileInf = new FileInfo(path);

            Console.WriteLine("Удалить файл? 0 - нет, 1 - да");
            if (fileInf.Exists)
            {
                string a = Console.ReadLine();
                switch (a)
                {
                    case "1":
                        fileInf.Delete();
                        Console.WriteLine("Файл удален");
                        break;
                    case "0":
                        Console.WriteLine("Файл сохранен");
                        break;
                }
            }
            }
    }
}
