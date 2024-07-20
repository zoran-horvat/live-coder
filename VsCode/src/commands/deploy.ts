import * as vscode from 'vscode';
import { Ide } from '../ide-integration/ide';
import { FileSystem } from '../fs-integration/filesystem';
import { Environment } from '../ide-integration/environment';
import { Script } from '../scripting/script';
import { NoEditorsOpen } from '../scripting/specifications/noeditorsopen';
import { WorkspaceOpen } from '../scripting/specifications/workspaceopen';
import { ExplorerFoldersCollapsed } from '../scripting/specifications/explorerFoldersCollapsed';

export async function command(ide: Ide, fs: FileSystem, environment: Environment) {
	
	// Select the source directory
    const sourcePath = await ide.dialogs.selectDirectoryOrShowError('Select Source Directory', "No source directory selected.", environment.lastSourcePath);
	if (!sourcePath) { return; }
    
	// Select the destination directory
    const destPath = await ide.dialogs.selectDirectoryOrShowError('Select Destination Directory', "No destination directory selected.", environment.lastDestPath);
	if (!destPath) { return; }

    environment.lastSourcePath = sourcePath;
    environment.lastDestPath = destPath;

	// Copy the source directory to the destination directory
	await fs.clearDirectoryRecursive(destPath, async (dir, err) => {
		await fs.deployDemo(sourcePath, dir);
		await executePrelude(ide, fs, dir);
	});
}

async function executePrelude(ide: Ide, fs: FileSystem, workspacePath: string) : Promise<void> {
	
	let prelude = new Script();
	prelude.append(new NoEditorsOpen());
	prelude.append(new ExplorerFoldersCollapsed());
	prelude.append(new WorkspaceOpen(workspacePath));
	
	await prelude.execute(ide, fs);
}
