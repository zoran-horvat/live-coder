export abstract class FileSystem {
	abstract deployDemo(sourcePath: string, destinationPath: string) : void;
	abstract clearDirectoryRecursive(directoryPath: string) : void;
}