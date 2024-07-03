import * as vscode from 'vscode';
import { Dialogs } from "../ide/Dialogs";
import { Ide } from "../ide/Ide";
import { VsCodeDialogs } from "./VsCodeDialogs";

export class VsCode extends Ide {
	get dialogs() : Dialogs { return new VsCodeDialogs(); }

	showError(message: string): void {
		vscode.window.showErrorMessage(message);
	}
}