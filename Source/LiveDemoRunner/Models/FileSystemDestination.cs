﻿using System;
using System.Diagnostics.Contracts;
using System.IO;
using LiveDemoRunner.Interfaces;

namespace LiveDemoRunner.Models
{
    public class FileSystemDestination: IFutureDestination
    {

        private DirectoryInfo DeploymentDirectory { get; }

        public FileSystemDestination(DirectoryInfo directory)
        {

            Contract.Requires(directory != null, "Root directory must be non-null.");
            this.DeploymentDirectory = directory;

        }

        public void PrepareForDeployment() => this.CreateDeploymentDirectory();

        public IDestination GetDeploymentDestination() => new FileSystemDirectory(this.DeploymentDirectory);

        private void CreateDeploymentDirectory() => Directory.CreateDirectory(this.DeploymentDirectory.FullName);

    }
}
