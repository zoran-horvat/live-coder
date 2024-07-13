import * as vscode from 'vscode';

export class Command { 
	private _id: string;

	constructor(id: string) {
		this._id = id;
	}

	public async execute() : Promise<void> {
		await vscode.commands.executeCommand(this._id);
	}

	public static get closeAllEditors() : Command { return new Command('workbench.action.closeAllEditors'); }
}