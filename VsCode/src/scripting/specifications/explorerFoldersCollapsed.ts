import { Ide } from "../../ide-integration/ide";
import { Specification } from "../specification";
import { FileSystem } from "../../fs-integration/filesystem";

export class ExplorerFoldersCollapsed extends Specification {
	public async ensure(ide: Ide, fs: FileSystem): Promise<void> {
		await ide.withExplorerFoldersCollapsed();
	}
}