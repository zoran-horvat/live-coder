import { FileSystem } from "../fs-integration/filesystem";
import { Environment } from "../ide-integration/environment";
import { Ide } from "../ide-integration/ide";
import * as vscode from 'vscode';

export async function command(ide: Ide, fs: FileSystem, environment: Environment) : Promise<void> {
	const filePath = await resolveActualScript(fs);
	if (!filePath) {
		await ide.dialogs.showErrorMessage('No Live Coder script found. Try deploying the demo again.');
		return;
	}
	await ide.editDocument(filePath);
}

async function resolveActualScript(fs: FileSystem) : Promise<any | null> {
	const localScriptPath = await getLocalScriptPath(fs);
	if (!localScriptPath) return null;

	const script = await fs.loadJsonFile(localScriptPath);

	if (script && script.script && script.script.redirect) return script.script.redirect as string;
	return null;
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