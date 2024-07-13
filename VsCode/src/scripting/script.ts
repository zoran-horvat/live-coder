import { Ide } from "../ide-integration/ide";
import { FileSystem } from "../fs-integration/filesystem";
import { Specification } from "./specification";

export class Script {
	public instructions: Specification[] = [];

    public execute(ide: Ide, fs: FileSystem): void {
		this.instructions.forEach(instruction => {
			instruction.ensure(ide, fs);
		});
	}

	public append(instruction: Specification): void {
		this.instructions.push(instruction);
	}
}