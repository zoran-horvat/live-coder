import * as vscode from 'vscode';
import { Dialogs } from '../ide-integration/dialogs';

export class VsCodeDialogs extends Dialogs {

    async selectDirectoryOrShowError(prompt: string, errorMessage: string, initialPath: string | undefined) {
        const uri = await this.selectDirectory(prompt, initialPath);
        if (!uri) { vscode.window.showErrorMessage(errorMessage); }
        return uri ? uri.fsPath : undefined;
    }

    async selectDirectory(prompt: string, initialPath: string | undefined) {
        const options: vscode.OpenDialogOptions = {
            canSelectFiles: false,
            canSelectFolders: true,
            canSelectMany: false,
            openLabel: prompt
        };

        if (initialPath) {
            options.defaultUri = vscode.Uri.file(initialPath);
        }

        const uris = await vscode.window.showOpenDialog(options);
        return uris && uris.length > 0 ? uris[0] : undefined;
    }

    async showInformationMessage(message: string) {
        await vscode.window.showInformationMessage(message);
    }

    async showErrorMessage(message: string) {
        await vscode.window.showErrorMessage(message);
    }
}