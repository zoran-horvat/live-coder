export abstract class FileSystem
{
	abstract clearDirectoryRecursive(directory: string): void;
	abstract copySourceCode(source: string, destination: string): void;
}