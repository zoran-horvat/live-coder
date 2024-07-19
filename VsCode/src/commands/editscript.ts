import { FileSystem } from "../fs-integration/filesystem";
import { Environment } from "../ide-integration/environment";
import { Ide } from "../ide-integration/ide";
import * as vscode from 'vscode';

export async function command(ide: Ide, fs: FileSystem, environment: Environment) : Promise<void> {
	const filePath = await getLocalScriptPath(fs);
	if (!filePath) {
		await ide.dialogs.showErrorMessage('No Live Coder script found. Try deploying the demo again.');
		return;
	}
	console.log('Edit script command in ' + filePath);
}

async function getLocalScriptPath(fs: FileSystem) : Promise<string | null> {
	const workspace = await getWorkspaceDirectory();
	if (! workspace) { return null; }
	return await fs.getExistingFilePath(workspace, '.live-coder/demo.lcs');
}

async function getWorkspaceDirectory() : Promise<string | null> {
	return !vscode.workspace.workspaceFolders ? null
		: vscode.workspace.workspaceFolders.length === 0 ? null
		: vscode.workspace.workspaceFolders[0].uri.fsPath;
}