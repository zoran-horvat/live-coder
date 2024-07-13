import { Ide } from "../ide-integration/ide";
import { FileSystem } from "../fs-integration/filesystem";
import { Specification } from "./specification";

export class Script {
	public instructions: Specification[] = [];

    public async execute(ide: Ide, fs: FileSystem): Promise<void> {
		this.instructions.forEach(async instruction => {
			await instruction.ensure(ide, fs);
		});
	}

	public append(instruction: Specification): void {
		this.instructions.push(instruction);
	}
}