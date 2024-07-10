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
}