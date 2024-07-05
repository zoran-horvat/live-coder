import * as vscode from 'vscode';
import { Commands } from './commands';
import { Integration } from './integration';

export function activate(context: vscode.ExtensionContext) {
    const commands = new Commands(Integration.ide, Integration.fs);
    pushCommand(context, 'demo.deploy', commands.deploy);
}

export function deactivate() {}

function pushCommand(context : vscode.ExtensionContext, command : string, callback : () => Thenable<void>) {
    const disposable = vscode.commands.registerCommand(command, callback);
    context.subscriptions.push(disposable);
}
