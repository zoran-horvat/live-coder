import { Ide } from "../ide-integration/ide";
import { FileSystem } from "../fs-integration/filesystem";

export abstract class Specification {
	public abstract ensure(ide: Ide, fs: FileSystem) : Promise<void>;
}