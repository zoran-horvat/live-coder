import * as vscode from 'vscode';
import * as deploy from './commands/deploy';
import { Ide } from './ide/ide';
import { VsCode } from './vscode/vscode';

export function activate(context: vscode.ExtensionContext) {
    const commands = new Commands(Integration.ide);

    pushCommand(context, 'demo.deploy', commands.deploy);
}

export function deactivate() {}

function pushCommand(context : vscode.ExtensionContext, command : string, callback : () => Thenable<void>) {
    const disposable = vscode.commands.registerCommand(command, callback);
    context.subscriptions.push(disposable);
}

async function safe(f: () => Thenable<void>) {
    try {
        await f();
    } catch (error) {
        Integration.ide.showError(`Error: ${error instanceof Error ? error.message : String(error)}`);
    }
}

class Commands {
    private ide : Ide;

    constructor(ide: Ide) {
        this.ide = ide;
    }

    public get deploy() { return () => safe(() => deploy.command(this.ide)); }
}

class Integration {
    private static ideInstance : Ide;

    static get ide() : Ide {
        if (!Integration.ide) {
            Integration.ideInstance = new VsCode();
        }
        return Integration.ide;
    }
}
