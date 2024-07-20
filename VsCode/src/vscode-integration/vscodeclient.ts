import * as vscode from 'vscode';
import { Dialogs } from "../ide-integration/dialogs";
import { Environment } from "../ide-integration/environment";
import { Ide } from "../ide-integration/ide";
import { VsCodeDialogs } from "./dialogs";
import { VsCodeMemento } from "./vscodememento";
import { Command } from './command';

export class VsCodeClient extends Ide {
    private globalState: vscode.Memento;

    constructor(globalState: vscode.Memento) {
        super();
        this.globalState = globalState;
    }

	get dialogs() : Dialogs { return new VsCodeDialogs(); }
    get environment(): Environment { return new VsCodeMemento(this.globalState); }

    async withNoEditorsOpen() : Promise<void> { await Command.closeAllEditors.execute(); }

    async withWorkspaceOpen(fsPath: string) : Promise<void> { await Command.openWorkspace(fsPath).execute(); }

    async withExplorerFoldersCollapsed() : Promise<void> { await Command.collapseExplorerFolders.execute(); }

    async withScriptEditorActive(): Promise<void> { await Command.scriptEditorActive.execute(); }

    async editDocument(path: string): Promise<void> {
        const document = await vscode.workspace.openTextDocument(path);
        await vscode.window.showTextDocument(document);
    }
}