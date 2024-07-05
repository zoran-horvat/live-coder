import { FileSystem } from "./fs-integration/filesystem";
import { LocalFileSystem } from "./fs-integration/localfilesystem";
import { Ide } from "./ide-integration/ide";
import { VsCodeClient } from "./vscode-integration/vscodeclient";

export class Integration {
    private static ideInstance: Ide;
	private static fsInstanced: FileSystem;

    static get ide(): Ide {
        if (!Integration.ideInstance) {
            Integration.ideInstance = new VsCodeClient();
        }
        return Integration.ideInstance;
    }

	static get fs(): FileSystem {
		if (!Integration.fsInstanced) {
			Integration.fsInstanced = new LocalFileSystem();
		}
		return Integration.fsInstanced;
	}
}
