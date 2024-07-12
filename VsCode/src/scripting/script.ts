import { Ide } from "../ide-integration/ide";
import { FileSystem } from "../fs-integration/filesystem";
import { Instruction } from "./instruction";

export class Script {
	public instructions: Instruction[] = [];

    public execute(ide: Ide, fs: FileSystem): void {
		this.instructions.forEach(instruction => {
			instruction.execute(ide, fs);
		});
	}

	public append(instruction: Instruction): void {
		this.instructions.push(instruction);
	}
}