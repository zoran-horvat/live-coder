# PreZenter

A toolset which helps deploy and hold a live coding session using Visual Studio. The toolset consists of the deployment tool (deploy.exe), and Visual Studio extension (PreZenter).

Solution consists of these projects:
- LiveDemoRunner - the deployment tool. Builds into Windows command-line executable application.
- VSExtension - the extension which plays snippets deployed by the deployment tool.
- Demo - an example project which includes PowerPoint presentation and snippets file. This project can be deployed and demonstrated using PreZenter.

## Steps to see the tools in action
After building VSExtension project, it should create prezenter.vsix in the build output directory. Install this extension to Visual Studio. The tool was tested in Visual Studio 2017 and 2019 Community Edition.

After building LiveDemoRunner, it will produce deploy.exe tool in the output directory. Move this tool to a location in PATH, so that you can execute it from source code directories.

With both tools built and installed, the next step is to try a live coding demo on the Demo project. Open command prompt and (optionally) change directory to the directory containing Demo.sln. This directory contains:
- Demo.sln and Demo directory with the project
- demo.snippets with all snippets for the demonstration. Snippets are named snp01, snp02, snp03...
- slides.pptx - a sample PowerPoint presentation

The first step in presenting is to deploy all the materials. That is done by using `deploy -src <dir>` and then adding specialized options. When executed in the directory which already contains all the files to be deployed, begin with `deploy -src .`.

You can explore deployment options by simply typing `deploy` - that will print out the current list of flags with explanations.

In a typical scenario, when beginning a presentation, one would copy all files and open them:
    deploy -src . -copy-all -open
This command, when executed, will first create a destination directory under `C:\Demo`. It will then copy all files in the current directory and its subdirectories. Finally, a `.sln` file and any `*.pptx` files will be open using the default handlers for their file types.

Once the deployment is done, simply use the `Next` command installed by the PreZenter Visual Studio extension to execute the demo one step at a time. It is advised to assign a keyboard shortcut to the `Next` command. Suggested shortcut is Ctrl+Shift+A.
