import * as deploy from './commands/deploy';
import { FileSystem } from './fs/FileSystem';
import { Ide } from './ide/Ide';
import { Integration } from './Integration';

export class Commands {
    private ide: Ide;
    private fs: FileSystem

    constructor(ide: Ide, fs: FileSystem) {
        this.ide = ide;
        this.fs = fs;
    }

    public get deploy() { return () => this.safe(() => deploy.command(this.ide, this.fs)); }

    private async safe(f: () => Thenable<void>) {
        try {
            await f();
        } catch (error) {
            Integration.ide.showError(`Error: ${error instanceof Error ? error.message : String(error)}`);
        }
    }
}
