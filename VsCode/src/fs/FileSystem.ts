export abstract class FileSystem
{
	abstract clearDirectoryRecursive(directory: string): void;
	abstract copyDemo(source: string, destination: string): void;
}