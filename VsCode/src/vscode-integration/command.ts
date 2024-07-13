import * as vscode from 'vscode';

export class Command { 
	private _id: string;
	private _parameters: any[];

	constructor(id: string, parameters?: any[] | undefined) {
		this._id = id;
		this._parameters = parameters || [];
	}

	public async execute() : Promise<void> {
		await vscode.commands.executeCommand(this._id, ...this._parameters);
	}

	public static get closeAllEditors() : Command { return new Command('workbench.action.closeAllEditors'); }

	public static openWorkspace(fsPath: string) : Command { return new Command('vscode.openFolder', [vscode.Uri.file(fsPath), false]); }

	public static get collapseExplorerFolders() : Command { return new Command('workbench.files.action.collapseExplorerFolders'); }
}