import * as vscode from 'vscode';
import * as deploy from './commands/deploy';
import { Ide } from "./ide-integration/ide";
import { FileSystem } from './fs-integration/filesystem';

export class Commands {
    private ide : Ide;
	private fs : FileSystem;

    constructor(ide: Ide, fs: FileSystem) {
        this.ide = ide;
		this.fs = fs;
    }

    public get deploy() { return () => this.safe(() => deploy.command(this.ide, this.fs)); }

    private async safe(f: () => Promise<void>) {
        try {
            await f();
        } catch (error) {
            vscode.window.showErrorMessage(`Error: ${error instanceof Error ? error.message : String(error)}`);
        }
    }
}