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

## Steps to create snippets
Presentation should contain a single file with extension `.snippet`, which is an XML file in the Visual Studio snippets format. Note that, although Visual Studio only supports one snippet per file, this file should contain all snippets. The deploy.exe tool will then split these snippets into separate files.
Snippets should be named snp01, snp02, etc. It is allowed to add a blank space before the number (e.g. snp 01). This is useful when one wants to execute snippets manually. In that case, the `snp` word should be registered under Tools -> Options -> Environment -> Task List, and then Visual Studio will show upcoming steps in the Task List window (View -> Task List). This method of execution a demo, however, is discouraged. Task List window used to fail to sort items, which made it difficult to discover the next step in the presentation. Also, Visual Studio and extensions may intercept snippets and try to format them, which might not produce the desired look of the code. We strongly advise the use of PreZenter's Next command to execute demo steps.
There is a special form of snippets which represents a non-executable comment. They are writeen as snp01.1, snp01.2, etc. These snippets are only meant to remind the speaker. When the tool "executes" them, the line that contains the snippet is simply deleted. Make sure that nothing else is located on the line which contains the comment snippet, or otherwise that additional content would be deleted, too.
