import * as vscode from 'vscode';
import * as deploy from './commands/deploy';
import * as recorddemo from './commands/recorddemo';
import * as editscript from './commands/editscript';
import { Integration } from './integration';

export class Commands {
    private integration: Integration;

    constructor(integration: Integration) {
        this.integration = integration;
    }

    private get ide() { return this.integration.ide; }
    private get fs() { return this.integration.fs; }
    private get environment() { return this.integration.environment; }

    public get deploy() { return () => this.safe(() => deploy.command(this.ide, this.fs, this.environment)); }
    public get record() { return () => this.safe(() => recorddemo.command(this.ide, this.fs, this.environment)); }
    public get edit() { return () => this.safe(() => editscript.command(this.ide, this.fs, this.environment)); }

    private async safe(f: () => Promise<void>) {
        try {
            await f();
        } catch (error) {
            vscode.window.showErrorMessage(`Error: ${error instanceof Error ? error.message : String(error)}`);
        }
    }
}