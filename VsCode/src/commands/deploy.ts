import * as vscode from 'vscode';
import * as fs from '../fs/services';
import { Ide } from '../ide/ide';

export async function command(ide: Ide) {

	// Select the source directory
    const sourcePath = await ide.dialogs.selectDirectoryOrShowError('Select Source Directory', "No source directory selected.");
	if (!sourcePath) { return; }
    
	// Select the destination directory
    const destPath = await ide.dialogs.selectDirectoryOrShowError('Select Destination Directory', "No destination directory selected.");
	if (!destPath) { return; }

	// Copy the source directory to the destination directory
	fs.clearDirectoryRecursive(destPath);
    fs.copyDirectoryRecursive(sourcePath, destPath);

	// Open the destination directory in the current VS Code
	vscode.commands.executeCommand('vscode.openFolder', vscode.Uri.file(destPath), false);
}
