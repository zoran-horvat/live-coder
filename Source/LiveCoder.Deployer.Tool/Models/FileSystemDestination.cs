﻿using System;
using System.IO;
using LiveCoder.Deployer.Tool.Interfaces;

namespace LiveCoder.Deployer.Tool.Models
{
    public class FileSystemDestination: IFutureDestination
    {

        private DirectoryInfo DeploymentDirectory { get; }

        public FileSystemDestination(DirectoryInfo directory)
        {
            this.DeploymentDirectory = directory ?? throw new ArgumentNullException(nameof(directory));
        }

        public void PrepareForDeployment() => this.CreateDeploymentDirectory();

        public IDestination GetDeploymentDestination() => new FileSystemDirectory(this.DeploymentDirectory);

        private void CreateDeploymentDirectory() => Directory.CreateDirectory(this.DeploymentDirectory.FullName);

    }
}