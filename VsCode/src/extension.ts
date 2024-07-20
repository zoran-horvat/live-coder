import * as vscode from 'vscode';
import { Commands } from './commands';
import { Integration } from './integration';

export function activate(context: vscode.ExtensionContext) {
    const commands = new Commands(new Integration(context.globalState));
    pushCommand(context, 'livecoder.deploy', commands.deploy);
    pushCommand(context, 'livecoder.record', commands.record);
    pushCommand(context, 'livecoder.edit', commands.edit);
}

export function deactivate() {}

function pushCommand(context : vscode.ExtensionContext, command : string, callback : () => Thenable<void>) {
    const disposable = vscode.commands.registerCommand(command, callback);
    context.subscriptions.push(disposable);
}
