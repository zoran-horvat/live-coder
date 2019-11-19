﻿using System;
using System.IO;
using LiveCoder.Deployer.Tool.Interfaces;

namespace LiveCoder.Deployer.Tool.Models
{
    internal class FileSystemDirectory: IDestination
    {

        private DirectoryInfo Directory { get; }

        public FileSystemDirectory(DirectoryInfo directory)
        {
            this.Directory = directory ?? throw new ArgumentNullException(nameof(directory));
        }

        public void DeployFile(string name, Stream stream)
        {
            using (BinaryWriter writer = new BinaryWriter(System.IO.File.Create(Path.Combine(this.Directory.FullName, name))))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                writer.Write(buffer, 0, buffer.Length);
            }
        }

        public void DeployDirectory(string name)
        {
            this.GetSubdirectory(name).Create();
        }

        public IDestination GetDirectory(string name)
        {
            return new FileSystemDirectory(this.GetSubdirectory(name));
        }

        public IDeployedComponent GetFile(string name)
        {
            string path = Path.Combine(this.Directory.FullName, name);
            FileInfo file = new FileInfo(path);
            return new DestinationFile(file);
        }

        public bool DirectoryExists(string name) => 
            System.IO.Directory.Exists(Path.Combine(this.Directory.FullName, name));

        public bool FileExists(string name) => 
            System.IO.File.Exists(Path.Combine(this.Directory.FullName, name));

        private string GetSubdirectoryPath(string name) => 
            $"{this.Directory.FullName}{Path.DirectorySeparatorChar}{name}";

        private DirectoryInfo GetSubdirectory(string name) => 
            new DirectoryInfo(this.GetSubdirectoryPath(name));
    }
}
