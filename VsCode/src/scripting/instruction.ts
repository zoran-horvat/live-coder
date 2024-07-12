import { Ide } from "../ide-integration/ide";
import { FileSystem } from "../fs-integration/filesystem";

export abstract class Instruction {
	public abstract execute(ide: Ide, fs: FileSystem) : Promise<void>;
}