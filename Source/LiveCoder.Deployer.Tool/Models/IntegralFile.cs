﻿using System;
using System.Collections.Generic;
using System.IO;
using LiveCoder.Deployer.Tool.Interfaces;

namespace LiveCoder.Deployer.Tool.Models
{
    public class IntegralFile: File
    {
        private string SourceFileName { get; }

        public IntegralFile(FileInfo file, ILogger logger) : base(file, logger)
        {
            this.SourceFileName = file.Name;
        }

        public IntegralFile(FileInfo file, ILogger logger, Action<FileInfo> beforeDeploy) : base(file, logger, beforeDeploy)
        {
            this.SourceFileName = file.Name;
        }

        protected override IEnumerable<IDeployedComponent> Deploy(Stream stream, IDestination toDestination)
        {
            toDestination.DeployFile(this.SourceFileName, stream);
            yield return toDestination.GetFile(this.SourceFileName);
        }
    }
}