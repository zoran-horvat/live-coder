export abstract class FileSystem {
	abstract deployDemo(sourcePath: string, destinationPath: string) : Promise<void>;
	abstract clearDirectoryRecursive(directoryPath: string) : Promise<void>;
	abstract ensureDirectoryExists(fullPath: string) : Promise<string>;
	abstract ensureDirectoryExists(root: string, directory?: string) : Promise<string>;
	abstract ensureTextFileExists(root: string, fileName: string, defaultContent: string) : Promise<string>;
	abstract ensureJsonFileExists(path: string, fileName?: string, defaultContent?: string) : Promise<string>;	
	abstract getExistingFilePath(root: string, fileName: string) : Promise<string | null>;
	abstract loadJsonFile(path: string) : Promise<any | null>;
}