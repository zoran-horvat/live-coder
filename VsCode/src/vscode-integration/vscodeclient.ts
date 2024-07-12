import * as vscode from 'vscode';
import { Dialogs } from "../ide-integration/dialogs";
import { Environment } from "../ide-integration/environment";
import { Ide } from "../ide-integration/ide";
import { VsCodeDialogs } from "./dialogs";
import { VsCodeMemento } from "./vscodememento";

export class VsCodeClient extends Ide {
    private globalState: vscode.Memento;

    constructor(globalState: vscode.Memento) {
        super();
        this.globalState = globalState;
    }

	get dialogs() : Dialogs { return new VsCodeDialogs(); }
    get environment(): Environment { return new VsCodeMemento(this.globalState); }

    async withNoEditorsOpen() : Promise<void> {
        this.getAllTabs().map(this.toUri).forEach(await this.closeDocument);
    }

    private getAllTabs(): vscode.Tab[] {
        return vscode.window.tabGroups.all.map(group => group.tabs).flat();
    }

    private toUri(tab: vscode.Tab): vscode.Uri | undefined {
        if (tab.input instanceof vscode.TabInputText) {
            return (tab.input as vscode.TabInputText).uri;
        }
        return undefined;
    }

    private async closeDocument(uri: vscode.Uri | undefined): Promise<void> {
        if (!uri) { return; }
        await vscode.window.showTextDocument(uri, {preview: false, preserveFocus: false});
        await vscode.commands.executeCommand("workbench.action.closeActiveEditor");
    }
}