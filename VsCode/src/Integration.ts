import { Ide } from './ide/Ide';
import { FileSystem } from './fs/FileSystem';
import { LocalFileSystem } from './fs/LocalFileSystem';
import { VsCode } from './vscode/vscode';

export class Integration {
    private static ideInstance: Ide;
    private static fsInstance: FileSystem;

    static get ide(): Ide {
        if (!Integration.ideInstance) {
            Integration.ideInstance = new VsCode();
        }
        return Integration.ide;
    }

    static get fs(): FileSystem {
        if (!Integration.fsInstance) {
            Integration.fsInstance = new LocalFileSystem();
        }
        return Integration.fs;
    }
}
