import { Ide } from "../../ide-integration/ide";
import { Specification } from "../specification";
import { FileSystem } from "../../fs-integration/filesystem";

export class WorkspaceOpen extends Specification {
	private fsPath: string;

	constructor(fsPath: string) {
		super();
		this.fsPath = fsPath;
	}

	public async ensure(ide: Ide, fs: FileSystem): Promise<void> {
		await ide.withWorkspaceOpen(this.fsPath);
	}
}
