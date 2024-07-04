import * as vscode from 'vscode';
import { Dialogs } from '../ide-integration/dialogs';

export class VsCodeDialogs extends Dialogs {

    async selectDirectoryOrShowError(prompt: string, errorMessage: string) {
        const uri = await this.selectDirectory(prompt);
        if (!uri) { vscode.window.showErrorMessage(errorMessage); }
        return uri ? uri.fsPath : undefined;
    }

    async selectDirectory(prompt: string) {
        const options: vscode.OpenDialogOptions = {
            canSelectFiles: false,
            canSelectFolders: true,
            canSelectMany: false,
            openLabel: prompt
        };

        const uris = await vscode.window.showOpenDialog(options);
        return uris && uris.length > 0 ? uris[0] : undefined;
    }
}