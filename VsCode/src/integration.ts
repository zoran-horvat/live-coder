import * as vscode from 'vscode';
import { FileSystem } from "./fs-integration/filesystem";
import { LocalFileSystem } from "./fs-integration/localfilesystem";
import { Environment } from "./ide-integration/environment";
import { Ide } from "./ide-integration/ide";
import { VsCodeClient } from "./vscode-integration/vscodeclient";
import { VsCodeMemento } from "./vscode-integration/vscodememento";

export class Integration {
    private static ideInstance: Ide;
	private static fsInstanced: FileSystem;
    private static environmentInstance: Environment;
    private static globalState: vscode.Memento;

    constructor(globalState: vscode.Memento) {
        Integration.globalState = globalState;
    }

    get ide(): Ide {
        if (!Integration.ideInstance) {
            Integration.ideInstance = new VsCodeClient(Integration.globalState);
        }
        return Integration.ideInstance;
    }

	get fs(): FileSystem {
		if (!Integration.fsInstanced) {
			Integration.fsInstanced = new LocalFileSystem();
		}
		return Integration.fsInstanced;
	}

    get environment(): Environment {
        if (!Integration.environmentInstance) {
            Integration.environmentInstance = new VsCodeMemento(Integration.globalState);
        }
        return Integration.environmentInstance;
    }
}
