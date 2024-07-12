import { FileSystem } from "../../fs-integration/filesystem";
import { Ide } from "../../ide-integration/ide";
import { Instruction } from "../instruction";

export class NoEditorsOpen extends Instruction {
	public async execute(ide: Ide, fs: FileSystem): Promise<void> {
		await ide.withNoEditorsOpen();
	}
}