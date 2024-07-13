import { FileSystem } from "../../fs-integration/filesystem";
import { Ide } from "../../ide-integration/ide";
import { Specification } from "../specification";

export class NoEditorsOpen extends Specification {
	public async ensure(ide: Ide, fs: FileSystem): Promise<void> {
		await ide.withNoEditorsOpen();
	}
}